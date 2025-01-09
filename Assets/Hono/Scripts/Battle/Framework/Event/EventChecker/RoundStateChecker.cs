#region

using System;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class RoundStateChecker : EventChecker
    {
        public RoundStateChecker(EEventType eventType, Action<IEventInfo> func = null) : base(eventType,
            BattleConstValue.BattleRootControllerUid, func) { }

        protected override bool onCheck(IEventInfo info)
        {
            return true;
        }
    }
}