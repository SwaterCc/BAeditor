#region

using System;
using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        /// <summary>
        ///     AFunctionNode 执行动作
        /// </summary>
        private class AFunctionNode : ANode<FunctionNodeData>, ICPoolObject
        {
            public override void DoJob()
            {
                var returnType = Type.GetType(Data.returnType);
                if (returnType == typeof(int))
                {
                    var value = ParseInt(Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(float))
                {
                    var value = ParseFloat(Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(bool))
                {
                    var value = ParseBoolean(Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(Vector3))
                {
                    var value = ParseVector3(Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(void))
                {
                    var wrap = AbilityEnv.GetWrapFunc(Data.action.funcName);
                    if (wrap is IReturnVoid voidWrap)
                    {
                        voidWrap.CallFunc(AContext, this, Data.action.funcParams);
                    }
                }
                else
                {
                    var value = ParseRef(Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.SetRef(Data.returnValueKey, value);
                }
            }

            public override void Recycle()
            {
                GPool<AFunctionNode>.Pool.Recycle(this);
            }
        }
    }
}