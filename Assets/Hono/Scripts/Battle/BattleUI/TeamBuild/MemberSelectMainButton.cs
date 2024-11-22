#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class MemberSelectMainButton : MonoBehaviour {
		public Image icon;
		public TextMeshProUGUI mainClassName;
		public TextMeshProUGUI subClassName;
		public int Index;

		private int _pawnConfigId;

		public void SetPawnConfigId(int configId) {
			_pawnConfigId = configId;
			refreshButtonView();
		}

		private void refreshButtonView() {
			mainClassName.SetText("");
			subClassName.SetText("");
			icon.enabled = false;
			if (_pawnConfigId != -1) {
				var pawnRow = ConfigManager.Table<PawnLogicTable>().Get(_pawnConfigId);
				if (pawnRow.ActorClassId > 0) {
					setClassName(mainClassName, pawnRow.ActorClassId);
				}

				if (pawnRow.ActorSubClassId > 0) {
					setClassName(subClassName, pawnRow.ActorSubClassId);
				}

				if (!string.IsNullOrEmpty(pawnRow.RPGIcon)) {
					var sprite = Resources.Load<Sprite>(pawnRow.RPGIcon);
					if (sprite != null) {
						icon.sprite = sprite;
						icon.enabled = true;
					}
				}
			}
		}

		public void OnBtnClick() {
			TeamSelectPanel.Instance.OnMemberBtnClick(Index);
		}

		private void setClassName(TextMeshProUGUI text, int id) {
			var classRow = ConfigManager.Table<ActorClassTable>().Get(id);
			text.SetText(classRow == null ? "ERROR" : classRow.ClassName);
		}
	}
}