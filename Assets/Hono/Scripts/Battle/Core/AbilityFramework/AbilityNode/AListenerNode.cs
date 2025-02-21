#region

using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class AListenerNode : ANode<ListenerNodeData>, IGPoolObject, ILocalVariableBoardHandle
        {
            private readonly EventListener _eventListener = new();
            private readonly MessageListener _messageListener = new();
            public VariableBoard LocalVariableBoard { get; private set; }

            public void RegisterEvent()
            {
                if (Data.isEvent)
                {
                    _eventListener.BindEvent(Data.eventType);
                    _eventListener.SetInterval(Data.eventInterval);
                    _eventListener.SetCallback(OnFire);
                    _eventListener.SetupChecker(ParseRef<IEventChecker>(Data.getCheckerFunc));
                    if (Data.isGlobalEvtListener) {
	                    AContext.Unit.AddWorldEvtListener(_eventListener);
                    }
                    else {
	                    AContext.Unit.AddUnitEvtListener(_eventListener);
                    }
                }
                else
                {
                    _messageListener.Bind(Data.msgName, OnFire);
                    AContext.Unit.RegisterMsgListener(_messageListener);
                }
            }

            public void UnRegisterEvent()
            {
                AContext.Unit.UnregisterMsgListener(_messageListener);
                AContext.Unit.RemoveUnitEvtListener(_eventListener);
            }

            private void OnFire(VariableBoard board)
            {
                LocalVariableBoard = board;
                DoChildrenJob();
                LocalVariableBoard = null;
            }

            public override void DoJob() { }

            public override void Recycle()
            {
                GPool<AListenerNode>.Pool.Recycle(this);
            }
        }
    }
}