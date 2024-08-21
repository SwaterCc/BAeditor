using System;
using Battle.Auto;
using Sirenix.OdinInspector;

namespace Battle
{
    /// <summary>
    /// 编辑器反射参数
    /// </summary>
    public struct Parameter
    {
        /// <summary>
        /// 参数类型，反射得到的字符串
        /// </summary>
        public string ParamType;
        
        /// <summary>
        /// 参数命名，反射得到的字符串
        /// </summary>
        public string ParamName;

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
        public object Value;
    }
}