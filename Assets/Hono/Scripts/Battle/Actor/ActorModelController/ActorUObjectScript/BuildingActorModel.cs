using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.DebugTools;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class BuildingActorModel : ActorModel {
		private bool _isPreShow;
		
		private bool _canBuilding;

		private ActorModelFollowMouse _followMouse;
		public int BuildTableRowId;
		public int CostRPCount;

		private void Awake() {
			ActorUid = ActorUidGenerator.GenerateUid(EActorUidRangeType.NormalActor);
			ActorType = EActorType.Building;
		}

		private void Start() {
			_isPreShow = true;
			
			if (_followMouse != null) {
				return;
			}

			if (!TryGetComponent(out _followMouse)) {
				_followMouse = gameObject.AddComponent<ActorModelFollowMouse>();
			}

			_followMouse.enabled = true;
		}

		public void ChangeBuildState(bool canBuild) {
			_canBuilding = canBuild;
		}

		public void Update() {
			if (!_isPreShow) return;
			
			if (Input.GetMouseButtonDown(0) && _canBuilding) {
				//左键
				_isPreShow = false;
				_followMouse.enabled = false;
				ActorManager.Instance.CreateBuilding(this, BuildTableRowId, (build) => {
					build.SetAttr(ELogicAttr.AttrPosition, transform.position, false);
				});
			}

			if (Input.GetMouseButtonDown(1)) {
				_followMouse.enabled = false;
				Destroy(gameObject);
			}
		}
	}
}