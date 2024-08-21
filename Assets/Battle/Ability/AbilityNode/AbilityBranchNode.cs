using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityBranchNode : AbilityNode
        {
            private bool _conditionRes;
            private BranchNodeData _branchNode;

            public AbilityBranchNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _conditionRes = false;
                _branchNode = NodeData.BranchNodeData;
            }

            protected override void onReset()
            {
                _conditionRes = false;
            }

            public override void DoJob()
            {
                if (!_branchNode.Right.TryCallFunc(out var right))
                {
                    Debug.LogError("Branch节点执行错误");
                }

                if (!_branchNode.Left.TryCallFunc(out var left))
                {
                    Debug.LogError("Branch节点执行错误");
                }

                var res = ((IComparable)left).CompareTo((IComparable)right);

                _conditionRes = getCompareRes(_branchNode.ResType, res);
            }

            private bool getCompareRes(ECompareResType compareResType, int flag)
            {
                switch (compareResType)
                {
                    case ECompareResType.Less:
                        return flag < 0;
                    case ECompareResType.LessAndEqual:
                        return flag <= 0;
                    case ECompareResType.Equal:
                        return flag == 0;
                    case ECompareResType.More:
                        return flag > 0;
                    case ECompareResType.MoreAndEqual:
                        return flag >= 0;
                }

                return true;
            }
            
            public override int GetNextNode()
            {
                if (_conditionRes && NodeData.ChildrenIds.Count > 0 &&
                    !_executor.IsPassedNode(NodeData.ChildrenIds[0]))
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