#region

using System;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class ActorStateChecker : EventChecker
    {
        public ActorStateChecker(EEventType eventType, Actor actor, Action<IEventInfo> func = null) : base(
            eventType, actor, func) { }

        public ActorStateChecker(EEventType eventType, int actorUid, Action<IEventInfo> func = null) : base(
            eventType, actorUid, func) { }

        protected override bool onCheck(IEventInfo info)
        {
            return true;
        }
    }
}