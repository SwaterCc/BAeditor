using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
	public interface IVarCollectionBind {
		public VarCollection Variables { get; }
	}
	
	public class VarCollection
    {
        private readonly Dictionary<string, object> _collection;
        private IVarCollectionBind _bind;
        
        public VarCollection(IVarCollectionBind bind, int capacity) {
	        _bind = bind;
            _collection = new Dictionary<string, object>(capacity);
        }
        
        public object Get(string name)
        {
            return _collection.GetValueOrDefault(name);
        }

        public void Set(string key,in object variable) {
	        if (string.IsNullOrEmpty(key)) {
		        Debug.LogError("变量名为空 Set Failed");
		        return;
	        }

	        if (variable == null) {
		        Debug.LogWarning($"你在尝试存储一个null key {key}");
	        }
	        _collection[key] = variable;
        }

        public void Delete(string name)
        {
            if (_collection.ContainsKey(name))
            {
                _collection.Remove(name);
            }
            else
            {
                Debug.LogWarning($"not find remove Variable name : {name}");
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}