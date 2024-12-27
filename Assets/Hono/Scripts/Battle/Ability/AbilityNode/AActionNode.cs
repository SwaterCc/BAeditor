#region

using System;
using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        ///     ActionNode 执行动作
        /// </summary>
        private class AActionNode : ANode<ActionNodeData>, IAPoolObject
        {
            public object FuncResult { get; private set; }

            public override void DoJob()
            {
                var returnType = Type.GetType(Data.returnType);
                if (returnType == typeof(int))
                {
                    var value = AParamParser.ParseInt(AContext, Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(float))
                {
                    var value = AParamParser.ParseFloat(AContext, Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(bool))
                {
                    var value = AParamParser.ParseBoolean(AContext, Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(Vector3))
                {
                    var value = AParamParser.ParseVector3(AContext, Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                else if (returnType == typeof(void))
                {
                    var wrap = AFuncInvoker.Instance.Get(Data.action.funcName);
                    if (wrap is IReturnVoid voidWrap)
                    {
                        voidWrap.CallFunc(AContext,Data.action.funcParams);
                    }
                }
                else
                {
                    var value = AParamParser.ParseRef(AContext, Data.action);
                    if (Data.isCreateVariable)
                        AContext.VariableBoard.Set(Data.returnValueKey, value);
                }
                
            }

            protected override void onReset()
            {
                FuncResult = null;
            }

            public override void Recycle()
            {
                APool<AActionNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                FuncResult = null;
                ((ANode)this).OnRecycle();
            }
        }
    }
}