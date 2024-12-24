using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Base
{
    public partial class AParamParser
    {
        private class Vector3Parser
        {
            public Vector3 Parse(in Ability ability, AParams aParams)
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
                        var wrap = Ability.AFuncInvoker.Instance.Get(aParams.funcName);
                        if (wrap is Ability.IReturnVector3 vector3Warp)
                            return vector3Warp.CallFunc(ability, aParams.funcParams);
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