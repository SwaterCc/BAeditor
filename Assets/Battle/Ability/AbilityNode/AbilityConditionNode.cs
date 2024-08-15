using System.Collections.Generic;
using Battle.Def;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityBranchNode : AbilityNode
        {
            private readonly Queue<Param> _branchNodeStack;
            private bool _conditionRes;

            public AbilityBranchNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _branchNodeStack = new Queue<Param>(NodeData.BranchNodeData);
                _conditionRes = false;
            }

            protected override void onReset()
            {
                _conditionRes = false;
            }

            public override void DoJob()
            {
                var func = _branchNodeStack.Dequeue();
                if (_branchNodeStack.TryCallFunc(out var res))
                {
                    _conditionRes = ((ValueBox<bool>)res).Get();
                }
                else
                {
                    Debug.LogError("Branch节点执行错误");
                }
            }

            public override int GetNextNode()
            {
                if (_conditionRes && NodeData.ChildrenUids.Count > 0 &&
                    !_executor.IsPassedNode(NodeData.ChildrenUids[0]))
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
                    return NodeData.Parent;
                }

                return -1;
            }
        }
    }
}