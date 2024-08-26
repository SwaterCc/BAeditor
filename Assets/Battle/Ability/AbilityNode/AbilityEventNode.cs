using System;
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
                if (!NodeData.EventNodeData.CreateCheckerFunc.TryCallFunc(out var valueBox)) return;
                _checker = (EventChecker)valueBox;
                BattleEventManager.Instance.Register(_checker);
            }

            public void UnRegisterEvent()
            {
                _checker.UnRegister();
            }

            
            
            public void OnEventFired()
            {
                
            }
            
            public override void DoJob()
            {
                
            }
        }
    }
}