using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle.Core
{
    public class CondKillCountByName : LevelConditionMonitor
    {
        private readonly string _dynamicName;
        private int _count;
        private readonly EventListener _listener;

        public CondKillCountByName(string dynamicName, int count)
        {
            _dynamicName = dynamicName;
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
            if (unit.DynamicName == _dynamicName)
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