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
                        if (aParams.Value is RefVector3 refV3)
                        {
                            return refV3;
                        }

                        break;
                    case EParamType.Function:
                        var wrap = AbilityEnv.GetWrapFunc(aParams.funcName);
                        if (wrap is IReturnVector3 vector3Warp)
                            return vector3Warp.CallFunc(ability, node, aParams.funcParams);
                        break;
                    case EParamType.Variable:
                        if (ability.VariableBoard.TryGet(aParams.variableName, out Vector3 value))
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