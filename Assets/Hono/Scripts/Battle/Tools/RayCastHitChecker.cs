using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public class RayCastHitChecker {
		private readonly RaycastHit[] _raycastHit = new RaycastHit[32];
		
		public bool OpenGizmos { get; set; }
		public Color GizmosColor =  new Color(0, 0.7f, 0.5f, 0.4f);

		public int GetHitActor(in CheckBoxData checkBoxData,in Vector3 selectCenterPos,in Quaternion followAttackerRot,ref List<int> actorIds) {
			var finalRot = followAttackerRot * Quaternion.Euler(checkBoxData.Rot);
			var finalCenterPos = selectCenterPos + finalRot * checkBoxData.Offset;

			int size = 0;
			
			switch (checkBoxData.ShapeType) {
				case ECheckBoxShapeType.Cube:
					var boxSize = new Vector3(checkBoxData.Length, checkBoxData.Height, checkBoxData.Width);
					size = Physics.BoxCastNonAlloc(finalCenterPos, boxSize / 2, followAttackerRot * Vector3.forward, _raycastHit, finalRot, 0.001f);
					if (OpenGizmos) {
						GizmosHelper.Instance.DrawCube(finalCenterPos, boxSize, finalRot, GizmosColor);
					}
					break;
				case ECheckBoxShapeType.Sphere:
					size = Physics.SphereCastNonAlloc(finalCenterPos, checkBoxData.Radius, followAttackerRot * Vector3.forward, _raycastHit, 0.001f);
					if (OpenGizmos)
						GizmosHelper.Instance.DrawSphere(finalCenterPos, checkBoxData.Radius, finalRot, GizmosColor);
					break;
			}
			
			actorIds.Clear();
			for (int i = 0; i < size; i++) {
				if (!_raycastHit[i].collider.TryGetComponent<ActorModel>(out var handle)) {
					continue;
				}

				if (handle.ActorType == EActorType.BattleLevelController)
				{
					continue;
				}
				
				actorIds.Add(handle.ActorUid);
			}

			return size;
		}
	}
}