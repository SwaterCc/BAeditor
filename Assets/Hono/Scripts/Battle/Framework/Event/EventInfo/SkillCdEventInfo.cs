#region

using System;

#endregion

namespace Hono.Scripts.Battle.Event {
	public class SkillCdEventInfo : IEventInfo {
		public int ActorUid;
		public int SkillId;
		public Action<Action<float>> AddCdTickFunc;
		public void Clear() {
			ActorUid = 0;
			SkillId = 0;
			AddCdTickFunc = null;
		}
	}
}