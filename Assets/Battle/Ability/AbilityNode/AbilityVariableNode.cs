using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Auto;
using Battle.Def;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 变量节点，执行变量创建或者指定变量修改
        /// </summary>
        private class AbilityVariableNode : AbilityNode
        {
            private readonly VariableNodeData _varData;
            private readonly Queue<Param> _params;

            public AbilityVariableNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _varData = data.VariableNodeData;
                _params = new Queue<Param>(_varData.VarParams);
            }

            public override void DoJob()
            {
                if (_params.TryCallFunc(out var variableBox))
                {
                    if (_varData.OperationType == EVariableOperationType.Create)
                    {
                        AbilityExtension.CreateVariable(_varData.Range, _varData.Name, variableBox);
                    }
                    else
                    {
                        var variable = AbilityExtension.GetVariableBox(_varData.Range, _varData.Name);
                        variable.Set(variableBox);
                    }
                }
                else
                {
                    Debug.LogError($"函数执行失败 Name {_varData.Name}");
                }
            }
        }
    }
}