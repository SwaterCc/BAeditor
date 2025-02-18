using System;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class Vector3Parser
        {
            public static Vector3 Parse(in Ability ability, ANode node, AParams aParams)
            {
                switch (aParams.paramType)
                {
                    case EParamType.Simple:
                        return (RefVector3)aParams.Value;
                    case EParamType.Function:
                        var wrap = (IReturnVector3)AbilityEnv.GetWrapFunc(aParams.funcName);
                        return wrap.CallFunc(ability, node, aParams.funcParams);
                    case EParamType.Variable:
                        if (node.TryGetVariable(aParams.variableName, out Vector3 value))
                        {
                            return value;
                        }

                        break;
                }

                return Vector3.negativeInfinity;
            }
        }
    }
}