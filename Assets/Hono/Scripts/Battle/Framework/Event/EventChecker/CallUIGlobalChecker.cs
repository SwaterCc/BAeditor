using System;

namespace Hono.Scripts.Battle.Event
{
    public class CallUIGlobalChecker : EventChecker
    {
        public CallUIGlobalChecker(EBattleEventType eventType, Action<IEventInfo> func = null) : base(eventType, -1, func) { }
        protected override bool onCheck(IEventInfo info)
        {
            return true;
        }
    }
}