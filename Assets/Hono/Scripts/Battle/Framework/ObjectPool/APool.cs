using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface IAPoolObject
    {
        public void OnRecycle();
    }

    public struct APoolRefCount
    {
        private int _refCount;
        private IAPoolObject _poolObject;

        public APoolRefCount(IAPoolObject poolObject, int refCount = 0)
        {
            _poolObject = poolObject;
            _refCount = refCount;
        }

        public void AddReference()
        {
            ++_refCount;
        }

        public void RemoveReference()
        {
            --_refCount;
            if (_refCount <= 0)
            {
                APoolManager.Instance.RecycleAObject(_poolObject);
                _poolObject = null;
            }
        }

        public int GetReferenceCount()
        {
            return _refCount;
        }
    }

    public interface IAPoolRefCount : IAPoolObject
    {
        public APoolRefCount RefCount { get; set; }
    }

    
    /// <summary>
    /// A池，逻辑对象池，用于管理代码运行中的逻辑对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APool<T> : IAPool where T : class, IAPoolObject, new()
    {
        /// <summary>
        /// 稳定池
        /// </summary>
        private readonly Queue<T> _pool;

        /// <summary>
        /// 动态池
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
        private static APool<T> _instance;

        // 获取单例实例的方法
        public static APool<T> Pool
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new APool<T>();
                }

                return _instance;
            }
        }

        private APool()
        {
            _pool = new Queue<T>(100);
            _tempPool = new Queue<T>(15);

            Debug.Log($"New Pool<{typeof(T)}> Create");
            APoolManager.Instance.RegisterPool(this);
        }

        public T Rent()
        {
            if (_tempPool.TryDequeue(out var obj) || _pool.TryDequeue(out obj))
            {
                if (obj is IAPoolRefCount refCountObj)
                {
                    refCountObj.RefCount = new APoolRefCount(obj,1);
                }

                return obj;
            }

            obj = new T();
            if (obj is IAPoolRefCount newRefCountObj)
            {
                newRefCountObj.RefCount = new APoolRefCount(obj,1);
            }

            return obj;
        }

        public void Recycle(IAPoolObject obj)
        {
            Recycle((T)obj);
        }

        public void Recycle(in T obj)
        {
            if (obj is IAPoolRefCount refCountObj)
            {
                refCountObj.RefCount.RemoveReference();
                if (refCountObj.RefCount.GetReferenceCount() > 0)
                {
                    return;
                }
            }

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

        public Type GetPoolType()
        {
            return typeof(T);
        }

        public void Tick(float dt)
        {
            if (_tempPool.Count == 0) return;

            if (_duration > _autoClearTempPoolTime)
            {
                _tempPool.Clear();
                _duration = 0; // 重置持续时间
            }

            _duration += dt;
        }

        /// <summary>
        /// 设置池容量
        /// </summary>
        /// <param name="capacity"></param>
        public void SetCapacity(int capacity)
        {
            _capacity = capacity;
        }

        /// <summary>
        /// 设置临时池清理时间
        /// </summary>
        /// <param name="time"></param>
        public void SetAutoClearTempPoolTime(float time)
        {
            _autoClearTempPoolTime = time;
        }
    }
}