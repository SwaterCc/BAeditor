using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private static class AbilityFuncWrapInit
        {
            public static void __Gen_AFuncWrap_Register()
            {
                AbilityEnv.RegisterWrap("GetBuffLayer", new __Gen_AFuncWrap_GetBuffLayer());
            }
        }


        private class __Gen_AFuncWrap_GetBuffLayer : IReturnInt
        {
            public int CallFunc(Ability caller, ANode node, List<AParams> @params)
            {
                var p0 = node.ParseInt(@params[0]);
                var p1 = node.ParseInt(@params[1]);
                return caller._functionDefine.GetBuffLayer(p0, p1);
            }
        }
    }
}