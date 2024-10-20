using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools {
	public class CreateBuildingButton : MonoBehaviour {

		public int ModelId;
		
		public void OnBuildClick() {
			var buildingLogicRow = ConfigManager.Table<BuildingLogicTable>().Get(ModelId);

			var canBuild = BattleManager.CurrentBattleGround.RuntimeInfo.RPCount >= buildingLogicRow.TempCost;
			if (false&&!canBuild) {
				Debug.Log("战斗点数不足");
				return;
			}
			var modelRow = ConfigManager.Table<ModelTable>().Get(buildingLogicRow.Model);
			var obj = GameObjectPreLoadMgr.Instance.GetBuildingCache(modelRow.ModelPath);
			Instantiate(obj, Vector3.zero,Quaternion.identity);
			if (obj.TryGetComponent<BuildingActorModel>(out var comp)) {
				comp.BuildTableRowId = buildingLogicRow.Id;
				comp.CostRPCount = buildingLogicRow.TempCost;
				comp.Path = modelRow.ModelPath;
			}
		}
	}
}