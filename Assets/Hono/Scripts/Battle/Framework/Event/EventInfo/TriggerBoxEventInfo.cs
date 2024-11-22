namespace Hono.Scripts.Battle.Event {
	public class TriggerBoxEventInfo : IEventInfo {
		public int TargetUid;
		public void Clear() {
			TargetUid = 0;
		}
	}
}