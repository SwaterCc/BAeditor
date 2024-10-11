using System;

namespace Hono.Scripts.Battle.Event {
	public class UseSkillChecker : EventChecker {

		private int _checkSkillId;

		public UseSkillChecker(EBattleEventType eventType, Actor actor, int checkSkillId, Action<IEventInfo> func) : base(
			eventType,
			actor, func) {
			_checkSkillId = checkSkillId;
		}


		protected override bool onCheck(IEventInfo info) {
			if (_checkSkillId > 0) {
				return _checkSkillId == ((UsedSkillEventInfo)info).SkillId;
			}
			return true;
		}
	}
}