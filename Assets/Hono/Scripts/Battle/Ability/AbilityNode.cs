using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private abstract class AbilityNode
        {
            public static AbilityNode CreateNode(AbilityExecutor executor, AbilityNodeData data)
            {
                switch (data.NodeType)
                {
                    case EAbilityNodeType.EAbilityCycle:
                        return new AbilityCycleNode(executor, data);
                    case EAbilityNodeType.ETimer:
                        return new AbilityTimerNode(executor, data);
                    case EAbilityNodeType.EBranchControl:
                        return new AbilityBranchNode(executor, data);
                    case EAbilityNodeType.EVariableControl:
                        return new AbilityVariableNode(executor, data);
                    case EAbilityNodeType.ERepeat:
                        return new AbilityRepeatNode(executor, data);
                    case EAbilityNodeType.EAction:
                        return new AbilityActionNode(executor, data);
                    case EAbilityNodeType.EGroup:
                        return new AbilityGroupNode(executor, data);
                    
                }

                Debug.LogError($"创建节点失败，节点ID{data.NodeId} 类型 {data.NodeType}");
                return new AbilityCycleNode(executor, data);
            }
            
            public int ConfigId;

            public AbilityNodeData NodeData;

            protected AbilityExecutor _executor;

            public bool JobFinish;

            protected AbilityNode(AbilityExecutor executor, AbilityNodeData data)
            {
                NodeData = data;
                ConfigId = NodeData.NodeId;
                _executor = executor;
            }
            
            /// <summary>
            /// 执行节点功能
            /// </summary>
            public abstract void DoJob();

            /// <summary>
            /// 重置该节点，注意，同时该节点会被重置为未执行过
            /// </summary>
            public virtual void Reset()
            {
                _executor.RemovePass(ConfigId);
                onReset();
                resetChildren();
            }

            protected void resetChildren()
            {
                foreach (var childrenUid in NodeData.ChildrenIds)
                {
                    _executor.ResetNode(childrenUid);
                }
            }

            protected virtual void onReset()
            {
                
            }
            
            
            public virtual int GetNextNode()
            {
                if (NodeData.ChildrenIds.Count > 0 && !_executor.IsPassedNode(NodeData.ChildrenIds[0])) 
                {
                    //有子节点返回第一个子节点
                    return NodeData.ChildrenIds[0];
                }

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
        }
    }
}