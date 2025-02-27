using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Unit击杀监听Tags判定
    /// </summary>
    public class CondKillCountByTag : LevelConditionMonitor
    {
        private readonly int _tag;
        private int _count;
        private readonly EventListener _listener;

        public CondKillCountByTag(int tag, int count)
        {
            _tag = tag;
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
            if (unit.Tags.HasTag(_tag))
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