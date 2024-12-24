using System;

namespace Hono.Scripts.Battle.Base
{
    public partial class AParamParser
    {
        private class BooleanParser
        {
            public bool Parse(in Ability ability,AParams aParams)
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
                        var wrap = Ability.AFuncInvoker.Instance.Get(aParams.funcName);
                        if (wrap is Ability.IReturnBoolean booleanWarp)
                        {
                            return booleanWarp.CallFunc(ability, aParams.funcParams);
                        }
                        break;
                    case EParamType.Variable:
                        if (ability.VariableBoard.TryGet(aParams.variableName, out bool value))
                        {
                            return value;
                        }
                        break;
                    case EParamType.Attr:
                        return ability.Actor.GetAttr(aParams.attrType) > 0;
                }
                
                throw new Exception("");
            }
    }
    
    }
}