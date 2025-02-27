using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle.Core
{
    public class CondKillCountByFaction : LevelConditionMonitor
    {
        private readonly int _faction;
        private int _count;
        private readonly EventListener _listener;

        public CondKillCountByFaction(int faction, int count)
        {
            _faction = faction;
            _count = count;
            _listener = new EventListener(EEventType.OnDead, OnUnitDead);
        }

        public override void OnMonitorExecute()
        {
            EventManager.Instance.AddWorldListener(_listener);
        }

        private void OnUnitDead(VariableBoard board)
        {
            var uid = board.GetEvtField(DeadEventInfoKey.DeadUnitUid);
            var unit = World.Query.GetUnit(uid);
            if (unit.GetAttr(EAttrType.AttrFaction) == _faction)
            {
                if (--_count <= 0)
                {
                    SetPass();
                }
            }
        }

        public override void OnPass()
        {
            EventManager.Instance.RemoveWorldListener(_listener);
        }
    }
}