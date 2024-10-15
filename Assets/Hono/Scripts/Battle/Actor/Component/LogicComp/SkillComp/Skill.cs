using Hono.Scripts.Battle.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class Skill {
			public int Id => Data.ID;
			public SkillData Data;
			public bool IsEnable => (!_isDisable) && (_curCdPercent <= 0 && _resEnough) && (!_isExecuting);

			public bool IsExecuting => _isExecuting;
			
			private int _level;
			private float _curCdPercent;
			private int _abilityUid;
			private bool _isDisable;
			private bool _resEnough;
			private bool _isExecuting;
			private float _maxCd;
			private ActorLogic _logic;
			private FilterSetting _skillTargetSetting;

			public Skill(ActorLogic logic, int skillId, int level) {
				_level = level;
				_curCdPercent = 0;
				_logic = logic;
				Data = AssetManager.Instance.GetData<SkillData>(skillId);
				_isDisable = false;
				_isExecuting = false;
				_skillTargetSetting = Data.CustomFilter;

				var ability = _logic._abilityController.CreateAbility(Data.SkillId);
				_abilityUid = ability.Uid;

				ability.GetCycleCallback(EAbilityAllowEditCycle.OnPreExecute).OnEnter += onAbilityBegin;
				ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnExit += onAbilityEnd;
				
				_logic._abilityController.AwardAbility(ability, Data.SkillType == ESkillType.PassiveSkill);

				resourceCheck();
			}

			private void onAbilityBegin()
			{
				_isExecuting = true;
				_logic._stateMachine.ChangeState(EActorState.Battle);
				if (Data.CostType == EResCostType.BeforeExecute)
				{
					resourceCost();
				}
				if (Data.EcdMode == ECDMode.BeforeExecute) {
					CdBegin();
				}
				
				BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnSkillUseSuccess,
					new UsedSkillEventInfo() { SkillId = _abilityUid, CasterUid = _logic.Uid });
			}
			
			private void onAbilityEnd() {
				_isExecuting = false;
				
				if (Data.CostType == EResCostType.AfterExecute)
				{
					resourceCost();
				}
				if (Data.EcdMode == ECDMode.AfterExecute) {
					CdBegin();
				}
				
				_logic._stateMachine.ChangeState(EActorState.Idle);
				BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnSkillStop,
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

				if (_curCdPercent != 0) {
					Debug.Log("Cd中！");
				}

				if (_isExecuting) {
					Debug.Log("技能释放中！");
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

			public bool TryUseSkill()
			{
				if (!IsEnable) return false;
				
				var targetUids = _logic.GetAttr<List<int>>(ELogicAttr.AttrAttackTargetUids);
				targetUids ??= new List<int>();
				targetUids.Clear();

				//选敌
				if (!Data.SelectSelf) {
					targetUids = ActorManager.Instance.UseFilter(_logic.Actor, _skillTargetSetting);
				}
				else {
					targetUids.Add(_logic.Uid);
				}

				if (targetUids.Count > 0) {
					_logic.SetAttr(ELogicAttr.AttrAttackTargetUids, targetUids, false);
					Debug.Log($"[UseSkill] Actor{_logic.Uid} -->执行了技能 {_abilityUid}");
					_logic._abilityController.ExecutingAbility(_abilityUid);
				}
				else {
					Debug.Log("技能没有找到目标！");
					return false;
				}

				return true;
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