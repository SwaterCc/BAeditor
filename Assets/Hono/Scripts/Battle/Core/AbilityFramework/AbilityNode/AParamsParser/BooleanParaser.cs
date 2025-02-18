using System;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class BooleanParser
        {
            public static bool Parse(in Ability ability, ANode node, AParams aParams)
            {
               
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return (RefBoolean)aParams.Value;
                    case EParamType.Function:
                        var booleanWarp = (IReturnBoolean)AbilityEnv.GetWrapFunc(aParams.funcName);
                        return booleanWarp.CallFunc(ability, node, aParams.funcParams);
                    case EParamType.Variable:
                        if (node.TryGetVariable(aParams.variableName,out bool value))
                        {
                            return value;
                        }
                        break;
                    case EParamType.Attr:
                        return ability.Unit.GetAttr(aParams.attrType) > 0;
                }

                return false;
            }
        }
    }
}