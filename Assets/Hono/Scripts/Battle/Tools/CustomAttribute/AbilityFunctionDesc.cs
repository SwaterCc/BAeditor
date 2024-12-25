#region

using System;
using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityFunctionDesc : Attribute
    {
        public string FunctionDesc;
        public List<string> ParamsDesc;
        
        public AbilityFunctionDesc(string funcDesc ,params string[] paramsDesc)
        {
            FunctionDesc = funcDesc;
            if (paramsDesc.Length > 0)
            {
                ParamsDesc = new List<string>(paramsDesc);
            }
        }
    }
}