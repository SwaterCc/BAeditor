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
            private readonly Stack<Param> _actionNodeData;

            public AbilityActionNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _actionNodeData = new Stack<Param>(NodeData.ActionNodeData);
            }
            
            
            public override void DoJob()
            {
                var func = _actionNodeData.Pop();
                if (func.IsFunc)
                {
                    _actionNodeData.CallFunc(func);
                }
                else
                {
                    Debug.LogError("Action节点执行错误");
                   
                }
            }
        }
    }
}