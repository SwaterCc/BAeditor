using System;
using System.Collections.Generic;

namespace Battle.GamePlay
{
    public class ActorManager
    {
        private Dictionary<EActorType, List<NewActor>> _actors = new();


        public NewActor CreateActor(EActorType actorType, int configId)
        {
            NewActor actor = null;
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
            actor.Initialize();
        }
        
        public void AddActor(int configId)
        {
            var actor = new NewActor();
        }
    }
}