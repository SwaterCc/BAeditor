#region

using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AEventNode : ANode<EventNodeData>, IAPoolObject
        {
            private EventChecker _checker;
            private MessageListener _messageListener = new();

            /// <summary>
            /// TODO:还有GC
            /// </summary>
            public void RegisterEvent()
            {
                if (Data.IsEvent)
                {
                    if (!Data.CreateChecker.TryParse(AContext, out _checker))
                    {
                        Debug.LogError("Event执行失败");
                        return;
                    }

                    _checker.BindFunc(onEventFired);
                    BattleEventManager.Instance.Register(_checker);
                }
                else
                {
                    _messageListener.Bind(Data.MsgName, onMsgCall);
                    AContext.Actor.AddMsgListener(_messageListener);
                }
            }

            public void UnRegisterEvent()
            {
                if (Data.IsEvent)
                {
                    _checker?.UnRegister();
                }
                else
                {
                    AContext.Actor.RemoveMsgListener(_messageListener);
                }
            }

            private void onEventFired(IEventInfo eventInfo)
            {
                eventInfo.SetFieldsInAbilityVariables(AContext);
                DoChildrenJob();
                eventInfo.ClearFields(AContext);
            }

            private void onMsgCall(object p1, object p2, object p3, object p4, object p5)
            {
                AContext.Vairables.Set("Msg:P1", p1);
                AContext.Vairables.Set("Msg:P2", p2);
                AContext.Vairables.Set("Msg:P3", p3);
                AContext.Vairables.Set("Msg:P4", p4);
                AContext.Vairables.Set("Msg:P5", p5);
                DoChildrenJob();
                AContext.Vairables.Delete("Msg:P1");
                AContext.Vairables.Delete("Msg:P2");
                AContext.Vairables.Delete("Msg:P3");
                AContext.Vairables.Delete("Msg:P4");
                AContext.Vairables.Delete("Msg:P5");
            }

            public override void DoJob() { }

            public override void Recycle()
            {
                AObjectPool<AEventNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                _messageListener.Reset();
                ((ANode)this).OnRecycle();
            }
        }
    }
}