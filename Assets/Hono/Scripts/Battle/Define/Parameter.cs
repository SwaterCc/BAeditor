using System;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 编辑器反射参数
    /// </summary>
    [Serializable]
    public struct Parameter
    {
        /// <summary>
        /// 是否是函数
        /// </summary>
        public bool IsFunc;
        public string FuncName;
        public int ParamCount;

        /// <summary>
        /// 是否是值类型
        /// </summary>
        public bool IsValueType;
        public AutoValue Value;

        /// <summary>
        /// 是否使用了变量
        /// </summary>
        public bool IsVariable;
        public string VariableName;

        /// <summary>
        /// 是否使用属性
        /// </summary>
        public bool IsAttr;
        public int AttrId;
    }
}