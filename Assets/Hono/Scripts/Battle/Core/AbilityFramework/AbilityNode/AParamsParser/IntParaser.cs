using System;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class IntParser
        {
            public static int Parse(Ability ability, ANode node, AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return (RefInt)aParams.Value;
                    case EParamType.Function:
                        var wrap = (IReturnInt)AbilityEnv.GetWrapFunc(aParams.funcName);
                        return wrap.CallFunc(ability, node, aParams.funcParams);
                    case EParamType.Variable:
                    {
                        if (node.TryGetVariable(aParams.variableName, out int value))
                        {
                            return value;
                        }
                        break;
                    }
                    case EParamType.Attr:
                        return ability.Unit.GetAttr(aParams.attrType);
                }

                return Int32.MinValue;
            }
        }
    }
}