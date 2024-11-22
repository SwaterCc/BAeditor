#region

using Hono.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class RoundInfoPanel : MonoBehaviour, IView<(int, int)> {
		public Slider Slider;
		public TextMeshProUGUI LastCount;

		public void OnFresh((int, int) data) {
			var curRound = data.Item1;
			var maxRound = data.Item2;

			Slider.value = curRound;
			Slider.maxValue = maxRound;
			LastCount.SetText((maxRound - curRound).ToString());
		}

		public void ShowPanel() {
			if (TryGetComponent<CanvasGroup>(out var canvasGroup)) {
				canvasGroup.alpha = 1;
			}
		}

		public void HidePanel() {
			if (TryGetComponent<CanvasGroup>(out var canvasGroup)) {
				canvasGroup.alpha = 0;
			}
		}
	}
}