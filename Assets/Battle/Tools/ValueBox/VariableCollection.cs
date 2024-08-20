using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public interface IVariableCollectionBind
    {
        public VariableCollection GetVariableCollection();
    }

    public class VariableCollection
    {
        private readonly Dictionary<string, object> _collection;
    
        private IVariableCollectionBind _bind;

        public VariableCollection(int capacity, IVariableCollectionBind bind = null) : this(capacity, capacity, bind)
        {
        }

        public VariableCollection(int variableSize, int funcSize, IVariableCollectionBind bind)
        {
            _collection = new Dictionary<string, object>(variableSize);
            _bind = bind;
        }

        public void SetBind(IVariableCollectionBind bind)
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