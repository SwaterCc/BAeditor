#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

namespace Hono.Scripts.Battle
{
    public class MonsterGeneratorLogic : ActorLogic, IAPoolObject
    {
        private class GenActorInfo
        {
            public int ConfigId;
            public EActorType ActorType;
        }

        private readonly MonsterGenEventChecker _checker;
        private bool _isGeneratorActive;
        private MonsterGenerateTmpTable.MonsterGenerateTmpRow _templateRow;
        private float _duration;
        private float _createMonsterInterval;
        private readonly Queue<GenActorInfo> _createMonsterQueue = new(64);
        private List<Vector3> _wayPoints = new(16);
        private readonly List<int> _curMonsterUid = new(64);
        private Rect _rect;
        private MonsterGenRtEventInfo _rtEventInfo;
        private int _beforeConfigId;
        public static int CurMonsterCount;

        public MonsterGeneratorLogic()
        {
            _checker = new MonsterGenEventChecker(EBattleEventType.OnCallMonsterGenerator, Uid,
                (info) => { OnGeneratorCall((MonsterGenEventInfo)info); });
            _rtEventInfo = new MonsterGenRtEventInfo();
        }

        protected override void onEnterScene()
        {
            BattleEventManager.Instance.Register(_checker);
            if (Self.ModelController.Model.TryGetComponent<MonsterGeneratorModel>(out var comp))
            {
                comp.GetWayPoint(ref _wayPoints);
                _rect = comp.GetRect();
                Self.Pos = comp.transform.position;
                Self.Rot = comp.transform.rotation;
            }
        }

        public override void RecycleLogicObject()
        {
            APool<MonsterGeneratorLogic>.Pool.Recycle(this);
        }

        protected override void OnChildRecycle()
        {
            BattleEventManager.Instance.UnRegister(_checker);
        }

        /// <summary>
        ///     传入配置后开始创建怪物
        /// </summary>
        /// <param name="eventInfo"></param>
        private void OnGeneratorCall(MonsterGenEventInfo eventInfo)
        {
            switch (eventInfo.Behave)
            {
                case EMonsterGenBehave.Summon:
                    summonMonster(eventInfo.MonsterConfigId);
                    return;
                case EMonsterGenBehave.Pause:
                    pauseGenerator();
                    return;
                case EMonsterGenBehave.Resume:
                    resumeGenerator();
                    return;
                case EMonsterGenBehave.Clear:
                    clearGenMonster();
                    return;
            }
        }

        /// <summary>
        ///     召唤怪物启动
        /// </summary>
        /// <param name="configId"></param>
        private void summonMonster(int configId)
        {
            if (_isGeneratorActive) return;
            _beforeConfigId = configId;
            _duration = 0;
            _templateRow = ConfigManager.Table<MonsterGenerateTmpTable>().Get(configId);
            _createMonsterInterval = _templateRow.Interval;
            //数据准备
            _createMonsterQueue.Clear();
            foreach (var monsterInfo in _templateRow.MonsterInfos)
            {
                for (int i = 0; i < monsterInfo[1]; i++)
                {
                    var info = new GenActorInfo() { ConfigId = monsterInfo[0], ActorType = EActorType.Monster, };
                    _createMonsterQueue.Enqueue(info);

                    if (_templateRow.FactionId > 0)
                    {
                        BattleManager.CurBattle.RtInfo.AddFactionActorCount(_templateRow.FactionId);
                    }
                    else
                    {
                        BattleManager.CurBattle.RtInfo.AddFactionActorCount(info.ActorType,
                            info.ConfigId);
                    }
                }
            }

            _isGeneratorActive = _createMonsterQueue.Count > 0;

            _curMonsterUid.Clear();
        }

        private void pauseGenerator()
        {
            _isGeneratorActive = false;
        }

        private void resumeGenerator()
        {
            _isGeneratorActive = _templateRow != null && _createMonsterQueue.Count > 0;
        }

        private void clearGenMonster()
        {
            _isGeneratorActive = false;
            _templateRow = null;
            _createMonsterQueue.Clear();
            foreach (var uid in _curMonsterUid)
            {
                var actor = ActorManager.Instance.GetActor(uid);
                if (actor != null)
                {
                    actor.ExitSceneCallBack -= OnActorDead;
                    ActorManager.Instance.RemoveActor(uid);
                }

                --CurMonsterCount;
            }

            _curMonsterUid.Clear();
        }

