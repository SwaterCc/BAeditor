#region

using System;
using Hono.Scripts.Battle.Base;

#endregion

namespace Hono.Scripts.Battle.Event
{
    [Serializable]
    public class SkillEventChecker : IEventChecker
    {
        public int skillId;
        
        public bool Check(in VariableBoard board)
        {
            return board.Get(SkillEventInfo.SkillId) == skillId;
        }
    }
}