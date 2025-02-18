using System;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class RefParser
        {
            public static object Parse(Ability ability, ANode node, AParams aParams)
            {
                if (aParams == null) return null;

                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return aParams.Value;
                    case EParamType.Function:
                        var wrap = (IReturnRef)AbilityEnv.GetWrapFunc(aParams.funcName);
                        return wrap.CallFunc(ability, node, aParams.funcParams);
                    case EParamType.Attr:
                        Debug.LogWarning($"aParams getVariable {aParams.attrType} 产生了装箱");
                        return ability.Unit.GetAttr(aParams.attrType);
                    case EParamType.Variable:
                        if (Type.GetType(aParams.paramCastType) == typeof(RefInt))
                        {
                            if (node.TryGetVariable(aParams.variableName, out int intValue))
                            {
                                Debug.LogWarning($"aParams getVariable {aParams.variableName} 产生了装箱");
                                return intValue;
                            }
                        }

                        if (Type.GetType(aParams.paramCastType) == typeof(RefFloat))
                        {
                            if (node.TryGetVariable(aParams.variableName, out float fValue))
                            {
                                Debug.LogWarning($"aParams getVariable {aParams.variableName} 产生了装箱");
                                return fValue;
                            }
                        }

                        if (Type.GetType(aParams.paramCastType) == typeof(RefVector3))
                        {
                            if (node.TryGetVariable(aParams.variableName, out Vector3 v3Value))
                            {
                                Debug.LogWarning($"aParams getVariable {aParams.variableName} 产生了装箱");
                                return v3Value;
                            }
                        }

                        if (Type.GetType(aParams.paramCastType) == typeof(RefBoolean))
                        {
                            if (node.TryGetVariable(aParams.variableName, out bool bValue))
                            {
                                Debug.LogWarning($"aParams getVariable {aParams.variableName} 产生了装箱");
                                return bValue;
                            }
                        }

                        if (node.TryGetVariableRef(aParams.variableName, out object value))
                        {
                            return value;
                        }

                        Debug.LogError($"[RefParser] key {aParams.variableName} 找不到指定类型的引用变量");
                        return null;
                }

                return null;
            }
        }
    }
}