#region

using Hono.Scripts.Battle.BattleUI;
using Hono.Scripts.Battle.Event;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public partial class BattleGround {
		private class RoundRunningState : RoundState {
			private bool _isEnd;
			private const float CheckInterval = 0.5f;
			private float _checkDt;
			private readonly List<RoundScoreCondition> _finalTimeCheck = new(8);
			private readonly List<RoundScoreCondition> _frameCheck = new(8);
			private bool _firstTick;
			public RoundRunningState(RoundController roundController) : base(roundController) { }
			private readonly MonsterGenEventInfo _monsterGenEventInfo = new();
			public override ERoundState GetRoundState() => ERoundState.Running;

			protected override void onEnter() {
				_firstTick = true;
				foreach (var abilityId in CurrentRoundData.RunningAbilityIds) {
					//BattleManager.BattleController.(abilityId);
				}

				foreach (var condition in CurrentRoundData.SuccessConditions) {
					if (condition.ScoreNow || CurrentRoundData.RunningCheckTime <= 0) {
						_frameCheck.Add(condition);
					}
					else {
						_finalTimeCheck.Add(condition);
					}
				}
				MonsterGeneratorLogic.CurMonsterCount = 0;
				PlayerControlPanel.Instance.Show();
			}

			//阶段最终检测时长
			//失败条件-即刻结算
			//成功条件-最终时长到达后检测
			//当不限制时间时，所有成功条件都变成即刻结算条件
			protected override void onTick(float dt) {
				Round.GameRunningState.BattleGroundHandle.RtInfo.CurRoundDurationTime += dt;
				if (_firstTick) {
					firstTick();
					return;
				}

				if (_checkDt > CheckInterval) {
					if (CurrentRoundData.SuccessConditions.Count <= 0) {
						if (Round.GameRunningState.BattleGroundHandle.RtInfo.GetRoundLastMonster() <= 0) {
							Round.SwitchState(ERoundState.SuccessScoring);
						}
					}
					else {
						int successCount = 0;
						foreach (var successCondition in _frameCheck) {
							if (checkCondition(successCondition)) {
								++successCount;
							}
						}

						if (successCount == _frameCheck.Count && successCount != 0) {
							Round.SwitchState(ERoundState.SuccessScoring);
							return;
						}
					}

					if (CurrentRoundData.FailedConditions.Count > 0) {
						foreach (var failedCondition in CurrentRoundData.FailedConditions) {
							if (checkCondition(failedCondition)) {
								Round.SwitchState(ERoundState.FailedScoring);
								return;
							}
						}
					}
					else {
						if (!Round.GameRunningState.BattleGroundHandle._pawnTeamController.CheckHasTeamAlive()) {
							Round.SwitchState(ERoundState.FailedScoring);
						}
					}

					_checkDt = 0;
				}

				_checkDt += dt;

				if (CurrentRoundData.RunningCheckTime <= 0 || CurrentRoundData.SuccessConditions.Count == 0) return;

				if (Duration <= CurrentRoundData.RunningCheckTime) return;
				foreach (var condition in _finalTimeCheck) {
					if (checkCondition(condition)) {
						continue;
					}

					Round.SwitchState(ERoundState.FailedScoring);
					return;
				}

				Round.SwitchState(ERoundState.SuccessScoring);
			}

			private void firstTick() {
				foreach (var info in CurrentRoundData.MonsterBuilderLinkInfos) {
					_monsterGenEventInfo.MonsterConfigId = info.ConfigId;
					_monsterGenEventInfo.SingleUid = info.MonsterBuilderUid;
					_monsterGenEventInfo.Behave = EMonsterGenBehave.Summon;
					BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.OnCallMonsterGenerator,
						_monsterGenEventInfo);
				}
				

				_firstTick = false;
			}

			private bool checkCondition(RoundScoreCondition condition) {
				var rtInfo = Round.GameRunningState.BattleGroundHandle.RtInfo;
				int flag = 0;
				switch (condition.TargetType) {
					case ERoundTargetType.FactionId:
						switch (condition.ConditionType) {
							case ERoundConditionType.Survival:
								flag = rtInfo.GetRoundSurvivalFaction(condition.TargetParam)
									.CompareTo(condition.ConditionCount);
								return getCompareRes(condition.CompareResType, flag);
							case ERoundConditionType.Death:
								flag = rtInfo.GetRoundDeadFaction(condition.TargetParam)
									.CompareTo(condition.ConditionCount);
								return getCompareRes(condition.CompareResType, flag);
						}

						break;
					case ERoundTargetType.SpecialUid:
						var hasActor = ActorManager.Instance.TryGetActor(condition.TargetParam, out _);
						switch (condition.ConditionType) {
							case ERoundConditionType.Survival:
								return hasActor;
							case ERoundConditionType.Death:
								return !hasActor;
						}

						break;
					case ERoundTargetType.Tag:
						if (condition.ConditionType == ERoundConditionType.Death) {
							flag = rtInfo.TagDeadCount(condition.TargetParam)
								.CompareTo(condition.ConditionCount);
							return getCompareRes(condition.CompareResType, flag);
						}

						Debug.Log("存活Tag未实现");
						break;
				}

				return false;
			}

			private bool getCompareRes(ECompareResType compareResType, int flag) {
				switch (compareResType) {
					case ECompareResType.Less:
						return flag < 0;
					case ECompareResType.LessAndEqual:
						return flag <= 0;
					case ECompareResType.Equal:
						return flag == 0;
					case ECompareResType.More:
						return flag > 0;
					case ECompareResType.MoreAndEqual:
						return flag >= 0;
				}

				return true;
			}

			protected override void onExit() {
				foreach (var abilityId in CurrentRoundData.RunningAbilityIds) {
					//BattleManager.BattleController.RemoveAbility(abilityId);
				}

				

				foreach (var info in CurrentRoundData.MonsterBuilderLinkInfos) {
					_monsterGenEventInfo.MonsterConfigId = info.ConfigId;
					_monsterGenEventInfo.SingleUid = info.MonsterBuilderUid;
					_monsterGenEventInfo.Behave = EMonsterGenBehave.Clear;
					BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.OnCallMonsterGenerator,
						_monsterGenEventInfo);
				}
				MonsterGeneratorLogic.CurMonsterCount = 0;
				Round.GameRunningState.BattleGroundHandle.RtInfo.CurRoundDurationTime = 0;
				PlayerControlPanel.Instance.Hide();
			}
		}
	}
}