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

        public void Init()
        {
            _filter = new Filter(this);
        }

        public Actor CreateActor(int actorPrototypeId)
        {
            var actorData = ConfigManager.Table<ActorPrototypeTable>().Get(actorPrototypeId);
            var actor = new Actor(_idGenerator.GenerateId());
            
            ActorLogic logic = null;
            var logicData =  ConfigManager.Table<ActorLogicTable>().Get(actorData.LogicConfigId);
            switch ((EActorLogicType)logicData.LogicType)
            {
                case EActorLogicType.Pawn:
                    logic = new PawnLogic(actor, logicData);
                    break;
                case EActorLogicType.Monster:
                    logic = new MonsterLogic(actor, logicData);
                    break;
                case EActorLogicType.Building:
                    break;
                case EActorLogicType.HitBox:
                    logic = new HitBoxLogic(actor, logicData);
                    break;
            }

            ActorShow show = null;
            if (actorData.ShowConfigId > 0)
            {
                var showData =  ConfigManager.Instance.GetTable<ActorShowTable>().Get(actorData.ShowConfigId);
                switch ((EActorShowType)showData.ShowType)
                {
                    case EActorShowType.LogicTest:
                        show = new TestShow(actor, showData);
                        break;
                    case EActorShowType.Pawn:
                        show = new TestShow(actor, showData);
                        break;
                    case EActorShowType.Monster:
                        show = new TestShow(actor, showData);
                        break;
                    case EActorShowType.Building:
                        show = new TestShow(actor, showData);
                        break;
                }
            }
            
            actor.Setup(show, logic);
            return actor;
        }
        
        
        public void Tick(float dt)
        {
            if (_addCaches.Count != 0)
            {
                foreach (var actor in _addCaches)
                {
                    actor.Init();
                    _runningActorList.Add(actor);
                    _uidActorDict.Add(actor.Uid, actor);
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

        public void AddActor(int configId)
        {
            var actor = CreateActor(configId);
            AddActor(actor);
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