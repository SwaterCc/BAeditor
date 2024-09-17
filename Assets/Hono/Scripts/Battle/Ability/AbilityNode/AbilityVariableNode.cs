
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 变量节点，执行变量创建或者指定变量修改
        /// </summary>
        private class AbilityVariableNode : AbilityNode
        {
            private readonly VarSetterNodeData _varData;

            public AbilityVariableNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _varData = (VarSetterNodeData)data;
            }

            public override void DoJob()
            {
                object variable = null;
                
                if (Parent.NodeType == EAbilityNodeType.EAction)
                {
                    variable = ((AbilityActionNode)Parent).FuncRes;
                }
                else
                {
                    if (!_varData.Value.Parse(out variable))
                    {
                        Debug.LogError($"函数执行失败 Name {_varData.Name}");
                        return;
                    }
                }
                
                _executor.Ability.Variables.Set(_varData.Name,variable);
                
                DoChildrenJob();
            }
        }
    }
}