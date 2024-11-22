#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public partial class BattleGround {
		public class GameRunningState : BattleState {
			private readonly RoundController _roundController;
			private bool _roundInitFlag;

			public GameRunningState(BattleGround battleGroundHandle, EBattleStateType stateType) : base(
				battleGroundHandle, stateType) {
				_roundController = new RoundController(this);
			}

			protected override void onEnter() {
				BattleGroundHandle.RtInfo.RPCount = BattleGroundHandle._levelData.InitRPCount;
				_roundInitFlag = _roundController.InitRoundController(BattleGroundHandle._levelData.RoundDatas,
					BattleGroundHandle._levelData.CanRepeatRound);
				if (!_roundInitFlag) {
					Debug.LogError("回合数据不对，启动失败");
				}


				/*foreach (var abilityId in BattleGroundHandle._levelData.BattleControllerAbilitys) {
					BattleManager.BattleController.RunAbility(abilityId);
				}*/
			}

			protected override void onTick(float dt) {
				if (_roundController.CurrentState == ERoundState.NoRunning) {
					_roundController.SwitchState(ERoundState.Ready);
				}

				BattleGroundHandle._pawnTeamController.Tick(dt);

				_roundController.Tick(dt);
			}

			public void RoundBegin() {
				if (_roundController.CurrentState != ERoundState.Ready) {
					return;
				}

				if (BattleGroundHandle._pawnTeamController.IsReady) {
					_roundController.SwitchState(ERoundState.Running);
				}
				else {
					Debug.LogError("队伍未准备好");
				}
			}

			public void ScoreBattle(bool isPass) {
				BattleGroundHandle._isScoreSuccess = isPass;
				BattleGroundHandle.switchState(EBattleStateType.Score);
			}

			protected override void onExit() {
				_roundController.SwitchState(ERoundState.NoRunning);
			}
		}
	}
}