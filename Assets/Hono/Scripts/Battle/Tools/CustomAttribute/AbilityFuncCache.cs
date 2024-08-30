using System;

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityFuncCache : System.Attribute
    {
        public EFuncCacheFlag Flag;

        public string FuncDesc;

        public string[] ParamDesc;

        public int OverLoadedTag;

        public AbilityFuncCache(EFuncCacheFlag flag = EFuncCacheFlag.OnlyCache,int overloadedTag = 0, string funcDesc = "",
            params string[] paramDesc)
        {
            Flag = flag;
            FuncDesc = funcDesc;
            ParamDesc = paramDesc;
            OverLoadedTag = overloadedTag;
        }
    }
}