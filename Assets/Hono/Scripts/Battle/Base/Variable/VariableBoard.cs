#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Base
{
    public class VariableBoard
    {
        /// <summary>
        /// 容器
        /// </summary>
        private readonly Dictionary<Type, IVarCollection> _collections;
        /// <summary>
        /// key值速查类型
        /// </summary>
        private readonly Dictionary<string, Type> _keySearch;

        public VariableBoard(int capacity)
        {
            _collections = new Dictionary<Type, IVarCollection>(capacity);
            _keySearch = new Dictionary<string, Type>();
        }

        /// <summary>
        /// 获取引用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="refValue"></param>
        /// <returns></returns>
        public bool TryGetRef(string key, out object refValue)
        {
            refValue = null;
            if (!_keySearch.TryGetValue(key, out Type type))
            {
                return false;
            }
          
            refValue = _collections[type].GetRef(key);
            return true;
        }

        public T Get<T>(string key)
        {
            return !_keySearch.ContainsKey(key) ? default : ((VarCollection<T>)_collections[typeof(T)]).GetValue(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (!_keySearch.ContainsKey(key))
            {
                return false;
            }

            if (!_collections.TryGetValue(typeof(T), out var collection))
                return false;

            value = ((VarCollection<T>)collection).GetValue(key);
            return true;
        }

        public void Set<T>(string key, T value)
        {
            if (!_keySearch.TryGetValue(key, out Type type))
            {
                type = typeof(T);
                _keySearch.Add(key, type);
                _collections.TryAdd(type, new VarCollection<T>());
            }

            ((VarCollection<T>)_collections[type]).SetValue(key, value);
        }
        
        public void Set(string key, object value)
        {
            if (!_keySearch.TryGetValue(key, out Type type))
            {
                type = value.GetType();
                _keySearch.Add(key, type);
              //  _collections.TryAdd(type, new VarCollection<T>());
            }

            //((VarCollection<T>)_collections[type]).SetValue(key, value);
        }

        public void Delete(string key)
        {
            if (!_keySearch.ContainsKey(key))
            {
                return;
            }

            _keySearch.Remove(key, out var type);
            _collections[type].Remove(key);
        }

        public void Clear()
        {
            _keySearch.Clear();
            foreach (var varCollection in _collections.Values)
            {
                varCollection.Clear();
            }
        }
    }
}