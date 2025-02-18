using System;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class FloatParser
        {
            public static float Parse(Ability ability, ANode node, AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return (RefFloat)aParams.Value;
                    case EParamType.Function:
                        var wrap = (IReturnFloat)AbilityEnv.GetWrapFunc(aParams.funcName);
                        return wrap.CallFunc(ability, node, aParams.funcParams);
                    case EParamType.Variable:
                        if (node.TryGetVariable(aParams.variableName,out float value))
                        {
                            return value;
                        }
                        break;
                    case EParamType.Attr:
                        return ability.Unit.GetAttr(aParams.attrType);
                }

                return Single.MinValue;
            }
        }
    }
}