#region

using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Message;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class AListenerNode : ANode<ListenerNodeData>, IGPoolObject
        {
            private readonly UnitEventListener _eventListener = new();
            private readonly MessageListener _messageListener = new();

            public VariableBoard Board { get; private set; }

            public void RegisterEvent()
            {
                if (Data.isEvent)
                {
                    _eventListener.BindEvent(Data.eventType);
                    _eventListener.SetInterval(Data.eventInterval);
                    _eventListener.SetCallback(OnFire);
                    _eventListener.SetupChecker(ParseRef<IEventChecker>(Data.getCheckerFunc));
                    _eventListener.IsWorldListener = Data.isGlobalEvtListener;
                    AContext.Unit.RegisterEvtListener(_eventListener);
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
                AContext.Unit.UnregisterEvtListener(_eventListener);
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
                GPool<AListenerNode>.Pool.Recycle(this);
            }
        }
    }
}