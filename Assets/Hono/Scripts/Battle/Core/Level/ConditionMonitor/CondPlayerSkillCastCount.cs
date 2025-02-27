using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 玩家释放某技能次数
    /// </summary>
    public class CondPlayerSkillCastCount : LevelConditionMonitor
    {
        private int _skillId;
        private int _count;
        private readonly EventListener _listener;

        public CondPlayerSkillCastCount(int skillId, int count)
        {
            _skillId = skillId;
            _count = count;
            _listener = new EventListener(EEventType.OnSkillUsed,OnSkillUsed);
        }

        private void OnSkillUsed(VariableBoard board)
        {
            var castUid = board.GetEvtField(SkillEventInfo.CastUnitUid);
            if (castUid == World.Current.PlayerControlUnit.Uid)
            {
                if (--_count <= 0)
                {
                    SetPass();
                }
            }
        }

        public override void OnMonitorExecute()
        {
           EventManager.Instance.AddWorldListener(_listener);
        }

        public override void OnPass()
        {
            EventManager.Instance.RemoveWorldListener(_listener);
        }
    }
}