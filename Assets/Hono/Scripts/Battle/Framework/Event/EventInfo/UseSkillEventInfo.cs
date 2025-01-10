namespace Hono.Scripts.Battle.Event
{
    public static class UsedSkillEventInfo
    {
        /// <summary>
        ///     技能ID
        /// </summary>
        public static readonly EvtInfoField<int> SkillId = new();

        /// <summary>
        ///     施法者Uid
        /// </summary>
        public static readonly EvtInfoField<int> UserUid = new();

        /// <summary>
        ///     玩家手动释放
        /// </summary>
        public static readonly EvtInfoField<bool> IsPlayerControl = new();
    }
}