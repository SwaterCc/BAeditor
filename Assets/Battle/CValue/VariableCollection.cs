using System.Collections.Generic;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public interface IVariableCollectionBind
    {
        public VariableCollection GetVariableCollection();
    }

    public class VariableCollection
    {
        private readonly Dictionary<string, Variable> _collection;
    
        private IVariableCollectionBind _bind;

        public VariableCollection(int capacity, IVariableCollectionBind bind = null) : this(capacity, capacity, bind)
        {
        }

        public VariableCollection(int variableSize, int funcSize, IVariableCollectionBind bind)
        {
            _collection = new Dictionary<string, Variable>(variableSize);
            _bind = bind;
        }

        public void SetBind(IVariableCollectionBind bind)
        {
            _bind ??= bind;
        }

        public Variable<T> GetVariable<T>(string name)
        {
            if (_collection.TryGetValue(name, out var variable))
            {
                return variable as Variable<T>;
            }

            return null;
        }

        public bool TryGetVariable<T>(string name, out Variable<T> value)
        {
            value = null;
            if (!_collection.TryGetValue(name, out var cValue)) return false;
            value = cValue as Variable<T>;
            return true;
        }

        public void Add(string key, Variable variable)
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