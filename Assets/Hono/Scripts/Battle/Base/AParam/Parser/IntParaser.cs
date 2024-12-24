using System;

namespace Hono.Scripts.Battle.Base
{
    public partial class AParamParser
    {
        private class IntParser 
        {
            public int Parse(in Ability ability,AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        if (aParams.Value is RefInt refInt)
                        {
                            return refInt;
                        }
                        break;
                    case EParamType.Function:
                        var wrap = Ability.AFuncInvoker.Instance.Get(aParams.funcName);
                        switch (wrap)
                        {
                            case Ability.IReturnInt intWarp:
                                return intWarp.CallFunc(ability, aParams.funcParams);
                            case Ability.IReturnFloat floatWarp:
                                return (int)floatWarp.CallFunc(ability, aParams.funcParams);
                        }
                        break;
                    case EParamType.Variable:
                        if (ability.VariableBoard.TryGet(aParams.variableName, out int value))
                        {
                            return value;
                        }
                        if (ability.VariableBoard.TryGet(aParams.variableName, out float fValue))
                        {
                            return (int)fValue;
                        }
                        break;
                    case EParamType.Attr:
                        return ability.Actor.GetAttr(aParams.attrType);
                }
                
                throw new Exception("");
            }
    }
    
    }
}