using System;

namespace Hono.Scripts.Battle.Event {
	public class HitEventChecker : EventChecker {
		/// <summary>
		/// 来源abilityUid
		/// </summary>
		private int _abilitySourceUid;

		/// <summary>
		/// 来源的伤害Id
		/// </summary>
		private int _damageConfigId;

		public HitEventChecker(EBattleEventType eventType, Actor bindActor, int abilitySourceUid, int damageConfigId,
			Action<IEventInfo> func = null) :
			base(eventType, bindActor, func) {
			_damageConfigId = damageConfigId;
			_abilitySourceUid = abilitySourceUid;
		}

		protected override bool onCheck(IEventInfo info) {
			var hitInfo = (HitInfo)info;
			bool res = true;

			if (_abilitySourceUid > 0) {
				res = hitInfo.SourceAbilityUId == _abilitySourceUid;
			}
			
			if (_damageConfigId > 0) {
				res = res && hitInfo.DamageConfigId == _damageConfigId;
			}

			return res;
		}
	}
}