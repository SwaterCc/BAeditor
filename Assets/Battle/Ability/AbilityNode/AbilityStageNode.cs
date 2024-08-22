namespace Battle
{
    public partial class Ability
    {
        private interface IStageNodeProxy
        {
            public int GetId();
            public void StageBegin();
            public void StageEnd();
        }
        
        private class AbilityStageNode : AbilityNode,IStageNodeProxy
        {
            private StageNodeData _stageData;

            
            
            public AbilityStageNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _stageData = data.StageNodeData;
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

            public int GetId()
            {
                return _stageData.StageId;
            }
            
            public void StageBegin()
            {
                resetChildren();
                _executor.ExecuteNode(NodeData.ChildrenIds[0]);
            }

            public void StageEnd()
            {
               
            }
        }   
    }
}