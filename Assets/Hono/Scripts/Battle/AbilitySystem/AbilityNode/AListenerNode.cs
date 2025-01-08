#region

using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private class AListenerNode : ANode<ListenerNodeData>, IAPoolObject
        {
            private EventChecker _checker;
            private readonly MessageListener _messageListener = new();

            /// <summary>
            /// TODO:还有GC
            /// </summary>
            public void RegisterEvent()
            {
                if (Data.IsEvent)
                {
                    _checker = ParseRef<EventChecker>(Data.GetChecker);
                    _checker.BindFunc(onEventFired);
                    EventManager.Instance.Register(_checker);
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
                DoChildrenJob();
            }

            private void onMsgCall(object p1, object p2, object p3, object p4, object p5)
            {
                AContext.VariableBoard.Set("Msg:P1", p1);
                AContext.VariableBoard.Set("Msg:P2", p2);
                AContext.VariableBoard.Set("Msg:P3", p3);
                AContext.VariableBoard.Set("Msg:P4", p4);
                AContext.VariableBoard.Set("Msg:P5", p5);
                DoChildrenJob();
                AContext.VariableBoard.Delete("Msg:P1");
                AContext.VariableBoard.Delete("Msg:P2");
                AContext.VariableBoard.Delete("Msg:P3");
                AContext.VariableBoard.Delete("Msg:P4");
                AContext.VariableBoard.Delete("Msg:P5");
            }

            public override void DoJob() { }

            public override void Recycle()
            {
                APool<AListenerNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                _messageListener.Reset();
                ((ANode)this).OnRecycle();
            }
        }
    }
}