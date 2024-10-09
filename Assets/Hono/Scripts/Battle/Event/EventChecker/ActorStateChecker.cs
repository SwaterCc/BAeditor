using System;

namespace Hono.Scripts.Battle.Event
{
    public class ActorStateChecker : EventChecker
    {
        public ActorStateChecker(EBattleEventType eventType, Actor actor, Action<IEventInfo> func = null) : base(eventType, actor, func) { }
        public ActorStateChecker(EBattleEventType eventType, int actorUid, Action<IEventInfo> func = null) : base(eventType, actorUid, func) { }
        protected override bool onCheck(IEventInfo info)
        {
            return true;
        }
    }
}