#region

using Hono.Core;
using QTool;
using QTool.UI;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class PlayerControlPanel : QUI<PlayerControlPanel> {
		public CanvasGroup TeamControlView;
		public CanvasGroup PawnControlView;

		public QObjectList TeamSwitchBtnList;
		public QObjectList PawnControlBtnList;

		public override void OnFresh() {
			if (BattleManager.CurBattle.TeamController.TeamCount > 1) {
				showTeamControlView();
			}

			showPawnControlView();
		}

		protected override void OnHide() {
			TeamControlView.alpha = 0;
			TeamSwitchBtnList?.Clear();
			PawnControlView.alpha = 0;
			PawnControlBtnList?.Clear();
			base.OnHide();
		}

		private void showTeamControlView() {
			TeamControlView.alpha = 1;

			foreach (var teamState in BattleManager.CurBattle.TeamController.TeamStates) {
				if (!teamState.HasAlive()) {
					continue;
				}

				TeamSwitchBtnList.Get().SetData(teamState);
			}
		}

		private void showPawnControlView() {
			PawnControlView.alpha = 1;
			OnSwitchTeam();
		}

		public void OnSwitchTeam() {
			PawnControlBtnList.Clear();
			var curTeam = BattleManager.CurBattle.TeamController.CurControlTeam;
			foreach (var memberState in curTeam.MemberStates) {
				if (memberState.CurStateType == EPawnTeamMemberStateType.Empty) {
					continue;
				}

				PawnControlBtnList.Get().SetData(memberState);
			}
		}

		public void TeamAssemblyClick() {
			BattleManager.CurBattle.TeamController.CurControlTeam?.TeamAssembly();
		}
	}
}