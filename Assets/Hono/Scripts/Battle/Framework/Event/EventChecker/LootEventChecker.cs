#region

using System;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class LootEventChecker : EventChecker
    {
        public LootEventChecker(EBattleEventType eventType, Action<IEventInfo> func = null) : base(eventType,
            BattleConstValue.BattleRootControllerUid, func) { }

        protected override bool onCheck(IEventInfo info)
        {
            return true;
        }
    }
}