using System;
using System.Collections.Generic;
using Battle.Auto;
using Battle.Def;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        /// <summary>
        /// ActionNode 执行动作
        /// </summary>
        private class AbilityActionNode : AbilityNode
        {
            private readonly Queue<Parameter> _actionFunc;

            public AbilityActionNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _actionFunc = new Queue<Parameter>(NodeData.ActionNodeData);
            }
            
            
            public override void DoJob()
            {
                _actionFunc.TryCallFunc(out _);
            }
        }
    }
}