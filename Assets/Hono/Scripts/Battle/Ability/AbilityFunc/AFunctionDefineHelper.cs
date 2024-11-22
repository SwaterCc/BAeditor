namespace Hono.Scripts.Battle {

	public static class ARunningTime {
		public static Ability AContext { get; private set; }
		public static Actor Actor => AContext.Actor;

		/// <summary>
		/// 更新当前执行的Ability
		/// </summary>
		/// <param name="ability"></param>
		public static void UpdateContext(Ability ability) {
			AContext = ability;
		}

		/// <summary>
		/// 获取Actor，当actorUid小于等于0时返回调用者Actor
		/// </summary>
		/// <param name="actorUid"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool TryGetActor(int actorUid, out Actor source) {
			source = null;
			if (actorUid <= 0) {
				source = AContext.Actor;
			}
			else {
				source = ActorManager.Instance.GetActor(actorUid);
			}

			return source != null;
		}
	}
	
}