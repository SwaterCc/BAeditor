#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Base
{
    /// <summary>
    /// 变量黑板，值类型单独存储，引用类型统一存储在object字典中,key值唯一
    /// </summary>
    public class VariableBoard : IAPoolRefCount
    {
        /// <summary>
        /// 引用类型容器
        /// </summary>
        private readonly Dictionary<string, object> _refCollection = new(10);
        /// <summary>
        /// key值速查类型，只要这里能查到，则说明变量存在
        /// </summary>
        private readonly Dictionary<string, Type> _keySearch = new();
        /// <summary>
        /// 值类型容器
        /// </summary>
        private readonly Dictionary<Type, IVarCollection> _collections = new()
        {
            { typeof(int), new VarCollection<int>() },
            { typeof(float), new VarCollection<float>() },
            { typeof(bool), new VarCollection<bool>() },
            { typeof(Vector3), new VarCollection<Vector3>() },
        };

        /// <summary>
        /// 引用计数器
        /// </summary>
        public APoolRefCount RefCount { get; set; }

        //预定义一些常用类型容器

        public T Get<T>(string key)
        {
            if (!_keySearch.TryGetValue(key, out var type))
                return default;

            if (type.IsClass)
            {
                return (T)_refCollection[key];
            }

            return ((VarCollection<T>)_collections[type]).GetValue(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (!_keySearch.ContainsKey(key))
            {
                return false;
            }

            if (typeof(T).IsClass)
            {
                var obj = _refCollection[key];
                if (obj is T tObj)
                {
                    value = tObj;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (!_collections.TryGetValue(typeof(T), out var collection))
                    return false;
                value = ((VarCollection<T>)collection).GetValue(key);
            }

            return true;
        }

        /// <summary>
        /// 获取引用对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="refValue"></param>
        /// <returns></returns>
        public bool TryGetRef(string key, out object refValue)
        {
            return _refCollection.TryGetValue(key, out refValue);
        }

        public void Set<T>(string key, T value)
        {
            var valueType = typeof(T);

            _keySearch[key] = valueType;

            if (valueType.IsClass)
            {
                _refCollection[key] = value;
            }
            else
            {
                if (!_collections.TryGetValue(valueType, out var iCollection))
                {
                    iCollection = new VarCollection<T>();
                    _collections.Add(valueType, iCollection);
                }

                ((VarCollection<T>)iCollection).SetValue(key, value);
            }
        }

        public void SetRef(string key, object value)
        {
            var valueType = value.GetType();

            if (valueType.IsClass)
            {
                _refCollection[key] = value;
                _keySearch[key] = valueType;
            }
            else
            {
                Debug.LogError($"尝试将值类型对象 key:{key}，装箱，操作不允许！");
            }
        }

        /// <summary>
        /// 传入EventInfo字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(EvtInfoField<T> field, T value)
        {
            Set(field.Name, value);
        }
        
        /// <summary>
        /// 获取EventInfo字段
        /// </summary>
        /// <param name="field"></param>
        /// <typeparam name="T"></typeparam>
        public T Get<T>(EvtInfoField<T> field)
        {
           return Get<T>(field.Name);
        }

        public void Delete(string key)
        {
            if (!_keySearch.ContainsKey(key))
            {
                return;
            }

            _keySearch.Remove(key, out var type);

            if (type.IsClass)
            {
                if (!_refCollection.Remove(key, out var obj)) return;
                if (obj is IAPoolObject poolObject)
                {
                    ObjectPoolManager.Instance.RecycleAObject(poolObject);
                }
            }
            else
            {
                _collections[type].Remove(key);
            }
        }

        public void Clear()
        {
            _keySearch.Clear();

            foreach (var obj in _refCollection.Values)
            {
                if (obj is IAPoolObject poolObject)
                {
                    ObjectPoolManager.Instance.RecycleAObject(poolObject);
                }
            }

            _refCollection.Clear();

            foreach (var varCollection in _collections.Values)
            {
                varCollection.Clear();
            }
        }

        public void OnRecycle()
        {
            Clear();
        }
    }
}