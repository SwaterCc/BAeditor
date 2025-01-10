#region

using System;
using Hono.Scripts.Battle.Base;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public static class SkillEventInfo
    {
        public const string SkillId = "SkillId";
    }
    
    [Serializable]
    public class SkillEventChecker : IEventChecker
    {
        public int skillId;
        
        public bool Check(in VariableBoard board)
        {
            return board.Get<int>(SkillEventInfo.SkillId) == skillId;
        }
    }
}