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


        public void Init()
        {
            
        }
        
        private Actor createActor(EActorType actorType, int configId)
        {
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

            Actor actor = new Actor(show, logic);
            actor.Init();
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
                actor.Tick(dt);
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

        public void AddActor(EActorType type, int configId)
        {
            var actor = createActor(type, configId);
            _addCaches.Add(actor);
        }

        public void RemoveActor(Actor actor) { }
    }
}