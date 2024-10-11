namespace Hono.Scripts.Battle.Event {
	public class UsedSkillEventInfo : IEventInfo
	{
		/// <summary>
		/// 技能ID
		/// </summary>
		public int SkillId;

		/// <summary>
		/// 施法者Uid
		/// </summary>
		public int CasterUid;
	}
}