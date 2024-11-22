#region

using Hono.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class TeamBuildButton : MonoBehaviour, IView<(int, Action<int>)> {
		private int _index;
		private Action<int> _onSelectCallBack;
		public TextMeshProUGUI TextMeshPro;

		public void OnFresh((int, Action<int>) data) {
			_index = data.Item1;
			_onSelectCallBack = data.Item2;
			TextMeshPro.SetText($"队伍{_index + 1}");
			if(transform.parent.TryGetComponent<ToggleGroup>(out var group)) {
				GetComponent<Toggle>().group = group;
			}
		}

		public void OnSelect(bool flag) {
			if (!flag) return;
			_onSelectCallBack.Invoke(_index);
		}
	}
}