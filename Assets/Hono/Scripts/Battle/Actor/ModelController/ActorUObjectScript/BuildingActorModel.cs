#region

using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.DebugTools;
using System;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public class BuildingActorModel : ActorModel {
		[NonSerialized] public bool IsBuildingModel;
		[NonSerialized] public EBuildingType BuildingType;
		private bool _canBuilding;
		private ActorModelFollowMouse _followMouse;
		[NonSerialized] public int BuildTableRowId;
		[NonSerialized] public int CostRPCount;

		private void Awake() {
			ActorUid = ActorUidGenerator.GenerateUid(EActorUidRangeType.NormalActor);
			ActorType = EActorType.Building;
		}

		private void Start() {
			if (_followMouse != null) {
				return;
			}

			if (!TryGetComponent(out _followMouse)) {
				_followMouse = gameObject.AddComponent<ActorModelFollowMouse>();
			}

			_followMouse.enabled = IsBuildingModel;
		}

		public void ChangeBuildState(bool canBuild) {
			_canBuilding = canBuild;
		}

		public void Update() {
			if (!IsBuildingModel) return;

			if (Input.GetMouseButtonDown(0) && _canBuilding) {
				//左键
				IsBuildingModel = false;
				_followMouse.enabled = false;
				BattleManager.CurBattle.RtInfo.RPCount -= CostRPCount;
				/*ActorManager.Instance.CreateBuilding(this, BuildTableRowId, (build) => {
					build.SetAttr(ELogicAttr.AttrPosition, transform.position, false);
				});*/
			}

			if (Input.GetMouseButtonDown(1)) {
				_followMouse.enabled = false;
				Destroy(gameObject);
			}
		}
	}
}