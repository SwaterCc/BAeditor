using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public class ObjectPoolManager: Singleton<ObjectPoolManager>,IBattleFrameworkTick
    {
        private readonly List<IAObjectPool> _aObjectPools = new(32);
        
        public void RegisterPool(IAObjectPool pool)
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