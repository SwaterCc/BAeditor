using System;
using Battle.Def;
using Battle.Event;
using Battle.Tools;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityEventNode : AbilityNode
        {
            private EventChecker _checker;
            public AbilityEventNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }
                        
            public void RegisterEvent()
            {
                if (!NodeData.EventNodeData.TryCallFunc(out var valueBox)) return;
                _checker = ((ValueBox<EventChecker>)valueBox).Get();
                BattleEventManager.Instance.Register(_checker);
            }

            public void UnRegisterEvent()
            {
                _checker.UnRegister();
            }
            
            public override void DoJob()
            {
                
            }
        }
    }
}