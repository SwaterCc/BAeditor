#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools
{
    public interface IVarCollectionBind
    {
        public VarCollection Variables { get; }
    }

    public class VarCollection
    {
        private readonly Dictionary<string, object> _collection;
        private IVarCollectionBind _bind;

        public VarCollection(IVarCollectionBind bind, int capacity)
        {
            _bind = bind;
            _collection = new Dictionary<string, object>(capacity);
        }

        public object Get(string name)
        {
            return _collection.GetValueOrDefault(name);
        }

        public void Set(string key, in object variable)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("变量名为空 Set Failed");
                return;
            }

            if (variable != null)
            {
                _collection[key] = variable;
                return;
            }
            // Debug.LogWarning($"你在尝试存储一个null key {key}");
        }

        public void Delete(string name)
        {
            if (_collection.ContainsKey(name))
            {
                _collection.Remove(name);
            }
            else
            {
                //Debug.LogWarning($"not find remove Variable name : {name}");
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}