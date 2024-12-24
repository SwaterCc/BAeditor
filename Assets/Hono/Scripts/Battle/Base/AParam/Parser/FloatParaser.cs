using System;

namespace Hono.Scripts.Battle.Base
{
    public partial class AParamParser
    {
        private class FloatParser
        {
            public float Parse(in Ability ability,AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        if (aParams.Value is RefFloat refFloat)
                        {
                            return refFloat;
                        }
                        break;
                    case EParamType.Function:
                        var wrap = Ability.AFuncInvoker.Instance.Get(aParams.funcName);
                        switch (wrap)
                        {
                            case Ability.IReturnInt intWarp:
                                return intWarp.CallFunc(ability, aParams.funcParams);
                            case Ability.IReturnFloat floatWarp:
                                return floatWarp.CallFunc(ability, aParams.funcParams);
                        }
                        break;
                    case EParamType.Variable:
                        if (ability.VariableBoard.TryGet(aParams.variableName, out int value))
                        {
                            return value;
                        }
                        if (ability.VariableBoard.TryGet(aParams.variableName, out float fvalue))
                        {
                            return (int)fvalue;
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