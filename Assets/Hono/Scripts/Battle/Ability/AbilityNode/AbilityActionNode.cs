using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// ActionNode 执行动作
        /// </summary>
        private class AbilityActionNode : AbilityNode
        {
            private new readonly ActionNodeData _data;
            
            private object _funcResult;
            public object FuncResult => _funcResult;

            public AbilityActionNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _data = data as ActionNodeData;
            }

            public override void DoJob()
            {
                _data.Function?.TryCallFunc(out _funcResult);
                
                DoChildrenJob();
            }
        }
    }
}