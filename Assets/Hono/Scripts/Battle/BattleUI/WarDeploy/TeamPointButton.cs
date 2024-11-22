#region

using Hono.Core;
using TMPro;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class TeamPointButton : MonoBehaviour, IView<int> {
		public TextMeshProUGUI TeamName;
		private int _teamIdx;

		public void OnFresh(int data) {
			_teamIdx = data;
			TeamName.SetText($"队伍 {_teamIdx + 1}");
		}

		public void OnTeamPointClick() {
			/*var obj = GameObjectPreLoadMgr.Instance[EPreLoadGameObjectType.TeamRefreshPoint];
			obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
			if (obj.TryGetComponent<TeamRefreshPoint>(out var comp)) {
				comp.TeamId = _teamIdx;
				comp.IsBuildingModel = true;
			}*/
		}
	}
}