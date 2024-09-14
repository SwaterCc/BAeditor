using Hono.Scripts.Battle.Tools;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityBranchNode : AbilityNode
        {
            private bool _conditionRes;
            public bool Result => _conditionRes;
            
            private BranchNodeData _branchNode;

            public AbilityBranchNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _conditionRes = false;
                _branchNode = _data.BranchNodeData;
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
                
                if (_conditionRes)
                {
	                DoChildrenJob();
                }

                Parent.IfInfos[_data.BranchNodeData.BranchGroup] = _conditionRes;
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
        }
    }
}