#region

using System;
using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityFunctionDesc : Attribute
    {
        public readonly string FunctionDesc;
        public readonly List<string> ParamsDesc;
        public readonly string FunctionReturnDesc;
        
        /// <summary>
        /// 描述标签
        /// </summary>
        /// <param name="funcDesc">函数功能描述</param>
        /// <param name="funcReturnDesc">函数返回值描述</param>
        /// <param name="paramsDesc">参数描述，按顺序显示</param>
        public AbilityFunctionDesc(string funcDesc , string funcReturnDesc, params string[] paramsDesc)
        {
            FunctionDesc = funcDesc;
            FunctionReturnDesc = funcReturnDesc;
            if (paramsDesc.Length > 0)
            {
                ParamsDesc = new List<string>(paramsDesc);
            }
        }
    }
}