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
                _branchNode = (BranchNodeData)_data;
            }

            protected override void onReset()
            {
                _conditionRes = false;
            }

            public override void DoJob()
            {
                if (!_branchNode.CompareFunc.Parse(_executor.Ability, out _conditionRes))
                {
                    Debug.LogError("Branch节点执行错误");
                    return;
                }
                
                if (_conditionRes)
                {
	                DoChildrenJob();
                }

                Parent.IfInfos[_branchNode.BranchGroup] = _conditionRes;
            }
        }
    }
}