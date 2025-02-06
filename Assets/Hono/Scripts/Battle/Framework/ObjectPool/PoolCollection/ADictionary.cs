using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.ObjectPool
{
    /// <summary>
    /// 池化字典
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ADictionary<TKey, TValue> : ICPoolObject, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();

        public int Count => _dictionary.Count;

        public void Add(TKey key, TValue value)
        {
            if (_dictionary.TryAdd(key, value))
            {
                if (key is IAPoolRefCount keyRefCount)
                {
                    keyRefCount.RefCount.AddReference();
                }

                if (value is IAPoolRefCount valueRefCount)
                {
                    valueRefCount.RefCount.AddReference();
                }
            }
            else
            {
                Debug.LogError($"添加元素 key {key} value {value} 失败！");
            }
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (_dictionary.TryAdd(key, value))
            {
                if (key is IAPoolRefCount keyRefCount)
                {
                    keyRefCount.RefCount.AddReference();
                }

                if (value is IAPoolRefCount valueRefCount)
                {
                    valueRefCount.RefCount.AddReference();
                }

                return true;
            }

            Debug.LogError($"添加元素 key {key} value {value} 失败！");
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool Remove(TKey key)
        {
            if (_dictionary.Remove(key, out var value))
            {
                if (key is IAPoolRefCount keyRefCount)
                {
                    keyRefCount.RefCount.RemoveReference();
                }

                if (value is IAPoolRefCount valueRefCount)
                {
                    valueRefCount.RefCount.RemoveReference();
                }

                return true;
            }

            return false;
        }

        public bool Remove(TKey key, out TValue value)
        {
            if (_dictionary.Remove(key, out value))
            {
                if (key is IAPoolRefCount keyRefCount)
                {
                    keyRefCount.RefCount.RemoveReference();
                }

                if (value is IAPoolRefCount valueRefCount)
                {
                    valueRefCount.RefCount.RemoveReference();
                }

                return true;
            }

            return false;
        }

        public TValue this[TKey key] => _dictionary[key];

        public static implicit operator Dictionary<TKey, TValue>(ADictionary<TKey, TValue> dictionary)
        {
            return dictionary._dictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var pair in _dictionary)
            {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            foreach (var item in _dictionary.Values)
            {
                if (item is IAPoolRefCount refCount)
                {
                    refCount.RefCount.RemoveReference();
                }
                else if (item is ICPoolObject poolObject)
                {
                    GPoolManager.Instance.RecycleAObject(poolObject);
                }
            }

            foreach (var item in _dictionary.Values)
            {
                if (item is IAPoolRefCount refCount)
                {
                    refCount.RefCount.RemoveReference();
                }
                else if (item is ICPoolObject poolObject)
                {
                    GPoolManager.Instance.RecycleAObject(poolObject);
                }
            }

            _dictionary.Clear();
        }

        public void OnRecycle()
        {
            Clear();
        }
    }
}