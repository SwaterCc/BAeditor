using System;

namespace Hono.Scripts.Battle.Event
{
    public class MonsterGenEventChecker : EventChecker
    {
        public MonsterGenEventChecker(EBattleEventType eventType, Actor actor, Action<IEventInfo> func = null) : base(eventType, actor, func) { }
        public MonsterGenEventChecker(EBattleEventType eventType, int actorUid, Action<IEventInfo> func = null) : base(eventType, actorUid, func) { }
        protected override bool onCheck(IEventInfo info)
        {
            var monsterGenInfo = (MonsterGenEventInfo)info;

            if (monsterGenInfo.FiredAll) return true;

            if (monsterGenInfo.SingleUid > 0) return _checkerBelongActorUid == monsterGenInfo.SingleUid;

            if (monsterGenInfo.SpecialUids.Count > 0)
                return monsterGenInfo.SpecialUids.Contains(_checkerBelongActorUid);
            
            return false;
        }
    }
}