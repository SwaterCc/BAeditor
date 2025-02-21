using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public interface IGPool
    {
        public Type GetPoolType();
        public void Tick(float dt);
        public void Recycle(IGPoolObject poolObject);
    }

    public class GPoolManager : Singleton<GPoolManager>, IWorldSystemWhenTickCalled
    {
        private readonly List<IGPool> _aObjectPools = new(32);
        private readonly Dictionary<Type, IGPool> _typePoolSearch = new(32);

        public void RegisterPool(IGPool pool)
        {
            _aObjectPools.Add(pool);
            _typePoolSearch.Add(pool.GetPoolType(), pool);
        }

        public void RecycleAObject(IGPoolObject poolObject)
        {
            var poolType = poolObject.GetType();
            if (_typePoolSearch.TryGetValue(poolType, out var pool))
            {
                pool.Recycle(poolObject);
            }
        }

        public void OnWorldTick(float dt)
        {
            foreach (var objectPool in _aObjectPools)
            {
                objectPool.Tick(dt);
            }
        }
    }
}