using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface IPoolObject
    {
        public void OnRecycle();
    }

    public interface IAObjectPool
    {
        public void Tick(float dt);
    }

    public class AObjectPool<T> : IAObjectPool  where T : class, IPoolObject, new()
    {
        /// <summary>
        /// 稳定堆
        /// </summary>
        private readonly Queue<T> _pool;

        /// <summary>
        /// 额外扩容堆
        /// </summary>
        private readonly Queue<T> _tempPool;

        /// <summary>
        /// 稳定堆容量
        /// </summary>
        private int _capacity;

        /// <summary>
        /// 临时池自动清理时间
        /// </summary>
        private float _autoClearTempPoolTime;

        /// <summary>
        /// 持续时间
        /// </summary>
        private float _duration;
        
        // 泛型类型的单例实例
        private static AObjectPool<T> _instance;
        
        // 获取单例实例的方法
        public static AObjectPool<T> Pool
        {
            get
            {
                if (_instance == null)
                {
                    if (_instance == null)
                    {
                        _instance = new AObjectPool<T>();
                    }
                }
                return _instance;
            }
        }
        
        private AObjectPool()
        {
            _pool = new Queue<T>(32);
            _tempPool = new Queue<T>(8);

            Debug.Log($"New Pool<{typeof(T)}> Create");
            ObjectPoolManager.Instance.RegisterPool(this);
        }

        public void SetCapacity(int capacity)
        {
            _capacity = capacity;
        }
        
        public void SetAutoClearTempPoolTime(float time)
        {
            _autoClearTempPoolTime = time;
        }
        
        public T Rent()
        {
            if (_tempPool.TryDequeue(out var obj))
            {
                return obj;
            }

            if (!_pool.TryDequeue(out obj))
            {
                obj = new T();
            }
            
            return obj;
        }


        public void Recycle(in T obj)
        {
            obj.OnRecycle();

            if (_pool.Count >= _capacity)
            {
                _tempPool.Enqueue(obj);
            }
            else
            {
                _pool.Enqueue(obj);
            }
        }

        public void Tick(float dt)
        {
            if (_tempPool.Count == 0) return;

            if (_duration > _autoClearTempPoolTime)
            {
                _tempPool.Clear();
            }

            _duration += dt;
        }
    }
}