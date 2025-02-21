using UnityEngine;

namespace Hono.Scripts.Battle.Core {
	public class HumanMoveControllor : MoveControllor {
		public HumanMoveControllor(MoveComp moveComp) : base(moveComp) { }

		protected override Vector3 CalcVelocity() {
			return MoveSpeed * InputDirection;
		}

		protected override Quaternion CalcRotation() {
			if (InputDirection.magnitude == 0) {
				return Quaternion.identity;
			}

			var angle = Vector3.SignedAngle(_moveComp.Unit.UnitTransform.Forward, InputDirection, Vector3.up);
			if (Mathf.Abs(angle) < 1) {
				return Quaternion.identity;
			}
			return Quaternion.Euler(0, angle, 0);;
		}
	}
}