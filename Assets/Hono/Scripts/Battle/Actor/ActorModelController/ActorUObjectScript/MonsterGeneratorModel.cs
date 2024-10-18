using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class MonsterGeneratorModel : SceneActorModel {
		public List<Transform> WayPoint = new();

		protected override void onInit() {
			ActorType = EActorType.MonsterGenerator;
			name = $"MonsterGenerator:{ActorUid}";
		}

		public void GetWayPoint(ref List<Vector3> wayPoints) {
			foreach (var trans in WayPoint) {
				wayPoints.Add(trans.position);
			}
		}
	}
}