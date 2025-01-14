using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private class FloatParser
        {
            public static float Parse(Ability ability,ANode node,AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        if (aParams.Value is RefFloat refFloat)
                        {
                            return refFloat;
                        }
                        if (aParams.Value is RefInt refInt)
                        {
                            return refInt;
                        }
                        break;
                    case EParamType.Function:
                        var wrap = AbilityEnv.GetWrapFunc(aParams.funcName);
                        switch (wrap)
                        {
                            case Ability.IReturnInt intWarp:
                                return intWarp.CallFunc(ability,node,aParams.funcParams);
                            case Ability.IReturnFloat floatWarp:
                                return floatWarp.CallFunc(ability,node, aParams.funcParams);
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