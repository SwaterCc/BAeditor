using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
	public interface IVarCollectionBind {
		public VarCollection Variables { get; }
	}
	
	public class VarCollection
    {
        private readonly Dictionary<string, AutoValue> _collection;
        private IVarCollectionBind _bind;
        
        public VarCollection(IVarCollectionBind bind, int capacity) {
	        _bind = bind;
            _collection = new Dictionary<string, AutoValue>(capacity);
        }
        
        public AutoValue Get(string name)
        {
            return _collection.GetValueOrDefault(name);
        }

        public void Set(string key, AutoValue variable) {
	        if (string.IsNullOrEmpty(key)) {
		        Debug.LogError("变量名为空 Set Failed");
		        return;
	        }

	        if (variable == null) {
		        Debug.LogWarning($"你在尝试存储一个null key {key}");
	        }
	        _collection[key] = variable;
        }

        public void Set(string key, object variable) {
	        if (string.IsNullOrEmpty(key)) {
		        Debug.LogError("变量名为空 Set Failed");
		        return;
	        }

	        if (variable == null) {
		        Debug.LogWarning($"你在尝试存储一个null key {key}");
	        }

	        var auto = new AutoValue();
	        auto.SetRef(variable);
	        _collection[key] = auto;
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