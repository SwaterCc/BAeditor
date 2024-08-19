using System;

namespace Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityFuncCache : System.Attribute
    {
        public EFuncCacheFlag Flag;

        public string FuncName;

        public string[] Desc;

        public AbilityFuncCache(string funcName, params string[] desc)
        {
            FuncName = funcName;
            Desc = desc;
        }
    }
}