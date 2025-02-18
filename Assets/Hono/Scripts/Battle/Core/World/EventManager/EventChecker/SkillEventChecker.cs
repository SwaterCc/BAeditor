#region

using System;
using Hono.Scripts.Battle.Base;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class SkillEventChecker : IEventChecker
    {
        private int _skillId;

        public SkillEventChecker() { }

        public SkillEventChecker(int skillId)
        {
            _skillId = skillId;
        }

        public void OnRent(int skillId)
        {
            _skillId = skillId;
        }

        public bool Check(in VariableBoard board)
        {
            return board.GetEvtField(SkillEventInfo.SkillId) == _skillId;
        }

        public void OnRecycle()
        {
            _skillId = 0;
        }
    }
}