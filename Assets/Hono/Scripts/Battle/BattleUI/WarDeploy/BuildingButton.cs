#region

using Hono.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class BuildingButton : MonoBehaviour, IView<BuildingLogicTable.BuildingLogicRow> {
		private int _buildingId;
		private int _modelId;
		private int _tempCost;
		private EBuildingType _buildingType;
		public TextMeshProUGUI BuidingName;
		public TextMeshProUGUI BuildingCost;
		public Image Icon;
		private int _beforeRp;

		public void OnFresh(BuildingLogicTable.BuildingLogicRow data) {
			_buildingId = data.Id;
			BuidingName.SetText(data.Name);
			_tempCost = data.TempCost;
			BuildingCost.SetText(data.TempCost.ToString());
			_modelId = data.Model;
			_buildingType = (EBuildingType)data.BuildingType;
			if (!string.IsNullOrEmpty(data.RPGIcon)) {
				var newSprite = Resources.Load<Sprite>(data.RPGIcon);
				if (newSprite != null) {
					Icon.sprite = newSprite;
				}
				else {
					Debug.LogWarning($"Sprite {data.RPGIcon} not found!");
				}
			}

			_beforeRp = BattleManager.CurBattle.RtInfo.RPCount;
			if (_tempCost > BattleManager.CurBattle.RtInfo.RPCount) {
				BuildingCost.color = Color.red;
				BuidingName.color = Color.red;
			}
			else {
				BuildingCost.color = Color.white;
				BuidingName.color = Color.white;
			}
		}

		private void Update() {
			if (BattleManager.CurBattle == null) return;
			if (_beforeRp != BattleManager.CurBattle.RtInfo.RPCount) {
				if (_tempCost > BattleManager.CurBattle.RtInfo.RPCount) {
					BuildingCost.color = Color.red;
					BuidingName.color = Color.red;
				}
				else {
					BuildingCost.color = Color.white;
					BuidingName.color = Color.white;
				}

				_beforeRp = BattleManager.CurBattle.RtInfo.RPCount;
			}
		}

		public void OnBuildingBtnClick() {
			var canBuild = BattleManager.CurBattle.RtInfo.RPCount >= _tempCost;
			if (!canBuild) {
				Debug.Log("战斗点数不足");
				return;
			}

			var modelRow = ConfigManager.Table<ModelTable>().Get(_modelId);
			var obj = GameObjectPreLoadMgr.Instance.GetBuildingCache(modelRow.ModelPath);
			obj = Instantiate(obj, Vector3.zero, Quaternion.identity);
			if (obj.TryGetComponent<BuildingActorModel>(out var comp)) {
				comp.BuildTableRowId = _buildingId;
				comp.CostRPCount = _tempCost;
				comp.IsBuildingModel = true;
				comp.BuildingType = _buildingType;
			}
		}
	}
}