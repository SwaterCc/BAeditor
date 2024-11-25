using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public interface IAPool
    {
        public Type GetPoolType();
        public void Tick(float dt);
        public void Recycle(IAPoolObject poolObject);
    }
    public class ObjectPoolManager : Singleton<ObjectPoolManager>, IBattleFrameworkTick
    {
        private readonly List<IAPool> _aObjectPools = new(32);
        private readonly Dictionary<Type, IAPool> _typePoolSearch = new(32);

        public void RegisterPool(IAPool pool)
        {
            _aObjectPools.Add(pool);
            _typePoolSearch.Add(pool.GetPoolType(), pool);
        }

        public void RecycleAObject(IAPoolObject poolObject)
        {
           var poolType = poolObject.GetType();
           if (_typePoolSearch.TryGetValue(poolType, out var pool))
           {
               pool.Recycle(poolObject);
           }
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