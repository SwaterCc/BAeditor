using System;
using System.Linq;
using Battle.Def;
using UnityEngine;

namespace Battle
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
                    case EAbilityNodeType.EWait:
                        return new AbilityWaitNode(executor, data);
                    case EAbilityNodeType.EBranchControl:
                        return new AbilityBranchNode(executor, data);
                    case EAbilityNodeType.EVariableControl:
                        return new AbilityVariableNode(executor, data);
                    case EAbilityNodeType.ERepeat:
                        return new AbilityRepeatNode(executor, data);
                    case EAbilityNodeType.EAction:
                        return new AbilityActionNode(executor, data);
                }

                Debug.LogError($"创建节点失败，节点ID{data.NodeUid} 类型 {data.NodeType}");
                return new AbilityWaitNode(executor, data);
            }
            
            public int ConfigId;

            public AbilityNodeData NodeData;

            //该节点是否以及运行过
            public bool JobFinish;

            public bool CanDeep;

            protected AbilityExecutor _executor;

            protected AbilityNode(AbilityExecutor executor, AbilityNodeData data)
            {
                JobFinish = false;
                NodeData = data;
                ConfigId = NodeData.NodeUid;
                CanDeep = true;
            }

            public virtual void Doing()
            {
                DoJob();
            }
            
            /// <summary>
            /// 执行节点功能
            /// </summary>
            public abstract void DoJob();
            
            protected int GetNextNode()
            {
                if (NodeData.ChildrenUids.Count > 0 && !_executor.nodeIsPast(NodeData.ChildrenUids[0])) 
                {
                    //有子节点返回第一个子节点
                    return NodeData.ChildrenUids[0];
                }

                if (NodeData.NextIdInSameLevel > 0)
                {
                    //没有子节点返回自己下一个相邻节点,不用判执行，因为理论上不会跳着走
                    return NodeData.NextIdInSameLevel;
                }

                if (NodeData.Parent > 0)
                {
                    return _executor.GetNode(NodeData.Parent).GetNextNode();
                }

                return -1;
            }
            
            public virtual bool TryGetChildren(out int childIdx)
            {
                childIdx = 0;
                return NodeData.ChildrenUids.Count > 1 && CanDeep;
            }

            public virtual bool TryGetBrother(out int brother)
            {
                brother = NodeData.NextIdInSameLevel;
                return NodeData.NextIdInSameLevel > 0;
            }
        }
    }
}