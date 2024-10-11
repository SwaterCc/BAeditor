using System;

namespace Hono.Scripts.Battle.Event {
	public class MotionEventChecker : EventChecker {
		private int _motionUid;

		public MotionEventChecker(EBattleEventType eventType, Actor actor, int motionUid, Action<IEventInfo> func) :
			base(eventType, actor, func) {
			_motionUid = motionUid;
		}

		protected override bool onCheck(IEventInfo info) {
			var motionInfo = (MotionEventInfo)info;

			if (_motionUid <= 0) return true;
			
			return _motionUid == motionInfo.MotionUid;
		}
	}
}