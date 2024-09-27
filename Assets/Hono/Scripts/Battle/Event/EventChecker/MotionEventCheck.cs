using System;

namespace Hono.Scripts.Battle.Event {
	public class MotionEventChecker : EventChecker {
		private int _motionId;

		public MotionEventChecker(EBattleEventType eventType, Actor actor, int motionId, Action<IEventInfo> func) :
			base(eventType, actor, func) {
			_motionId = motionId;
		}

		protected override bool onCheck(IEventInfo info) {
			var motionInfo = (MotionEventInfo)info;
			return _motionId == motionInfo.MotionId;
		}
	}
}