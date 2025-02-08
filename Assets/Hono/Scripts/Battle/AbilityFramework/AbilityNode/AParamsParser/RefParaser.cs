using System;
using Hono.Scripts.Battle.AbilitySystem;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class RefParser
        {
            public static object Parse(Ability ability, ANode node, AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return aParams.Value;
                    case EParamType.Function:
                        var wrap = AbilityEnv.GetWrapFunc(aParams.funcName);
                        if (wrap is IReturnRef refWarp)
                        {
                            return refWarp.CallFunc(ability, node, aParams.funcParams);
                        }

                        break;
                    case EParamType.Variable:
                        if (ability.VariableBoard.TryGetRef(aParams.variableName, out object value))
                        {
                            return value;
                        }

                        break;
                }

                throw new Exception("");
            }
        }
    }
}