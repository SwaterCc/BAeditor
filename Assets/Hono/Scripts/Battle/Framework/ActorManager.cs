using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Scene;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle
{
    public partial class ActorManager : Singleton<ActorManager>
    {
        /// <summary>
        /// 正在运行的actor列表
        /// </summary>
        private readonly List<Actor> _runningActorList = new(512);

        /// <summary>
        /// actor字典
        /// </summary>
        private readonly Dictionary<int, Actor> _uidActorDict = new(512);

        private readonly Dictionary<int, Actor> _loadingCaches = new(128);
        private readonly List<Actor> _removeList = new(16);
        private readonly List<Actor> _addCaches = new(16);


        public ActorManager()
        {
            _filter = new Filter(this);
        }
		public void ForeachChracters(Action<Actor> action) {
			foreach (var item in _runningActorList) {
				switch (item.ActorType) {
					case EActorType.Pawn:
					case EActorType.Monster:
						action(item);
						break;
					default:
						break;
				}
			}
		}

		public BattleController GetBattleControl()
        {
            var actor = new Actor(BattleConstValue.BattleRootControllerUid, EActorType.BattleLevelController);
            var battleLevelControl = new BattleController(actor);
            actor.Setup(new BattleControllerModel(actor), battleLevelControl);
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
        private Actor getActor(int uid, EActorType type, int configId, Action<Actor> afterSetupCall,
            ActorModel actorModel)
        {
            var actor = new Actor(uid, type);
            actor.SetAttr(ELogicAttr.AttrUid, uid, false);
            actor.SetAttr(ELogicAttr.AttrConfigId, configId, false);
            switch (type)
            {
                case EActorType.Pawn:
                    actor.Setup(new NormalModelController(actor, (SceneActorModel)actorModel), new PawnLogic(actor));
                    break;
                case EActorType.Monster:
                    actor.Setup(new NormalModelController(actor, (SceneActorModel)actorModel), new MonsterLogic(actor));
                    break;
                case EActorType.Building:
	                var buildingActorModel = actorModel == null ? null : (BuildingActorModel)actorModel;
                    actor.Setup(new BuildingModelController(actor, buildingActorModel), new BuildingLogic(actor));
                    break;
                case EActorType.Bullet:
                    actor.Setup(new SimplePreLoadModelController(actor, EPreLoadGameObjectType.BulletModel),
                        new BulletLogic(actor));
                    break;
                case EActorType.HitBox:
                    actor.Setup(new SimplePreLoadModelController(actor, EPreLoadGameObjectType.HitBoxModel),
                        new HitBoxLogic(actor));
                    break;
                case EActorType.MonsterGenerator:
                    actor.Setup(new SimpleSceneModelController(actor, (SceneActorModel)actorModel), new MonsterGeneratorLogic(actor));
                    break;
                case EActorType.TriggerBox:
                    actor.Setup(new TriggerBoxModelController(actor, (SceneActorModel)actorModel), new TriggerBoxLogic(actor));
                    break;
                case EActorType.TeamDefaultBirthPoint:
                    actor.Setup(new SimpleSceneModelController(actor, (SceneActorModel)actorModel),
                        new TeamDefaultBirthPointLogic(actor));
                    break;
                case EActorType.TeamRefreshPoint:
                    actor.Setup(new SimplePreLoadModelController(actor, EPreLoadGameObjectType.TeamRefreshPoint),
                        new TeamRefreshPointLogic(actor));
                    break;
            }

            afterSetupCall?.Invoke(actor);

            _loadingCaches.Add(actor.Uid, actor);

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
        
        public int CreateBuilding(BuildingActorModel buildingModel,int configId = 0, Action<Actor> afterSetupCallFunc = null)
        {
	        int uid = buildingModel.ActorUid;
	        var actor = getActor(uid, EActorType.Building, configId, afterSetupCallFunc, buildingModel);
	        actor.SetAttr(ELogicAttr.AttrSourceActorUid, actor.Uid, false);
	        actor.SetAttr(ELogicAttr.AttrTopSourceActorUid, actor.Uid, false);
	        return actor.Uid;
        }

        #endregion


        #region 召唤流程

        public int SummonActor(Actor summoner, EActorType type, int configId, bool fromTopSummer,
            Action<Actor> afterSetupCallFunc = null)
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
	        Profiler.BeginSample("AllActorTick");
	        
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
	        
            if (_loadingCaches.Count > 0)
            {
                foreach (var actor in _loadingCaches)
                {
                    if (actor.Value.ActorSetupFinish)
                    {
                        _addCaches.Add(actor.Value);
                    }
                }
            }

            if (_addCaches.Count > 0)
            {
                foreach (var actor in _addCaches)
                {
                    //从加载列表里删除
                    _loadingCaches.Remove(actor.Uid);
					if (!_uidActorDict.ContainsKey(actor.Uid)) {
						_runningActorList.Add(actor);
						_uidActorDict.Add(actor.Uid, actor);
						actor.Init();
					}
					else {
						Debug.LogError($"出现key值重复{actor.Uid} {actor.ActorType}");
					}
                 
                }

                _addCaches.Clear();
            }

            foreach (var actor in _runningActorList)
            {
                actor.Tick(dt);
            }
            Profiler.EndSample();
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
            return _uidActorDict.TryGetValue(uid, out var actor) ? actor : _loadingCaches.GetValueOrDefault(uid);
        }

        public bool TryGetActor(int uid, out Actor actor)
        {
            return _loadingCaches.TryGetValue(uid, out actor) || _uidActorDict.TryGetValue(uid, out actor);
        }

        public EActorRunningState GetActorRtState(int uid)
        {
            if (_loadingCaches.ContainsKey(uid))
            {
                return EActorRunningState.Loading;
            }

            if (_uidActorDict.ContainsKey(uid))
            {
                return EActorRunningState.Active;
            }

            return EActorRunningState.NotExist;
        }
        
        public void RemoveActor(int actorUid)
        {
            if (_uidActorDict.TryGetValue(actorUid, out var actor)) {
	            actor.IsExpired = true;
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