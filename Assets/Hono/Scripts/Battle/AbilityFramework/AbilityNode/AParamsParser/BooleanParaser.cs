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
                        if (aParams.Value is RefBoolean refBoolean)
                        {
                            return refBoolean;
                        }

                        break;
                    case EParamType.Function:
                        var wrap = AbilityEnv.GetWrapFunc(aParams.funcName);
                        if (wrap is IReturnBoolean booleanWarp)
                        {
                            return booleanWarp.CallFunc(ability, node, aParams.funcParams);
                        }

                        break;
                    case EParamType.Variable:
                        if (ability.VariableBoard.TryGet(aParams.variableName, out bool value))
                        {
                            return value;
                        }

                        break;
                    case EParamType.Attr:
                        return ability.Unit.GetAttr(aParams.attrType) > 0;
                }

                throw new Exception("");
            }
        }
    }
}