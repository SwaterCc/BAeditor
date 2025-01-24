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
            private readonly ActorEventListener _eventListener = new();
            private readonly MessageListener _messageListener = new();

            public VariableBoard Board { get; private set; }

            public void RegisterEvent()
            {
                if (Data.isEvent)
                {
                    _eventListener.BindEvent(Data.eventType);
                    _eventListener.SetInterval(Data.eventInterval);
                    _eventListener.SetCallback(OnFire);
                    _eventListener.SetupChecker(Data.Checker);
                    _eventListener.IsGlobalListener = Data.isGlobalEvtListener;
                    AContext.Actor.RegisterEvtListener(_eventListener);
                }
                else
                {
                    _messageListener.Bind(Data.msgName, OnFire);
                    AContext.Actor.RegisterMsgListener(_messageListener);
                }
            }

            public void UnRegisterEvent()
            {
                AContext.Actor.UnregisterMsgListener(_messageListener);
                AContext.Actor.UnregisterEvtListener(_eventListener);
            }

            private void OnFire(VariableBoard board)
            {
                Board = board;
                DoChildrenJob();
                Board = null;
            }

            public override void DoJob() { }

            public override void Recycle()
            {
                APool<AListenerNode>.Pool.Recycle(this);
            }
        }
    }
}