using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle.Core
{
    public class ActorPool : Singleton<ActorPool>
    {
        /// <summary>
        /// jsonKey -> actorPool
        /// </summary>
        private readonly Dictionary<string, Queue<Actor>> _pools = new(30);

        public Actor Get(string jsonKey)
        {
            if (!_pools.TryGetValue(jsonKey, out var pool) || pool.Count == 0)
            {
                return new Actor(jsonKey);
            }

            return pool.Dequeue();
        }

        public void Recycle(Actor actor)
        {
            actor.OnRecycle();
            
            if (!_pools.TryGetValue(actor.JsonKey, out var pool))
            {
                pool = new Queue<Actor>(100);
                _pools.Add(actor.JsonKey, pool);
            }
            pool.Enqueue(actor);
        }
    }
}