using System;
using System.Collections.Generic;
using Battle.Tools;

namespace Battle.GamePlay
{
    public class ActorManager : Singleton<ActorManager>
    {
        private List<Actor> _actors = new List<Actor>();
        private Dictionary<int, Actor> _uidActorDict = new();
        private Dictionary<EActorType, Dictionary<int, Actor>> _actorTypeDict = new();
        private List<Actor> _removeList = new List<Actor>();
        private List<Actor> _addCaches = new List<Actor>();

        private CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
        
        public void Init() { }

        public Actor CreateActor(EActorType actorType, int configId)
        {
            var actor = new Actor(_idGenerator.GenerateId(), actorType);
            ActorLogic logic = null;
            ActorShow show = null;
            switch (actorType)
            {
                case EActorType.Pawn:
                    break;
                case EActorType.Monster:
                    break;
                case EActorType.Building:
                    break;
                case EActorType.HitBox:
                    break;
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
                    _actors.Add(actor);
                    _uidActorDict.Add(actor.Uid, actor);
                    if (!_actorTypeDict.TryGetValue(actor.ActorType, out var dict))
                    {
                        dict = new Dictionary<int, Actor>();
                        _actorTypeDict.Add(actor.ActorType, dict);
                    }

                    dict.Add(actor.Uid, actor);
                }

                _addCaches.Clear();
            }

            foreach (var actor in _actors)
            {
                actor.RTState.Tick(dt);
            }

            if (_removeList.Count != 0)
            {
                foreach (var actor in _removeList)
                {
                    _actors.Remove(actor);
                }

                _removeList.Clear();
            }
        }

        public void Update(float dt)
        {
            foreach (var actor in _actors)
            {
                actor.RTState.Update(dt);
            }
        }

        public void AddActor(Actor actor)
        {
            _addCaches.Add(actor);
        }

        public void AddActor(EActorType type, int configId)
        {
            var actor = CreateActor(type, configId);
            _addCaches.Add(actor);
        }

        public Actor GetActor(int uid)
        {
            return _actors[uid];
        }

        public void RemoveActor(Actor actor) { }
    }
}