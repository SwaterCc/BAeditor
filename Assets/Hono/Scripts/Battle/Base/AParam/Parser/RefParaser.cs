using System;

namespace Hono.Scripts.Battle.Base
{
    public partial class AParamParser
    {
        private class RefParser
        {
            public object Parse(in Ability ability, AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return aParams.Value;
                    case EParamType.Function:
                        var wrap = Ability.AFuncInvoker.Instance.Get(aParams.funcName);
                        if (wrap is Ability.IReturnRef refWarp)
                        {
                            return refWarp.CallFunc(ability, aParams.funcParams);
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