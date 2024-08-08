using System.Collections.Generic;
using Battle.Tools;

namespace Battle
{
    /// <summary>
    /// 初始化后只读
    /// </summary>
    /// <typeparam name="TValueType">变量类型</typeparam>
    public class Variable<TValueType> : CValue
    {
        private new TValueType _value;
        public string Name { get; }

        public Variable(string name,TValueType value)
        {
            Name = name;
            _value = value;
        }

        public new TValueType Get()
        {
            return _value;
        }
            
        private new void Set(object value)
        {
        }
    }
    
    public class VariableFunc<TFuncReturnType> : CValue
    {
        public string Name { get; }

        private readonly int _funcId;

        private readonly ParamCollection _param;
        
        public VariableFunc(string name,int funcId,ParamCollection param)
        {
            Name = name;
            _funcId = funcId;
            _param = param;
        }

        public new TFuncReturnType Get()
        {
            return AbilityHelper.InvokeFunc<TFuncReturnType>(_funcId,_param);
        }
            
        private new void Set(object value)
        {
        }
    }
    
    public class VariableCollection
    {
        private Dictionary<string, CValue> _collection = new();

        public void Add(string key, CValue value)
        {
            if (!_collection.TryAdd(key, value))
            {
                //报个错
            }
        }
    }
}