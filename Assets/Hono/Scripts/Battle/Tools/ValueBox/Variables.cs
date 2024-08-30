using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface IVariablesBind
    {
        public Variables GetVariables();
    }

    public class Variables
    {
        private readonly Dictionary<string, object> _collection;
    
        private IVariablesBind _bind;

        public Variables(int capacity, IVariablesBind bind = null)
        {
            _collection = new Dictionary<string, object>(capacity);
            _bind = bind;
        }
        
        public void SetBind(IVariablesBind bind)
        {
            _bind ??= bind;
        }
        
        public object GetVariable(string name)
        {
            return _collection.GetValueOrDefault(name);
        }

        public void Add(string key, object variable)
        {
            if (!_collection.TryAdd(key, variable))
            {
                //报个错
            }
        }

        public void ChangeValue(string key, object variable)
        {
            if (_collection.ContainsKey(key))
            {
                _collection[key] = variable;
            }
        }

        public void Remove(string name)
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