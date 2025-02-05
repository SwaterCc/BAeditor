using System.Collections;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.ObjectPool
{
    /// <summary>
    /// 池化list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AList<T> : IAPoolObject, IEnumerable
    {
        private readonly List<T> _list = new(8);

        public int Count => _list.Count;
        
        public T this[int idx] => _list[idx];
        
        public static implicit operator List<T>(AList<T> list)
        {
            return list._list;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public void Add(T item)
        {
            _list.Add(item);
            if (item is IAPoolRefCount refCount)
            {
                refCount.RefCount.AddReference();
            }
        }

        public void Remove(T item)
        {
            _list.Remove(item);
            
            if (item is IAPoolRefCount refCount)
            {
                refCount.RefCount.RemoveReference();
            }
            else if (item is IAPoolObject poolObject)
            {
                GPoolManager.Instance.RecycleAObject(poolObject);
            }
        }
        
        public void Clear()
        {
            foreach (var item in _list)
            {
                if (item is IAPoolRefCount refCount)
                {
                    refCount.RefCount.RemoveReference();
                }
                else if(item is IAPoolObject poolObject)
                {
                    GPoolManager.Instance.RecycleAObject(poolObject);
                }
            }
            _list.Clear();
        }
        
        public void OnRecycle()
        {
            Clear();
        }
    }
}