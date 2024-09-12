
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// Set变量或者捕获变量
        /// </summary>
        private class AbilityVariableNode : AbilityNode
        {
            private readonly VariableNodeData _varData;

            public AbilityVariableNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _varData = data as VariableNodeData;
            }

            private void CapturingActionResult(AbilityActionNode actionNode)
            {
                if (actionNode.FuncResult != null)
                {
                    _executor.Variables.Set(_varData.Name,actionNode.FuncResult.DeepCopy());
                }
            }
            
            public override void DoJob()
            {
                if (Parent is AbilityActionNode actionNode)
                {
                    CapturingActionResult(actionNode);
                }
                
                if (!_varData.VarParams.ParseParameters(out var autoValue))
                {
	                Debug.LogError($"函数执行失败 Name {_varData.Name}");
                }
               
                _executor.Variables.Set(_varData.Name,autoValue);
                
                DoChildrenJob();
            }
        }
    }
}