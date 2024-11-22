#region

using Hono.Core;
using QTool.Tween;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class SwitchTeamBtn : MonoBehaviour, IView<PawnTeamState> {
		private PawnTeamState _pawnTeamState;

		public Slider HpBar;
		public QTweenLoop FullMpEffect;
		public Image Icon;

		public Transform DeadState;
		public Transform SelectState;

		public void OnFresh(PawnTeamState data) {
			_pawnTeamState = data;

			if (_pawnTeamState.LeaderUid == -1) return;

			if (!ActorManager.Instance.TryGetActor(_pawnTeamState.LeaderUid, out var leader)) {
				return;
			}

			var pawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(leader.ConfigId);

			if (pawnLogicRow != null) {
				var icon = Resources.Load<Sprite>(pawnLogicRow.RPGIcon);
				if (icon)
					Icon.sprite = icon;
			}
		}

		public void Update() {
			if (_pawnTeamState == null) return;

			DeadState.gameObject.SetActive(false);
			SelectState.gameObject.SetActive(_pawnTeamState.IsControl);
			if (!_pawnTeamState.HasAlive()) {
				DeadState.gameObject.SetActive(true);
			}
		}

		public void OnSwitchClick() {
			if (_pawnTeamState.IsControl) return;
			if (!_pawnTeamState.HasAlive()) return;
			BattleManager.CurBattle.TeamController.ChangeControlTeam(_pawnTeamState.TeamIndex);
		}
	}
}