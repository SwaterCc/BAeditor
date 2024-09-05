using System;
using System.Collections.Generic;
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

        private readonly List<Actor> _removeList = new();
        private readonly List<Actor> _addCaches = new();

        private readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

        public void Init()
        {
            _filter = new Filter(this);
        }

        public Actor CreateActor(int configId)
        {
            var actorData = AssetManager.Instance.GetData<ActorPrototypeData>(configId);
            var actor = new Actor(_idGenerator.GenerateId(), actorData);
            ActorLogic logic = null;
            var logicData = AssetManager.Instance.GetData<ActorLogicData>(actorData.LogicConfigId);
            switch (logicData.logicType)
            {
                case EActorLogicType.Pawn:
                    logic = new PawnLogic(actor.Uid, logicData);
                    break;
                case EActorLogicType.Monster:
                    break;
                case EActorLogicType.Building:
                    break;
                case EActorLogicType.HitBox:
                    logic = new HitBoxLogic(actor.Uid, logicData);
                    break;
            }

            ActorShow show = null;
            if (actorData.ShowConfigId > 0)
            {
                var showData = AssetManager.Instance.GetData<ActorShowData>(actorData.ShowConfigId);
                switch (showData.ShowType)
                {
                    case EActorShowType.LogicTest:
                        show = new TestShow(actor.Uid, showData);
                        break;
                    case EActorShowType.Pawn:
                        show = new TestShow(actor.Uid, showData);
                        break;
                    case EActorShowType.Monster:
                        break;
                    case EActorShowType.Building:
                        break;
                }
            }

            actor.Init(show, logic);
            return actor;
        }

        public void Tick(float dt)
        {
            if (_addCaches.Count != 0)
            {
                foreach (var actor in _addCaches)
                {
                    _runningActorList.Add(actor);
                    _uidActorDict.Add(actor.Uid, actor);
                }

                _addCaches.Clear();
            }

            foreach (var actor in _runningActorList)
            {
                actor.RTState.Tick(dt);
            }

            if (_removeList.Count != 0)
            {
                foreach (var actor in _removeList)
                {
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
                actor.RTState.Update(dt);
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

        public List<Actor> GetRunningActors()
        {
            return _runningActorList;
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