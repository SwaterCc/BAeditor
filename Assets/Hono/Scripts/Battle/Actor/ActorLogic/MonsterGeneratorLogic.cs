using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class MonsterGeneratorLogic : ActorLogic
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
        public MonsterGeneratorLogic(Actor actor) : base(actor)
        {
            _checker = new MonsterGenEventChecker(EBattleEventType.OnCallMonsterGen, Uid, (info) =>
            {
                int configId = ((MonsterGenEventInfo)info).MonsterConfigId;
                StartGenerator(configId);
            });
        }

        protected override void onInit()
        {
            BattleEventManager.Instance.Register(_checker);
            if (Actor.ModelController.Model.TryGetComponent<MonsterGeneratorModel>(out var comp)) {
	            comp.GetWayPoint(ref _wayPoints);
            }
        }

        protected override void onDestroy()
        {
            BattleEventManager.Instance.UnRegister(_checker);
        }

        /// <summary>
        /// 传入配置后开始创建怪物
        /// </summary>
        /// <param name="configId"></param>
        private void StartGenerator(int configId)
        {
            if (_isGeneratorActive) return;

            _duration = 0;
            _templateRow = ConfigManager.Table<MonsterGenerateTmpTable>().Get(configId);

            //数据准备
            _createMonsterQueue.Clear();
            foreach (var monsterInfo in _templateRow.MonsterInfos)
            {
                for (int i = 0; i < monsterInfo[1]; i++)
                {
                    var info = new GenActorInfo()
                    {
                        ConfigId = monsterInfo[0],
                        ActorType = EActorType.Monster,
                    };
                    _createMonsterQueue.Enqueue(info);

                    if (_templateRow.FactionId > 0)
                    {
                        BattleManager.CurrentBattleGround.RuntimeInfo.AddFactionActorCount(_templateRow.FactionId);
                    }
                    else
                    {
                        BattleManager.CurrentBattleGround.RuntimeInfo.AddFactionActorCount(info.ActorType,
                            info.ConfigId);
                    }
                }
            }

            _isGeneratorActive = _createMonsterQueue.Count > 0;

            //数据收集一下
        }

        private void GeneratingMonster()
        {
            if (_createMonsterInterval < _templateRow.Interval) return;

            var info = _createMonsterQueue.Dequeue();

            ActorManager.Instance.CreateActor(info.ActorType, info.ConfigId, onActorSetup);

            if (_createMonsterQueue.Count == 0)
            {
                _templateRow = null;
                _isGeneratorActive = false;
            }

            _createMonsterInterval = 0;
        }

        private void onActorSetup(Actor actor)
        {
            actor.OnDestroyCallBack += BattleManager.CurrentBattleGround.RuntimeInfo.OnActorDead;

            if (_wayPoints.Count > 0) {
	            actor.Variables.Set("WayPoints",_wayPoints);
            }
            
            if (actor.Logic.TryGetComponent<BeHurtComp>(out var hurtComp))
            {
                hurtComp.OnHitKillActorCallBack += BattleManager.CurrentBattleGround.RuntimeInfo.OnActorBeKilled;
            }

            foreach (var tag in _templateRow.ExTags)
            {
                actor.AddTag(tag);
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
                foreach (var skillInfo in _templateRow.ExBuffs)
                {
                    skillComp.LearnSkill(skillInfo[0], skillInfo[1]);
                }
            }

            if (_templateRow.FactionId > 0)
            {
                actor.SetAttr(ELogicAttr.AttrFaction, _templateRow.FactionId, false);
            }

            //actor.OnTickCallBack += self => self.SetAttr(ELogicAttr.AttrOriginPos, Actor.Pos, false); 
            actor.SetAttr(ELogicAttr.AttrOriginPos, Actor.Pos, false); 
            actor.SetAttr(ELogicAttr.AttrPosition, Actor.Pos, false);
            actor.SetAttr(ELogicAttr.AttrRot, Actor.Rot, false);
        }

        protected override void onTick(float dt)
        {
            if (!_isGeneratorActive) return;
            if (_templateRow == null) return;

            _duration += dt;

            if (_duration < _templateRow.DelayTime) return;

            _createMonsterInterval += dt;
            GeneratingMonster();
        }
    }
}