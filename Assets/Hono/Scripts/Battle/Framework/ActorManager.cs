using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Scene;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public partial class ActorManager : Singleton<ActorManager>
    {
        /// <summary>
        /// 正在运行的actor列表
        /// </summary>
        private readonly List<Actor> _runningActorList = new();

        /// <summary>
        /// actor字典
        /// </summary>
        private readonly Dictionary<int, Actor> _uidActorDict = new();

        private readonly List<Actor> _removeList = new(16);
        private readonly List<Actor> _addCaches = new(16);
        private readonly List<Actor> _loadingCaches = new(16);

        public ActorManager()
        {
            _filter = new Filter(this);
        }

        public BattleController GetBattleControl()
        {
            var actor = new Actor(BattleConstValue.BattleRootControllerUid, EActorType.BattleLevelController);
            var battleLevelControl = new BattleController(actor);
            actor.Setup(new ActorModelController.PreLoadModelSetup(EPreLoadGameObjectType.BattleRootModel),
                battleLevelControl);
            return battleLevelControl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <param name="configId"></param>
        /// <param name="afterSetupCall"></param>
        /// <param name="actorModel"></param>
        /// <returns></returns>
        private Actor getActor(int uid, EActorType type, int configId, Action<Actor> afterSetupCall, SceneActorModel actorModel)
        {
            var actor = new Actor(uid, type);
            ActorModelController.ModelSetup modelSetup = null;
            actor.SetAttr(ELogicAttr.AttrUid, uid, false);
            actor.SetAttr(ELogicAttr.AttrConfigId, configId, false);
            switch (type)
            {
                case EActorType.Pawn:
                    modelSetup = new ActorModelController.AsyncLoadModelSetup();
                    actor.Setup(modelSetup, new PawnLogic(actor));
                    break;
                case EActorType.Monster:
                    modelSetup = new ActorModelController.AsyncLoadModelSetup();
                    actor.Setup(modelSetup, new MonsterLogic(actor));
                    break;
                case EActorType.Building:
                    modelSetup = new ActorModelController.AsyncLoadModelSetup();
                    actor.Setup(modelSetup, new BuildingLogic(actor));
                    break;
                case EActorType.Bullet:
                    modelSetup = new ActorModelController.PreLoadModelSetup(EPreLoadGameObjectType.BulletModel);
                    actor.Setup(modelSetup, new BulletLogic(actor));
                    break;
                case EActorType.HitBox:
                    modelSetup = new ActorModelController.PreLoadModelSetup(EPreLoadGameObjectType.HitBoxModel);
                    actor.Setup(modelSetup, new HitBoxLogic(actor));
                    break;
                case EActorType.MonsterBuilder:
                    modelSetup = new ActorModelController.SceneModelSetup(actorModel);
                    actor.Setup(modelSetup, new HitBoxLogic(actor));
                    break;
                case EActorType.TriggerBox:
                    modelSetup = new ActorModelController.SceneModelSetup(actorModel);
                    actor.Setup(modelSetup, new HitBoxLogic(actor));
                    break;
            }

            afterSetupCall?.Invoke(actor);
            _loadingCaches.Add(actor);
            
            return actor;
        }


        #region 创建流程

        public int CreateSceneActor(SceneActorModel model, int configId = 0,
            Action<Actor> afterSetupCallFunc = null)
        {
            var actor = getActor(model.ActorUid, model.ActorType, configId, afterSetupCallFunc, model);
            actor.SetAttr(ELogicAttr.AttrSourceActorUid, actor.Uid, false);
            actor.SetAttr(ELogicAttr.AttrTopSourceActorUid, actor.Uid, false);
            return model.ActorUid;
        }

        public int CreateUniqueActor(int uniqueUid, EActorType type, int configId = 0,
            Action<Actor> afterSetupCallFunc = null)
        {
            var actor = getActor(uniqueUid, type, configId, afterSetupCallFunc, null);
            actor.SetAttr(ELogicAttr.AttrSourceActorUid, actor.Uid, false);
            actor.SetAttr(ELogicAttr.AttrTopSourceActorUid, actor.Uid, false);
            return uniqueUid;
        }

        public int CreateActor(EActorType type, int configId = 0, Action<Actor> afterSetupCallFunc = null)
        {
            int uid = ActorUidGenerator.GenerateUid(EActorUidRangeType.NormalActor);
            var actor = getActor(uid, type, configId, afterSetupCallFunc, null);
            actor.SetAttr(ELogicAttr.AttrSourceActorUid, actor.Uid, false);
            actor.SetAttr(ELogicAttr.AttrTopSourceActorUid, actor.Uid, false);
            return actor.Uid;
        }

        #endregion


        #region 召唤流程
        public int SummonActor(Actor summoner, EActorType type, int configId, bool fromTopSummer, Action<Actor> afterSetupCallFunc = null)
        {
            int uid = ActorUidGenerator.GenerateUid(EActorUidRangeType.DynamicActor);
            var summoned = getActor(uid, type, configId, afterSetupCallFunc, null);
            summoned.SetAttr(ELogicAttr.AttrIsSummoned, 1, false);
            var sourceUid = fromTopSummer
                ? summoner.GetAttr<int>(ELogicAttr.AttrTopSourceActorUid)
                : summoner.GetAttr<int>(ELogicAttr.AttrSourceActorUid);
            summoned.SetAttr(ELogicAttr.AttrSourceActorUid, sourceUid, false);
            summoned.SetAttr(ELogicAttr.AttrTopSourceActorUid, summoner.GetAttr<int>(ELogicAttr.AttrTopSourceActorUid),
                false);
            summoned.SetAttr(ELogicAttr.AttrFaction, summoner.GetAttr<int>(ELogicAttr.AttrFaction), false);
            return summoned.Uid;
        }
        #endregion


        public void Tick(float dt)
        {
            if (_loadingCaches.Count > 0)
            {
                foreach (var actor in _loadingCaches)
                {
                    if (actor.ActorSetupFinish)
                    {
                        _addCaches.Add(actor);
                    }
                }
            }
            
            if (_addCaches.Count > 0)
            {
                foreach (var actor in _addCaches)
                {
                    //从加载列表里删除
                    _loadingCaches.Remove(actor);
                    _runningActorList.Add(actor);
                    _uidActorDict.Add(actor.Uid, actor);
                    actor.Init();
                }

                _addCaches.Clear();
            }

            foreach (var actor in _runningActorList)
            {
                actor.Tick(dt);
            }

            if (_removeList.Count != 0)
            {
                foreach (var actor in _removeList)
                {
                    actor.Destroy();
                    _runningActorList.Remove(actor);
                    _uidActorDict.Remove(actor.Uid);
                }

                _removeList.Clear();
            }
        }

        public void Update(float dt)
        {
            foreach (var actor in _runningActorList)
            {
                actor.Update(dt);
            }
        }

        public Actor GetActor(int uid)
        {
            if (_uidActorDict.TryGetValue(uid, out var actor))
            {
                return actor;
            }

            return null;
        }

        public bool TryGetActor(int uid, out Actor actor)
        {
            return _uidActorDict.TryGetValue(uid, out actor);
        }

        public void RemoveActor(int actorUid)
        {
            if (_uidActorDict.TryGetValue(actorUid, out var actor))
            {
                _removeList.Add(actor);
            }
        }

        public void ClearAllActor()
        {
            foreach (var actor in _runningActorList)
            {
                actor.Destroy();
            }

            _runningActorList.Clear();
            _uidActorDict.Clear();
            _addCaches.Clear();
            _removeList.Clear();
        }
    }
}