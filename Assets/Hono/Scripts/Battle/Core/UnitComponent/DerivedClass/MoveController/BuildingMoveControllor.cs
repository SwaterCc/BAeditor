using UnityEngine;

namespace Hono.Scripts.Battle.Core {
	public class BuildingMoveControllor : MoveControllor {
		public BuildingMoveControllor(MoveComp moveComp) : base(moveComp) { }

		protected override Vector3 CalcVelocity() {
			return Vector3.zero;
		}

		protected override Quaternion CalcRotation() {
			if (InputDirection.magnitude == 0) {
				return Quaternion.identity;
			}

			var angle = Vector3.SignedAngle(_moveComp.Unit.UnitTransform.Forward, InputDirection, Vector3.up);
			return Quaternion.Euler(0, angle, 0);;
		}
	}
}