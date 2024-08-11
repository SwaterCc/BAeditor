using Battle.Tools;

namespace Battle
{
    /// <summary>
    /// Variable 变量，其功能和定义变量一样，只在指定域内生效，
    /// 当域的生命结束则会跟着一起释放，且同域内变量名不可重复
    /// </summary>
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
}