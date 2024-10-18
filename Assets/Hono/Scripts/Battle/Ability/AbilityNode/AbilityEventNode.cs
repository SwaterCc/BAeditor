
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
                    if (!_eventNodeData.CreateChecker.Parse(_executor.Ability,out _checker))
                    {
                        Debug.LogError("Event执行失败");
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
                    _checker?.UnRegister();
                }
                else
                {
                    _executor.Ability.Actor.RemoveMsgListener(_messageListener);
                }
            }

            private void onEventFired(IEventInfo eventInfo)
            {
	            //Debug.Log($"[Ability] ActorUid {_executor.Ability.Actor.Uid} AbilityId:{_executor.AbilityData.ConfigId} EventNodeFired nodeId {NodeId}");
	            eventInfo.SetFieldsInAbilityVariables(_executor.Ability);
	            DoChildrenJob();
                eventInfo.ClearFields(_executor.Ability);
            }

            private void onMsgCall(object p1,object p2,object p3,object p4,object p5)
            {
                _executor.Ability.Variables.Set("Msg:P1",p1);
                _executor.Ability.Variables.Set("Msg:P2",p2);
                _executor.Ability.Variables.Set("Msg:P3",p3);
                _executor.Ability.Variables.Set("Msg:P4",p4);
                _executor.Ability.Variables.Set("Msg:P5",p5);
                DoChildrenJob();
                _executor.Ability.Variables.Delete("Msg:P1");
                _executor.Ability.Variables.Delete("Msg:P2");
                _executor.Ability.Variables.Delete("Msg:P3");
                _executor.Ability.Variables.Delete("Msg:P4");
                _executor.Ability.Variables.Delete("Msg:P5");
            }

            public override void DoJob() { }
        }
    }
}