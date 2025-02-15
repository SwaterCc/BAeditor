using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface IVarCollection
    {
        public void Remove(string key);
        public void Clear();
    }

    /// <summary>
    /// 这个容器中理论上只存在值类型，不存在引用类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VarCollection<T> : IVarCollection
    {
        private readonly Dictionary<string, T> _collection = new(8);
        private readonly Type _variableType = typeof(T);

        public bool TryGetValue(string key, out T value)
        {
            return _collection.TryGetValue(key, out value);
        }

        public T GetValue(string key)
        {
            return _collection.GetValueOrDefault(key, default);
        }

        public void SetValue(string key, T value)
        {
            if (!_collection.TryAdd(key, value))
            {
                _collection[key] = value;
            }
        }
        
        public object GetRef(string key)
        {
            if (_variableType.IsValueType)
            {
                Debug.LogWarning($"Variable GetRef 尝试将一个值类型装箱 {key}");
            }

            return GetValue(key);
        }

        public void AddRef(string key, object value)
        {
            if (value is not T tValue)
            {
                Debug.LogError($"Variable AddRef 存入不符合类型的值 {key}");
                return;
            }

            if (!_variableType.IsClass)
            {
                Debug.LogWarning($"Variable AddRef 尝试将一个值类型装箱 {key}");
            }

            if (!_collection.TryAdd(key, tValue))
            {
                _collection[key] = tValue;
            }
        }

        public void Remove(string key)
        {
            _collection.Remove(key);
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}