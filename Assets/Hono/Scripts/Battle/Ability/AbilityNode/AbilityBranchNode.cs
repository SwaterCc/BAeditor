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
            private BranchNodeData _branchNode;

            public AbilityBranchNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _conditionRes = false;
                _branchNode = _data as BranchNodeData;
            }

            protected override void onReset()
            {
                _conditionRes = false;
            }

            public override void DoJob()
            {
                _branchNode.CompareFunc.ParseParameters(out var conditionRes);
                _conditionRes = (bool)conditionRes;
                if (_conditionRes)
                {
                    DoChildrenJob();
                }
                else
                {
                    _executor.ExecuteNode(_branchNode.LinkBranchNodeId);
                }
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