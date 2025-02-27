namespace Hono.Scripts.Battle.Event
{
    public static class SkillEventInfo
    {
        /// <summary>
        /// 施法者Uid
        /// </summary>
        public static readonly EvtInfoField<int> CastUnitUid = new();
        /// <summary>
        /// 技能ID
        /// </summary>
        public static readonly EvtInfoField<int> SkillId = new();
    }
}