        protected override void onTick(float dt)
        {
            if (!_isGeneratorActive) return;
            if (_templateRow == null) return;

            _duration += dt;

            if (_duration < _templateRow.DelayTime) return;
            _createMonsterInterval += dt;
            //场景怪物超过上限，暂时不刷
            if (CurMonsterCount > 60) return;
            GeneratingMonster();
        }

        private void GeneratingMonster()
        {
            if (_templateRow == null || _createMonsterInterval < _templateRow.Interval) return;

            for (int i = 0; i < _templateRow.MaxCreationOnce; i++)
            {
                if (!_createMonsterQueue.TryDequeue(out GenActorInfo info)) return;

                var uid = ActorManager.Instance.CreateActor(info.ActorType, info.ConfigId, null, onActorSetup);
                _curMonsterUid.Add(uid);
                ++CurMonsterCount;

                if (_createMonsterQueue.Count == 0)
                {
                    _templateRow = null;
                    _isGeneratorActive = false;
                    break;
                }
            }

            _createMonsterInterval = 0;
        }


        private void onActorSetup(Actor actor)
        {
            actor.ExitSceneCallBack += OnActorDead;

            if (_wayPoints.Count > 0)
            {
                actor.Variables.Set("WayPoints", _wayPoints);
            }

            /*if (actor.Logic.TryGetComponent<BeHurtComp>(out var hurtComp)) {
                hurtComp.OnHitKillActorCallBack += BattleManager.CurBattle.RtInfo.OnActorBeKilled;
            }*/

            foreach (var tag in _templateRow.ExTags)
            {
                actor.TagCollection.Add(tag);
            }

            if (actor.Logic.TryGetComponent(out BuffComp buffComp))
            {
                foreach (var buffInfo in _templateRow.ExBuffs)
                {
                    buffComp.AddBuff(Uid, buffInfo[0], buffInfo[1]);
                }
            }

            if (actor.Logic.TryGetComponent(out SkillComp skillComp))
            {
                foreach (var skillInfo in _templateRow.ExSkills)
                {
                    skillComp.LearnSkill(skillInfo[0], skillInfo[1]);
                }
            }

            if (_templateRow.FactionId > 0)
            {
                actor.SetAttr(EAttrType.AttrFaction, _templateRow.FactionId, false);
            }

            Vector3 randomOffset = new(
                Random.Range(-_rect.width, _rect.width),
                0,
                Random.Range(-_rect.height, _rect.height)
            );

            // 计算新的位置
            Vector3 position = Self.Pos + Self.Rot * randomOffset;

            actor.Pos = position;
            actor.Rot = Self.Rot;

            switch ((EGenMonsterActionType)_templateRow.MonsterBehave)
            {
                case EGenMonsterActionType.Stay:
                    actor.TargetPos = position;
                    break;
                case EGenMonsterActionType.AttackPlayer:
                    actor.BeforeTickCallBack += (monster, _) =>
                    {
                        var uid = BattleManager.CurBattle.TeamController.ControlUid;
                        if (ActorManager.Instance.TryGetActor(uid, out var leader))
                        {
                            monster.TargetPos = leader.Pos;
                        }
                    };
                    break;
            }
        }

        private void OnActorDead(Actor actor)
        {
            BattleManager.CurBattle.RtInfo.OnActorDead(actor);
            BattleManager.CurBattle.LootController.OnActorDead(actor);
            _curMonsterUid.Remove(actor.Uid);
            --CurMonsterCount;
            if (_curMonsterUid.Count == 0)
            {
                _rtEventInfo.ConfigId = _beforeConfigId;
                _rtEventInfo.MonsterGeneratorUid = Uid;
                _rtEventInfo.CurRoundCount = BattleManager.CurBattle.RtInfo.CurRoundCount;
                BattleEventManager.Instance.TriggerActorEvent(BattleConstValue.BattleRootControllerUid,
                    EBattleEventType.OnMonsterGeneratorAllDead, _rtEventInfo);
            }
        }
    }
}