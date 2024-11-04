#region

using System;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class SkillCDChecker : EventChecker
    {
        private int _skillId;

        public SkillCDChecker(EBattleEventType eventType, int actorUid, int skillId, Action<IEventInfo> func = null) :
            base(eventType, actorUid, func)
        {
            _skillId = skillId;
        }

        protected override bool onCheck(IEventInfo info)
        {
            var skillCdInfo = (SkillCdEventInfo)info;
            return skillCdInfo.SkillId == _skillId;
        }
    }
}