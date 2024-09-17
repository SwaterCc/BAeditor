
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
            private MessageListener _messageListener;
            public AbilityEventNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _eventNodeData = (EventNodeData)_data;
            }

            public void RegisterEvent()
            {
                if (_eventNodeData.IsEvent)
                {
                    if (!_eventNodeData.CreateChecker.Parse(out _checker))
                    {
                        Debug.LogError("Event�ڵ�ִ��ʧ��");
                        return;
                    }
                
                    _checker.BindFunc(onEventFired);
                    BattleEventManager.Instance.Register(_checker);
                }
                else
                {
                    _messageListener = new MessageListener(_eventNodeData.MsgName, onMsgCall);
                    _executor.Ability.Actor.AddMsgListener(_messageListener);
                }
            }

            public void UnRegisterEvent()
            {
                if (_eventNodeData.IsEvent)
                {
                    _checker.UnRegister();
                }
                else
                {
                    _executor.Ability.Actor.AddMsgListener(_messageListener);
                }
            }

            private void onEventFired(IEventInfo eventInfo)
            {
	            _context.UpdateContext((_executor.Ability.Actor, _executor.Ability));
	            eventInfo.SetFieldsInAbilityVariables(_executor.Ability);
	            DoChildrenJob();
                eventInfo.ClearFields(_executor.Ability);
	            _context.ClearContext();
            }

            private void onMsgCall(object p1,object p2,object p3,object p4,object p5)
            {
                _context.UpdateContext((_executor.Ability.Actor, _executor.Ability));
                _executor.Ability.Variables.Set("P1",p1);
                _executor.Ability.Variables.Set("P2",p2);
                _executor.Ability.Variables.Set("P3",p3);
                _executor.Ability.Variables.Set("P4",p4);
                _executor.Ability.Variables.Set("P5",p5);
                DoChildrenJob();
                _executor.Ability.Variables.Delete("P1");
                _executor.Ability.Variables.Delete("P2");
                _executor.Ability.Variables.Delete("P3");
                _executor.Ability.Variables.Delete("P4");
                _executor.Ability.Variables.Delete("P5");
                _context.ClearContext();
            }

            public override void DoJob() { }
        }
    }
}