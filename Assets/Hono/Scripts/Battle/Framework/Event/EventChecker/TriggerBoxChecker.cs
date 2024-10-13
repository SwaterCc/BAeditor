using System;

namespace Hono.Scripts.Battle.Event
{
    public class TriggerBoxChecker : EventChecker
    {
        public TriggerBoxChecker(EBattleEventType eventType, Actor actor, Action<IEventInfo> func = null) : base(eventType, actor, func) { }
       
        protected override bool onCheck(IEventInfo info)
        {
            return true;
        }
    }
}