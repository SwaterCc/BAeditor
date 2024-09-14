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
            public AbilityActionNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }


            public override void DoJob()
            {
                if (_data.ActionNodeData[0].IsFunc)
                {
	                _data.ActionNodeData.TryCallFunc(out _);
                }
                
                DoChildrenJob();
            }
        }
    }
}