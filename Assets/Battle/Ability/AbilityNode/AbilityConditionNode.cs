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
                _conditionRes = true;
            }

            public override void DoJob()
            {
                var func = _branchNodeStack.Dequeue();
                if (_branchNodeStack.TryCallFunc(out var res))
                {
                    CanDeep = ((ValueBox<bool>)res).Get();
                }
                else
                {
                    Debug.LogError("Branch节点执行错误");
                }
            }
        }
    }
}