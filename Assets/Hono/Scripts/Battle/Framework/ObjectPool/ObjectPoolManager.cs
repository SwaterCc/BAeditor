using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public interface IAPool
    {
        public void Tick(float dt);
    }
    public class ObjectPoolManager : Singleton<ObjectPoolManager>, IBattleFrameworkTick
    {
        private readonly List<IAPool> _aObjectPools = new(32);

        public void RegisterPool(IAPool pool)
        {
            _aObjectPools.Add(pool);
        }

        public void Tick(float dt)
        {
            foreach (var objectPool in _aObjectPools)
            {
                objectPool.Tick(dt);
            }
        }
    }
}