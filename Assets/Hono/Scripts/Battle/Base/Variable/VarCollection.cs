using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hono.Scripts.Battle.Base
{
    public interface IVarCollection
    {
        public Type GetVariableType();
        public object GetRef(string key);
        public void Remove(string key);
        public void Clear();
    }

    public class VarCollection<T> : IVarCollection
    {
        private readonly Dictionary<string, T> _collection = new(5);
        private Type _variableType = typeof(T);

        public bool TryGetValue(string key,out T value)
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
     
        public Type GetVariableType()
        {
            return _variableType;
        }

        public object GetRef(string key)
        {
            if (_variableType.IsValueType)
            {
                Debug.LogWarning($"Variable Get 尝试将一个值类型装箱 {key}");
            }

            return GetValue(key);
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