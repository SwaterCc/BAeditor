using Hono.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Hono.Scripts.Battle.BattleUI.RougePanel {
	public class RougeCard : MonoBehaviour , IView<(int, LootSetting)> {
		public Image Icon;
		public Image BG;
		public TextMeshProUGUI Desc;
		public TextMeshProUGUI Other;
		
		private static Random _random = new();
		private List<ActorLogic.Skill> _randomSkill = new(16);
		public List<Sprite> PerkTypeSprites = new List<Sprite>();
		public List<Sprite> PerkTypeBG = new List<Sprite>();

		private Action _finalAction;
		
		public void OnFresh((int, LootSetting) data) {
			
			var uid = data.Item1;
			var setting = data.Item2;
			Desc.SetText("");
			Other.SetText("");
			Desc.color = Color.white;
			Other.color = Color.white;
			
			if (!ActorManager.Instance.TryGetActor(uid, out var rougeActor)) {
				return;
			}
			
			
			switch (setting.LootFunctionType) {
				case ELootFunctionType.SkillLevelUp:
					showSkillLevelUp(rougeActor, setting);
					break;
				case ELootFunctionType.SkillLearn:
					showSkillLearn(rougeActor, setting);
					break;
				case ELootFunctionType.AttrChange:
					showAttrAdd(rougeActor, setting);
					break;
				case ELootFunctionType.BuffAdd:
					showBuff(rougeActor, setting);
					break;
			}
		}

		private void showSkillLevelUp(Actor actor, LootSetting setting) {
			var skillComp = actor.Logic.GetComponent<ActorLogic.SkillComp>();

			foreach (var pSkill in skillComp.Skills) {
				if (pSkill.Value.SkillData.SkillType == ESkillType.RogueSkill) {
					_randomSkill.Add(pSkill.Value);
				}
			}

			if (_randomSkill.Count == 0) {
				Icon.sprite = PerkTypeSprites[0];
				BG.sprite = PerkTypeBG[0];
				Desc.SetText("未学会Rouge技能");
				Other.SetText("--");
				return;
			}
			
			int idx = _random.Next(0, _randomSkill.Count);
			var finalSkill = _randomSkill[idx];
			_finalAction += () => finalSkill.Level += setting.SkillLevelNum;

			//UI
			Icon.sprite = PerkTypeSprites[0];
			BG.sprite = PerkTypeBG[0];
			Desc.SetText($"{finalSkill.SkillData.SkillName} 技能等级 +{setting.SkillLevelNum}");
			Other.SetText($"当前等级 {finalSkill.Level}");
			if (finalSkill.Level == 10) {
				Other.color = Color.red;
			}
		}

		private void showSkillLearn(Actor actor, LootSetting setting) {
			var skillComp = actor.Logic.GetComponent<ActorLogic.SkillComp>();
			var skillLearnList = BattleManager.CurBattle.LootController.RougeSkillLearnList;
			var idx = _random.Next(0, skillLearnList.Count);
			var skillData = AssetManager.Instance.GetData<SkillData>(skillLearnList[idx]);
			Desc.SetText($"学习技能 {skillData.SkillName}");
			Icon.sprite = PerkTypeSprites[1];
			BG.sprite = PerkTypeBG[1];
			if (!skillComp.Skills.ContainsKey(skillLearnList[idx])) {
				_finalAction = () => skillComp.LearnSkill(skillLearnList[idx], 1);
				Other.SetText("未学会");
			}
			else {
				Other.SetText("已习得");
				Other.color = Color.red;
			}
		}

		private void showAttrAdd(Actor actor, LootSetting setting) {
			
			string desc = "NULL";
			string other = "";
			var total = BattleManager.CurBattle.RtInfo.GetRougePawnAttrChange(actor.Uid, setting.attrTypeType);
			switch (setting.attrTypeType) {
				case EAttrType.AttrAttackAdd:
					desc = $"攻击力增加 {setting.ChangeAttrValue}";
					other = $"累计增加 :{total}";
					break;
				case EAttrType.AttrMaxHpPer:
					desc = $"最大生命值增加 {setting.ChangeAttrValue/100f}";
					other = $"累计增加 :{total / 100f}%";
					break;
				case EAttrType.AttrCritAdd:
					desc = $"暴击率增加 {setting.ChangeAttrValue/100f}%";
					other = $"累计增加 :{total/100f}%";
					break;
				case EAttrType.AttrCritDamageAdd:
					desc = $"暴击伤害增加 {setting.ChangeAttrValue/100f}%";
					other = $"累计增加 :{total/100f}%";
					break;
				case EAttrType.AttrSkillCDPCTAdd:
					desc = $"冷却时间减少 {setting.ChangeAttrValue/100f}%";
					other = $"累计减少 :{total/100f}%";
					break;
				case EAttrType.AttrMoveSpeedPCTAdd:
					desc = $"移动速度增加 {setting.ChangeAttrValue/100f}%";
					other = $"累计增加 :{total/100f}%";
					break;
			}
			Icon.sprite = PerkTypeSprites[2];
			BG.sprite = PerkTypeBG[2];
			Desc.SetText(desc);
			Other.SetText(other);
			
			_finalAction = () => {
				var current = actor.GetAttr(setting.attrTypeType);
				actor.SetAttr(setting.attrTypeType, current + setting.ChangeAttrValue, false);
				BattleManager.CurBattle.RtInfo.RecordPawnRougeAttrChange(actor.Uid, setting.attrTypeType,
					setting.ChangeAttrValue);
			};
		}

		private void showBuff(Actor actor, LootSetting setting) {
			Other.SetText("");
			if (setting.BuffId == 999203) {
				Icon.sprite = PerkTypeSprites[3];
				BG.sprite = PerkTypeBG[3];
				Desc.SetText("生命值回复");
			}
			if (setting.BuffId == 999205) {
				Icon.sprite = PerkTypeSprites[3];
				BG.sprite = PerkTypeBG[3];
				Desc.SetText("能量 +50%");
			}

			_finalAction = () => {
				var buffComp = actor.Logic.GetComponent<ActorLogic.BuffComp>();
				buffComp.AddBuff(actor.Uid, setting.BuffId, setting.BuffLayer);
			};
		}

		public void OnCardSelect() {
			_finalAction?.Invoke();
			RougePanel.Instance.Hide();
			//ActorManager.Instance.TimeScale = 1;
		}
	}
}