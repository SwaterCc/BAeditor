using System;

namespace Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityFuncTag : System.Attribute
    {
        public static int TagCount = 0;
        public EFuncCacheFlag Flag;

        public AbilityFuncTag(EFuncCacheFlag flag)
        {
            ++TagCount;
            Flag = flag;
        }
    }
}