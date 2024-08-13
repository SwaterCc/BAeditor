using System.Collections.Generic;
using Battle.Def;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityBranchNode : AbilityNode
        {
            private readonly Stack<Param> _branchNodeStack;
            private bool _conditionRes;
            
            public AbilityBranchNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _branchNodeStack = new Stack<Param>(NodeData.BranchNodeData);
                _conditionRes = true;
            }

            public override void DoJob()
            {
                var func = _branchNodeStack.Pop();
                if (func.IsFunc)
                {
                    CanDeep = ((CValueBox<bool>)_branchNodeStack.CallFunc(func)).Get();
                }
                else
                {
                    Debug.LogError("Branch节点执行错误");
                }
            }
        }
    }
}