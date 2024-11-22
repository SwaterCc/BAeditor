#region

using Hono.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class PawnSelectButton : MonoBehaviour, IView<(int, Action<int>)> {
		private Action<int> _onSelect;
		private int _configId;
		public TextMeshProUGUI MainClassName;
		public TextMeshProUGUI SubClassName;
		public Image icon;
		public Toggle Toggle;

		public void OnFresh((int, Action<int>) data) {
			if (Toggle.group == null) {
				Toggle.group = transform.GetComponentInParent<ToggleGroup>();
			}

			_configId = data.Item1;
			_onSelect = data.Item2;

			var pawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(_configId);

			if (pawnLogicRow.ActorClassId > 0) {
				var mainClassName = ConfigManager.Table<ActorClassTable>().Get(pawnLogicRow.ActorClassId).ClassName;
				MainClassName.SetText(mainClassName);
			}

			if (pawnLogicRow.ActorSubClassId > 0) {
				var subClassName = ConfigManager.Table<ActorClassTable>().Get(pawnLogicRow.ActorSubClassId).ClassName;
				SubClassName.SetText(subClassName);
			}


			if (!string.IsNullOrEmpty(pawnLogicRow.RPGIcon)) {
				var sprite = Resources.Load<Sprite>(pawnLogicRow.RPGIcon);
				if (sprite != null) {
					icon.sprite = sprite;
				}
			}
		}

		public void OnBtnSelect(bool flag) {
			if (flag) {
				_onSelect.Invoke(_configId);
			}
		}
	}
}