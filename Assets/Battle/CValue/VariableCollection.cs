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
        private readonly Dictionary<string, IValueBox> _collection;
    
        private IVariableCollectionBind _bind;

        public VariableCollection(int capacity, IVariableCollectionBind bind = null) : this(capacity, capacity, bind)
        {
        }

        public VariableCollection(int variableSize, int funcSize, IVariableCollectionBind bind)
        {
            _collection = new Dictionary<string, IValueBox>(variableSize);
            _bind = bind;
        }

        public void SetBind(IVariableCollectionBind bind)
        {
            _bind ??= bind;
        }

        public ValueBox<T> GetVariable<T>(string name)
        {
            if (_collection.TryGetValue(name, out var variable))
            {
                return variable as ValueBox<T>;
            }

            return null;
        }
        
        public IValueBox GetVariable(string name)
        {
            return _collection.GetValueOrDefault(name);
        }


        public bool TryGetVariable<T>(string name, out ValueBox<T> value)
        {
            value = null;
            if (!_collection.TryGetValue(name, out var cValue)) return false;
            value = cValue as ValueBox<T>;
            return true;
        }

        public void Add(string key, IValueBox variable)
        {
            if (!_collection.TryAdd(key, variable))
            {
                //报个错
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