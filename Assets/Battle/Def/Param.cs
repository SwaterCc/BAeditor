using System;
using Battle.Auto;
using Sirenix.OdinInspector;

namespace Battle
{
    /// <summary>
    /// 编辑器反射参数
    /// </summary>
    public sealed class Param
    {
        /// <summary>
        /// 参数类型，反射得到的字符串
        /// </summary>
        public string ParamType;

        /// <summary>
        /// 是否是变量
        /// </summary>
        public bool IsVariable;
        public string VarName;
        public EVariableRange VarRange;

        /// <summary>
        /// 是否是属性
        /// </summary>
        public bool IsAttribute;
        public EAttributeType AttributeType;

        /// <summary>
        /// 是否是函数
        /// </summary>
        public bool IsFunc;
        public string FuncName;

        /// <summary>
        /// 是否是值类型
        /// </summary>
        public bool IsBaseType;
        public int IntValue;
        public long LongValue;
        public float FloatValue;
        public bool BoolValue;
    }
}