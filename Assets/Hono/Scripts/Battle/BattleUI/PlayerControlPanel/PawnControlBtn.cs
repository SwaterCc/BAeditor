#region

using Hono.Core;
using Hono.Scripts.Battle.Event;
using QTool.Tween;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class PawnControlBtn : MonoBehaviour, IView<PawnTeamMemberState> {
		private PawnTeamMemberState _memberState;
		private int _ultSkillId;
		private UsedSkillEventInfo _skillEventInfo = new();

		public Slider HpBar;
		public Image DeadImage;
		public Image SkillIcon;
		public Image LeaderIcon;
		public Image CD;
		public QTweenLoop FullMpEffect;

		private SkillCDChecker _skillCdChecker;

		public void OnFresh(PawnTeamMemberState data) {
			_memberState = data;
			CD.fillAmount = 0;
			if (!ActorManager.Instance.TryGetActor(data.ActorUid, out var actor)) {
				Debug.LogError("玩家控制按钮初始化失败");
				return;
			}

			var pawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(actor.ConfigId);

			foreach (var pSkill in pawnLogicRow.OwnerSkills) {
				var skillData = AssetManager.Instance.GetData<SkillData>(pSkill[0]);
				if (skillData.SkillType == ESkillType.UltimateSkill) {
					_ultSkillId = skillData.ID;
					if (!string.IsNullOrEmpty(skillData.SkillIconPath)) {
						var sprite = Resources.Load<Sprite>(skillData.SkillIconPath);
						if (sprite) {
							SkillIcon.sprite = sprite;
						}
					}

					break;
				}
			}


			if (!string.IsNullOrEmpty(pawnLogicRow.RPGIcon)) {
				var sprite = Resources.Load<Sprite>(pawnLogicRow.RPGIcon);
				if (sprite != null) {
					LeaderIcon.sprite = sprite;
				}
			}

			if (_ultSkillId <= 0) {
				Debug.LogError("未找到大招");
				return;
			}

			_skillEventInfo.SkillId = _ultSkillId;
			_skillEventInfo.UserUid = actor.Uid;
			_skillEventInfo.IsPlayerControl = true;
			_skillCdChecker = new SkillCDChecker(EBattleEventType.SkillCDBegin, actor.Uid, _ultSkillId, OnCdBegin);
			BattleEventManager.Instance.Register(_skillCdChecker);
		}

		public void Update() {
			if (_memberState == null || _ultSkillId <= 0) return;

			DeadImage.gameObject.SetActive(false);
			if (_memberState.CurStateType == EPawnTeamMemberStateType.Dead) {
				DeadImage.gameObject.SetActive(true);
				FullMpEffect.gameObject.SetActive(false);
				return;
			}

			if (!ActorManager.Instance.TryGetActor(_memberState.ActorUid, out var actor)) {
				return;
			}

			var curHp = actor.GetAttr(EAttrType.AttrHp);
			var maxHp = actor.GetAttr(EAttrType.AttrMaxHp);

			HpBar.value = curHp / (float)maxHp;

			var curMp = actor.GetAttr(EAttrType.AttrMp);
			var maxMp = actor.GetAttr(EAttrType.AttrMaxMp);

			FullMpEffect.gameObject.SetActive(curMp >= maxMp);
		}

		private void OnCdBegin(IEventInfo info) {
			var cdInfo = (SkillCdEventInfo)info;
			cdInfo.AddCdTickFunc?.Invoke(cdTick);
			CD.fillAmount = 1;
		}

		private void cdTick(float cdPer) {
			CD.fillAmount = cdPer;
		}

		public void OnUseSkill() {
			if (_memberState.CurStateType == EPawnTeamMemberStateType.Normal) {
				BattleEventManager.Instance.TriggerActorEvent(_memberState.ActorUid, EBattleEventType.UseSkill,
					_skillEventInfo);
			}
		}

		private void OnDestroy() {
			BattleEventManager.Instance.UnRegister(_skillCdChecker);
		}
	}
}