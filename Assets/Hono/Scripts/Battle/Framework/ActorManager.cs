using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

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

        private readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

        private bool _battleModelIsCreated;

        public static int NormalHitBoxConfigId = 1;
        
        public void Init()
        {
            _filter = new Filter(this);
        }

        public Actor GetBattleMode() {
	        if (_battleModelIsCreated) throw new Exception("重复创建BattleMode");
	        _battleModelIsCreated = true;
	        var actor = new Actor(_idGenerator.GenerateId(), EActorType.BattleMode);
	        actor.Setup(new BattleModeShow(actor),new BattleMode(actor));
	        return actor;
        }

        public Actor CreateActor(EActorType type, int configId = 0) {
	        var actor = getActor(type, configId);
	        actor.SetAttr(ELogicAttr.AttrSourceActorUid, actor.Uid,false);
	        actor.SetAttr(ELogicAttr.AttrTopSourceActorUid, actor.Uid,false);
	        return actor;
        }
        
        private Actor getActor(EActorType type, int configId = 0) {

	        int uid = _idGenerator.GenerateId();
	        var actor = new Actor(uid, type);
	        actor.SetAttr(ELogicAttr.AttrUid, uid, false);
	        actor.SetAttr(ELogicAttr.AttrConfigId, configId, false);
            switch (type)
            {
	            case EActorType.Pawn:
		            actor.Setup(new AlwaysShow(actor), new PawnLogic(actor));
		            break;
	            case EActorType.Monster:
		            actor.Setup(new AlwaysShow(actor), new MonsterLogic(actor));
		            break;
	            case EActorType.Building:
		            actor.Setup(new AlwaysShow(actor), new BuildingLogic(actor));
		            break;
	            case EActorType.Bullet:
		            actor.Setup(new BulletShow(actor), new BulletLogic(actor));
		            break;
	            case EActorType.HitBox:
		            actor.Setup(new AlwaysShow(actor), new HitBoxLogic(actor));
		            break;
            }
            
            return actor;
        }
        
        public Actor SummonActor(Actor summoner, EActorType type, int configId ,bool fromTopSummer) {
	        var summoned = getActor(type, configId);
	        summoned.SetAttr(ELogicAttr.AttrIsSummoned, 1, false);
            var sourceUid = fromTopSummer
                ? summoner.GetAttr<int>(ELogicAttr.AttrTopSourceActorUid)
                : summoner.GetAttr<int>(ELogicAttr.AttrSourceActorUid);
	        summoned.SetAttr(ELogicAttr.AttrSourceActorUid, sourceUid,false);
	        summoned.SetAttr(ELogicAttr.AttrTopSourceActorUid, summoner.GetAttr<int>(ELogicAttr.AttrTopSourceActorUid),false);
            summoned.SetAttr(ELogicAttr.AttrFaction, summoner.GetAttr<int>(ELogicAttr.AttrFaction), false);
            return summoned;
        }

        public Actor SummonActorByAbility(Ability summonerAbility,EActorType type, int configId, bool fromTopSummer)
        {
            var summoned = SummonActor(summonerAbility.Actor, type, configId, fromTopSummer);
	        summoned.SetAttr(ELogicAttr.AttrSourceAbilityConfigId, summonerAbility.ConfigId, false);
	        return summoned;
        }

        public void Tick(float dt)
        {
            if (_addCaches.Count != 0)
            {
                foreach (var actor in _addCaches)
                {
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
        
        public void AddActor(Actor actor)
        {
            if (actor == null) return;
            _addCaches.Add(actor);
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

        public void RemoveActor(Actor actor)
        {
            if (actor != null)
            {
                _removeList.Add(actor);
            }
        }

        public void RemoveActor(int actorUid)
        {
            if (_uidActorDict.TryGetValue(actorUid, out var actor))
            {
                _removeList.Add(actor);
            }
        }
    }
}