using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// ActionNode 执行动作
        /// </summary>
        private class AbilityActionNode : AbilityNode
        {
            private ActionNodeData _nodeData;

            public object FuncRes => _funcRes;

            private object _funcRes;

            public AbilityActionNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _nodeData = (ActionNodeData)base._data;
            }

            public override void DoJob()
            {
                if (_nodeData.Function.ParameterType == EParameterType.Function) {
	                if (!_nodeData.Function.Parse(_executor.Ability, out _funcRes)) 
                    {
                        Debug.LogError("函数执行失败！");
                    }
                }
                
                DoChildrenJob();
            }
        }
    }
}