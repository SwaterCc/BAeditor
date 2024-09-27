using Hono.Scripts.Battle.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class Skill {
			public int Id => Data.ID;
			public SkillData Data;
			public bool IsEnable => (!_isDisable) && (_curCdPercent <= 0 && _resEnough);

			private int _level;
			private float _curCdPercent;
			private int _abilityUid;
			private bool _isDisable;
			private bool _resEnough;
			private float _maxCd;
			private ActorLogic _logic;
			private FilterSetting _skillTargetSetting;

			public Skill(ActorLogic logic, int skillId, int level) {
				_level = level;
				_curCdPercent = 0;
				_logic = logic;
				Data = AssetManager.Instance.GetData<SkillData>(skillId);
				_isDisable = false;

				if (Data.SkillTargetType != ESkillTargetType.Self) {
					if (Data.UseCustomFilter) {
						_skillTargetSetting = Data.CustomFilter;
					}
					else {
						_skillTargetSetting = new FilterSetting();
						var boxData = new CheckBoxData();
						boxData.ShapeType = ECheckBoxShapeType.Sphere;
						boxData.Radius = Data.SkillRange;
						_skillTargetSetting.BoxData = boxData;
						_skillTargetSetting.OpenBoxCheck = true;
						var range = new FilterRange() { RangeType = EFilterRangeType.Faction, };
						switch (Data.SkillTargetType) {
							case ESkillTargetType.Enemy:
								range.Value = (int)EFactionType.Enemy;
								break;
							case ESkillTargetType.Friendly:
								range.Value = (int)EFactionType.Friendly;
								break;
						}

						_skillTargetSetting.Ranges.Add(range);
					}
				}

				var ability = _logic._abilityController.CreateAbility(Data.SkillId);
				_abilityUid = ability.Uid;

				if (Data.CostType == EResCostType.AfterExecute) {
					ability.GetCycleCallback(EAbilityAllowEditCycle.OnPreExecute).OnEnter += resourceCost;
				}
				else {
					ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnEnter += resourceCost;
				}

				if (Data.EcdMode == ECDMode.AfterExecute) {
					ability.GetCycleCallback(EAbilityAllowEditCycle.OnPreExecute).OnEnter += CdBegin;
				}
				else {
					ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnEnter += CdBegin;
				}

				ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnExit += onSkillEnd;

				_logic._abilityController.AwardAbility(ability, false);

				resourceCheck();
			}

			private void onSkillEnd() {
				_logic._stateMachine.ChangeState(EActorState.Idle);
				BattleEventManager.Instance.TriggerEvent(_logic.Uid, EBattleEventType.OnSkillStop,
					new UsedSkillEventInfo() { SkillId = _abilityUid, CasterUid = _logic.Uid });
			}

			/// <summary>
			/// cd开始
			/// </summary>
			public void CdBegin() {
				Debug.Log("CDBegin");
				calculateCd();
				_curCdPercent = 1;
			}

			public void SendFailedMsg() {
				if (_isDisable) {
					Debug.Log("被禁止使用");
				}

				if (!_resEnough) {
					Debug.Log("能量不足");
				}

				if (_curCdPercent <= 1) {
					Debug.Log("Cd中！");
				}
			}

			/// <summary>
			/// 减少cd
			/// </summary>
			public void LessCd(float second) {
				var percent = second / _maxCd;

				_curCdPercent -= percent;
				_curCdPercent = Math.Clamp(_curCdPercent, 0, 1);
				Debug.Log($"LessCD {second} percent {percent} curCDPer {_curCdPercent}");
			}

			private void calculateCd() {
				_maxCd = Data.SkillCD;
			}

			private void resourceCheck() {
				if (Data.SkillResCheck.Count == 0) {
					_resEnough = true;
					return;
				}

				foreach (var resItems in Data.SkillResCheck) {
					foreach (var resItem in resItems.Items) {
						switch (resItem.ResourceType) {
							case EBattleResourceType.Energy:
								var mp = _logic.GetAttr<int>((ELogicAttr)resItem.ResId);
								_resEnough = mp > resItem.Value;
								break;
							case EBattleResourceType.Item:
								break;
							case EBattleResourceType.Buff:
								_resEnough = _logic.GetComponent<BuffComp>().GetBuffLayer(resItem.ResId) >
								             resItem.Value;
								break;
						}
					}
				}
			}

			private void resourceCost() {
				foreach (var resItems in Data.SkillResCost) {
					foreach (var resItem in resItems.Items) {
						switch (resItem.ResourceType) {
							case EBattleResourceType.Energy:
								var mp = _logic.GetAttr<int>((ELogicAttr)resItem.ResId);
								_logic.SetAttr((ELogicAttr)resItem.ResId, mp - resItem.Value, false);
								break;
						}
					}
				}
			}

			public void OnTick(float dt) {
				//CD
				if (_curCdPercent > 0) {
					_curCdPercent -= ((dt / _maxCd) * (_logic.GetAttr<int>(ELogicAttr.AttrSkillCDPCT) / 10000f + 1));
				}

				if (!_resEnough) {
					resourceCheck();
				}
			}

			public void OnSkillUsed() {
				var targetUids = _logic.GetAttr<List<int>>(ELogicAttr.AttrAttackTargetUids);
				targetUids ??= new List<int>();
				targetUids.Clear();

				//选敌
				if (Data.SkillTargetType != ESkillTargetType.Self) {
					var targetList = ActorManager.Instance.UseFilter(_logic.Actor, _skillTargetSetting);
					float minDistance = 9999;
					var selfPos = _logic.GetAttr<Vector3>(ELogicAttr.AttrPosition);

					if (Data.MaxTargetCount != 1) {
						if (targetList.Count > Data.MaxTargetCount) {
							targetUids.AddRange(targetList.GetRange(0, Data.MaxTargetCount));
						}
						else {
							targetUids.AddRange(targetList);
						}
					}
					else {
						int targetUid = -1;
						foreach (var uid in targetList) {
							var target = ActorManager.Instance.GetActor(uid);
							var targetPos = target.GetAttr<Vector3>(ELogicAttr.AttrPosition);
							var newMinDis = Vector3.Distance(selfPos, targetPos);
							if (newMinDis < minDistance) {
								minDistance = newMinDis;
								targetUid = uid;
							}
						}

						if (targetUid > 0) {
							targetUids.Add(targetUid);
						}
					}
				}
				else {
					targetUids.Add(_logic.Uid);
				}

				if (targetUids.Count > 0) {
					_logic.SetAttr(ELogicAttr.AttrAttackTargetUids, targetUids, false);
					_logic._abilityController.ExecutingAbility(_abilityUid);
					BattleEventManager.Instance.TriggerEvent(_logic.Uid, EBattleEventType.OnSkillUseSuccess,
						new UsedSkillEventInfo() { SkillId = _abilityUid, CasterUid = _logic.Uid });
					resourceCheck();
				}
				else {
					Debug.Log("技能没有找到目标！");
				}
			}

			public static bool operator <(Skill control1, Skill control2) {
				return control1.Data.PriorityATK > control2.Data.PriorityDEF;
			}

			public static bool operator >(Skill control1, Skill control2) {
				return control1.Data.PriorityDEF > control2.Data.PriorityATK;
			}

			public void Destroy() {
				_logic._abilityController.RemoveAbility(_abilityUid);
			}
		}
	}
}