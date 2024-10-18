using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools {
	public class RemoveBuildingButton : MonoBehaviour{
		
		public void RemoveBuilding(BuildingActorModel actorModel) {
			if(actorModel == null) return;
			ActorManager.Instance.RemoveActor(actorModel.ActorUid);
			BattleManager.CurrentBattleGround.RuntimeInfo.RPCount += actorModel.CostRPCount;
		}
	}
}