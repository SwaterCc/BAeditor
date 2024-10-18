using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public class AllowBuildArea : MonoBehaviour {

		private BoxCollider _area;
		
		public void Awake() {
			if (!gameObject.TryGetComponent(out _area)) {
				_area = gameObject.AddComponent<BoxCollider>();
			}
		}

		public void OnTriggerEnter(Collider other) {
			if (other.TryGetComponent<BuildingActorModel>(out var building)) {
				building.ChangeBuildState(true);
			}
		}

		public void OnTriggerExit(Collider other) {
			if (other.TryGetComponent<BuildingActorModel>(out var building)) {
				building.ChangeBuildState(false);
			}
		}
	}
}