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
            public AbilityActionNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }


            public override void DoJob()
            {
                if (NodeData.ActionNodeData[0].IsFunc)
                {
                    NodeData.ActionNodeData.TryCallFunc(out _);
                }
            }
        }
    }
}