
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityEventNode : AbilityNode
        {
            private EventChecker _checker;
            private EventNodeData _eventNodeData;

            public AbilityEventNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _eventNodeData = (EventNodeData)_data;
            }

            public void RegisterEvent()
            {
                if (!_eventNodeData.CreateChecker.Parse(out _checker))
                {
                    Debug.LogError("Event½ÚµãÖ´ÐÐÊ§°Ü");
                    return;
                }
                
                _checker.BindFunc(onEventFired);
                BattleEventManager.Instance.Register(_checker);
            }

            public void UnRegisterEvent()
            {
                _checker.UnRegister();
            }

            private void onEventFired(IEventInfo eventInfo)
            {
	            _context.UpdateContext((_executor.Ability.Actor, _executor.Ability));
	            eventInfo.SetFieldsInAbilityVariables(_executor.Ability);
	            DoChildrenJob();
                eventInfo.ClearFields(_executor.Ability);
	            _context.ClearContext();
            }

            public override void DoJob() { }
        }
    }
}