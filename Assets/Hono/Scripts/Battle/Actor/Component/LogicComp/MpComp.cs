using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class MpComp : ALogicComponent {
			private readonly UseSkillChecker _useSkillChecker;
			private readonly HitEventChecker _hitEventChecker;
			private readonly HitEventChecker _beHitEventChecker;

			private const int MpRecCastBase = 180;
			private const int MpRecBehit = 1000;
			private const int MpRecKilled = 400;

			public MpComp(ActorLogic logic) : base(logic) {
				_useSkillChecker =
					new UseSkillChecker(EBattleEventType.OnSkillUseSuccess, Actor, -1, AttackChangeMp);
				_hitEventChecker = new HitEventChecker(EBattleEventType.OnHitDamage, Actor, -1, -1, KillChangeMp);
				_beHitEventChecker = new HitEventChecker(EBattleEventType.OnBeHit, Actor, -1, -1, BeHitChangeMp);
			}

			public override void Init() {
				BattleEventManager.Instance.Register(_useSkillChecker);
				BattleEventManager.Instance.Register(_beHitEventChecker);
				BattleEventManager.Instance.Register(_hitEventChecker);
			}


			private void AttackChangeMp(IEventInfo info) {
				var useSkillInfo = (UsedSkillEventInfo)info;
				var skillData = AssetManager.Instance.GetData<SkillData>(useSkillInfo.SkillId);
				if (skillData == null) return;
				if (skillData.SkillType == ESkillType.UltimateSkill) return;

				var maxMp = Actor.GetAttr<int>(ELogicAttr.AttrMaxMp);
				var curMp = Actor.GetAttr<int>(ELogicAttr.AttrMp);
				var mpRecCastAdd = Actor.GetAttr<int>(ELogicAttr.AttrMpRecCastAdd);
				var mpRecCastPer = Actor.GetAttr<int>(ELogicAttr.AttrMpRecCastPer);
				var allRecAdd = Actor.GetAttr<int>(ELogicAttr.AttrMpRecAllAdd);
				var value = ((MpRecCastBase + mpRecCastAdd) * (10000 + mpRecCastPer) / 10000) *
				            ((10000 + allRecAdd) / 10000);
				curMp = Mathf.Clamp(curMp + value, 0, maxMp);
				Actor.SetAttr(ELogicAttr.AttrMp, curMp, false);
			}

			private void BeHitChangeMp(IEventInfo info) {
				var hitDamageInfo = (HitDamageInfo)info;
				var maxMp = Actor.GetAttr<int>(ELogicAttr.AttrMaxMp);
				var curMp = Actor.GetAttr<int>(ELogicAttr.AttrMp);
				var maxHp = Actor.GetAttr<int>(ELogicAttr.AttrMaxHp);
				
				var mpRecBehitAdd = Actor.GetAttr<int>(ELogicAttr.AttrMpRecBehitAdd);
				var mpRecBehitPer = Actor.GetAttr<int>(ELogicAttr.AttrMpRecBehitPer);
				var allRecAdd = Actor.GetAttr<int>(ELogicAttr.AttrMpRecAllAdd);
				var baseValue = (int)(hitDamageInfo.FinalDamageValue / maxHp) * MpRecBehit;
				var value = ((baseValue + mpRecBehitAdd) * (10000 + mpRecBehitPer) / 10000) *
				            ((10000 + allRecAdd) / 10000);
				curMp = Mathf.Clamp(curMp + value, 0, maxMp);
				Actor.SetAttr(ELogicAttr.AttrMp, curMp, false);
			}

			public void KillChangeMp(IEventInfo info) {
				var hitDamageInfo = (HitDamageInfo)info;
				if (!hitDamageInfo.IsKillTarget) return;

				var maxMp = Actor.GetAttr<int>(ELogicAttr.AttrMaxMp);
				var curMp = Actor.GetAttr<int>(ELogicAttr.AttrMp);
				var mpRecBehitAdd = Actor.GetAttr<int>(ELogicAttr.AttrMpRecKilledAdd);
				var mpRecBehitPer = Actor.GetAttr<int>(ELogicAttr.AttrMpRecKilledPer);
				var allRecAdd = Actor.GetAttr<int>(ELogicAttr.AttrMpRecAllAdd);
				var value = ((MpRecKilled + mpRecBehitAdd) * (10000 + mpRecBehitPer) / 10000) *
				            ((10000 + allRecAdd) / 10000);
				curMp = Mathf.Clamp(curMp + value, 0, maxMp);
				Actor.SetAttr(ELogicAttr.AttrMp, curMp, false);
			}
		}
	}
}