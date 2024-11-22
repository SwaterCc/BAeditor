#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools {
	public class AllowTeamPointArea : MonoBehaviour {
		private BoxCollider _area;

		public void Awake() {
			if (!gameObject.TryGetComponent(out _area)) {
				_area = gameObject.AddComponent<BoxCollider>();
			}
		}

		public void OnTriggerEnter(Collider other) {
			if (other.TryGetComponent<TeamRefreshPoint>(out var point)) {
				point.ChangeBuildState(true);
			}
		}

		public void OnTriggerExit(Collider other) {
			if (other.TryGetComponent<TeamRefreshPoint>(out var point)) {
				point.ChangeBuildState(false);
			}
		}
	}
}