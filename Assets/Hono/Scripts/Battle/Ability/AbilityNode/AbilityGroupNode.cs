

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private interface IGroupNodeProxy
        {
            public int GetGroupId();
            public void GroupBegin();
            public void GroupEnd();
        }
        
        private class AbilityGroupNode : AbilityNode,IGroupNodeProxy
        {
            private GroupNodeData _groupData;
            
            public AbilityGroupNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _groupData = data.GroupNodeData;
            }
            public override void DoJob()
            {
                
            }

            public override int GetNextNode()
            {
                //执行到阶段节点时不执行其子节点，其子节点以及被托管给了Executing状态
                if (NodeData.NextIdInSameLevel > 0)
                {
                    //没有子节点返回自己下一个相邻节点,不用判执行，因为理论上不会跳着走
                    return NodeData.NextIdInSameLevel;
                }

                if (NodeData.Parent > 0)
                {
                    return NodeData.Parent;
                }

                return -1;
            }

            public int GetGroupId()
            {
                return _groupData.GroupId;
            }
            
            public void GroupBegin()
            {
                ((ExecutingCycle)_executor.State.Current).NextGroupId = -1;
                resetChildren();
                _executor.ExecuteNode(NodeData.ChildrenIds[0]);
            }

            public void GroupEnd()
            {
               
            }
        }   
    }
}