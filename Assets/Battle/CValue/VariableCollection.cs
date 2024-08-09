using System.Collections.Generic;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    #region Variable

    public class Variable : CValue
    {
        public string Name { get; }

        public Variable(string name)
        {
            Name = name;
        }

        private new void Set(object value)
        {
        }
    }

    public class Variable<TValueType> : Variable
    {
        protected new TValueType _value;

        public Variable(string name, TValueType value) : base(name)
        {
            _value = value;
        }

        public new TValueType Get()
        {
            return _value;
        }
    }

    public class VariableFunc<TFuncReturnType> : Variable<TFuncReturnType>
    {
        private readonly int _funcId;

        private readonly ParamCollection _param;

        public VariableFunc(string name, int funcId, ParamCollection param) : base(name, default)
        {
        }

        public new TFuncReturnType Get()
        {
            _value = AbilityHelper.InvokeFunc<TFuncReturnType>(new Ability(1), _funcId, _param);
            return _value;
        }
    }

    #endregion

    public interface IVariableCollectionBind
    {
        public VariableCollection GetCollection();
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