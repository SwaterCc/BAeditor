#region

using System;
using Hono.Scripts.Battle.Base;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class SkillEventChecker : IEventChecker
    {
        private int _castUid;
        private int _skillId;

        public SkillEventChecker() { }

        public SkillEventChecker(int castUid, int skillId)
        {
            _castUid = castUid;
            _skillId = skillId;
        }

        public void OnRent(int castUid, int skillId)
        {
            _castUid = castUid;
            _skillId = skillId;
        }

        public bool Check(in VariableBoard board)
        {
            bool res = true;

            if (_castUid > 0)
            {
                res = res && board.GetEvtField(SkillEventInfo.CastUnitUid) == _castUid;
            } 

            if (_skillId > 0)
            {
                res = res && board.GetEvtField(SkillEventInfo.SkillId) == _skillId;
            }
            
            return res;
        }

        public void OnRecycle()
        {
            _skillId = 0;
            _castUid = 0;
        }
    }
}