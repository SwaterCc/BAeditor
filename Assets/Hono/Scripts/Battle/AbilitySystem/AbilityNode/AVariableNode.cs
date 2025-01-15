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
        ///  变量节点，执行变量创建或者指定变量修改
        /// </summary>
        private class AVariableNode : ANode<VariableNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
                var type = Data.value.GetParamType();

                if (type == null)
                {
                    AContext.LogError($"AVariableNode 获取类型失败 {Data.value.paramCastType}");
                    return;
                }

                if (!Data.isModify)
                {
                    setVariable(type);
                }
                else
                {
                    modifyVariable(type);
                }
            }

            private void setVariable(Type varType)
            {
                if (varType == typeof(RefInt))
                {
                    var value = ParseInt(Data.value);
                    AContext.VariableBoard.Set(Data.key, value);
                }
                else if (varType == typeof(RefFloat))
                {
                    var value = ParseFloat(Data.value);
                    AContext.VariableBoard.Set(Data.key, value);
                }
                else if (varType == typeof(RefBoolean))
                {
                    var value = ParseBoolean(Data.value);
                    AContext.VariableBoard.Set(Data.key, value);
                }
                else if (varType == typeof(RefVector3))
                {
                    var value = ParseVector3(Data.value);
                    AContext.VariableBoard.Set(Data.key, value);
                }
                else
                {
                    var value = ParseRef(Data.value);
                    AContext.VariableBoard.SetRef(Data.key, value);
                }
            }

            private void modifyVariable(Type varType)
            {
                if (varType == typeof(RefInt))
                {
                    var modifyValue = ParseInt(Data.value);

                    if (!AContext.VariableBoard.TryGet<int>(Data.key, out var origin))
                    {
                        AContext.LogError($"modifyVariable error ! key {Data.key} not find");
                        return;
                    }

                    switch (Data.operationType)
                    {
                        case EVariableOperationType.Reset:
                            AContext.VariableBoard.Set(Data.key, modifyValue);
                            break;
                        case EVariableOperationType.Add:
                            AContext.VariableBoard.Set(Data.key, origin + modifyValue);
                            break;
                        case EVariableOperationType.Sub:
                            AContext.VariableBoard.Set(Data.key, origin * modifyValue);
                            break;
                    }
                }
                else if (varType == typeof(RefFloat))
                {
                    var modifyValue = ParseFloat(Data.value);
                    if (!AContext.VariableBoard.TryGet<float>(Data.key, out var origin))
                    {
                        AContext.LogError($"modifyVariable error ! key {Data.key} not find");
                        return;
                    }

                    switch (Data.operationType)
                    {
                        case EVariableOperationType.Reset:
                            AContext.VariableBoard.Set(Data.key, modifyValue);
                            break;
                        case EVariableOperationType.Add:
                            AContext.VariableBoard.Set(Data.key, origin + modifyValue);
                            break;
                        case EVariableOperationType.Sub:
                            AContext.VariableBoard.Set(Data.key, origin * modifyValue);
                            break;
                    }
                }
                else if (varType == typeof(RefBoolean))
                {
                    var modifyValue = ParseBoolean(Data.value);
                    if (!AContext.VariableBoard.TryGet<bool>(Data.key, out var origin))
                    {
                        AContext.LogError($"modifyVariable error ! key {Data.key} not find");
                        return;
                    }

                    switch (Data.operationType)
                    {
                        case EVariableOperationType.Reset:
                            AContext.VariableBoard.Set(Data.key, modifyValue);
                            break;
                        case EVariableOperationType.Reverse:
                            AContext.VariableBoard.Set(Data.key, !origin);
                            break;
                    }
                }
                else if (varType == typeof(RefVector3))
                {
                    var modifyValue = ParseVector3(Data.value);
                    AContext.VariableBoard.Set(Data.key, modifyValue);
                }
            }

            public override void Recycle()
            {
                APool<AVariableNode>.Pool.Recycle(this);
            }
        }
    }
}