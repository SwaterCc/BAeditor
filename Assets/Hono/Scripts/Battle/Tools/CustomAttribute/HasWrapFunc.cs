using System;

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HasWrapFunc: Attribute
    {
        public string WrapFuncName;

        public HasWrapFunc(string wrapFuncName)
        {
            WrapFuncName = wrapFuncName;
        }
    }
}