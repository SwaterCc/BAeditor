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
                        if (aParams.Value is RefInt refInt)
                        {
                            return refInt;
                        }

                        if (aParams.Value is RefFloat refFloat)
                        {
                            return (int)(refFloat.Value);
                        }

                        break;
                    case EParamType.Function:
                        var wrap = AbilityEnv.GetWrapFunc(aParams.funcName);
                        switch (wrap)
                        {
                            case IReturnInt intWarp:
                                return intWarp.CallFunc(ability, node, aParams.funcParams);
                            case IReturnFloat floatWarp:
                                return (int)floatWarp.CallFunc(ability, node, aParams.funcParams);
                        }

                        break;
                    case EParamType.Variable:
                    {
                        if (ability.VariableBoard.TryGet(aParams.variableName, out int value))
                        {
                            return value;
                        }

                        if (ability.VariableBoard.TryGet(aParams.variableName, out float fValue))
                        {
                            return (int)fValue;
                        }

                        break;
                    }
                    case EParamType.Attr:
                        return ability.Unit.GetAttr(aParams.attrType);
                    case EParamType.ListenerInfo:
                    {
                        VariableBoard borad = null;
                        if (node is ATimerNode timerNode)
                        {
                            borad = timerNode.ListenerBoard;
                        }
                        else
                        {
                            if (node.TryGetParent<AListenerNode>(out var parent))
                            {
                                borad = parent.Board;
                            }
                            else
                            {
                                return 0;
                            }
                        }

                        if (borad.TryGet(aParams.variableName, out int value))
                        {
                            return value;
                        }

                        if (borad.TryGet(aParams.variableName, out float fValue))
                        {
                            return (int)fValue;
                        }

                        return 0;
                    }
                }

                throw new Exception("");
            }
        }
    }
}