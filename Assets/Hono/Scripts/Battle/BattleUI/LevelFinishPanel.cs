#region

using QTool.UI;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class LevelFinishPanel : QUI<LevelFinishPanel> {
		public CanvasGroup Success;
		public CanvasGroup Failure;

		public void ShowSuccess() {
			Show();
			Failure.alpha = 0;
			Success.alpha = 1;
		}

		public void ShowFailure() {
			Show();
			Failure.alpha = 1;
			Success.alpha = 0;
		}

		public void OnExit() {
			Hide();
			BattleManager.Instance.ExitBattle();
		}
	}
}