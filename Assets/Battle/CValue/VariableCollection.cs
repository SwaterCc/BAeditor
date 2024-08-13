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
        private readonly Dictionary<string, ICValueBox> _collection;
    
        private IVariableCollectionBind _bind;

        public VariableCollection(int capacity, IVariableCollectionBind bind = null) : this(capacity, capacity, bind)
        {
        }

        public VariableCollection(int variableSize, int funcSize, IVariableCollectionBind bind)
        {
            _collection = new Dictionary<string, ICValueBox>(variableSize);
            _bind = bind;
        }

        public void SetBind(IVariableCollectionBind bind)
        {
            _bind ??= bind;
        }

        public CValueBox<T> GetVariable<T>(string name)
        {
            if (_collection.TryGetValue(name, out var variable))
            {
                return variable as CValueBox<T>;
            }

            return null;
        }
        
        public ICValueBox GetVariable(string name)
        {
            return _collection.GetValueOrDefault(name);
        }


        public bool TryGetVariable<T>(string name, out CValueBox<T> value)
        {
            value = null;
            if (!_collection.TryGetValue(name, out var cValue)) return false;
            value = cValue as CValueBox<T>;
            return true;
        }

        public void Add(string key, ICValueBox variable)
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