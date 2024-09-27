using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public enum EGizmosDrawType {
		Cube,
		Sphere,
	}
	public class GizmosInfo {
		private float _duration = 0;
		public bool Invalid { get; private set; }

		public EGizmosDrawType DrawType;
		
		public Vector3 CubeSize;
		public Vector3 CenterPos;
		public Vector3 CubeTransform;
		public Quaternion Rot;
		public Color DrawColor;
		public float Radius;
		
		public float MaxDrawDuration = 5;

		public void Update(float dt) {
			_duration += dt;
			if (_duration >= MaxDrawDuration) {
				Invalid = true;
			}
		}
	}
	
	public class GizmosHelper : MonoSingleton<GizmosHelper> {

		private List<GizmosInfo> _drawList = new();
		private List<GizmosInfo> _removeList = new();
		
		public void DrawCube(Vector3 centerPos,Vector3 cubeSize, Quaternion rot, Color color) {
			var cube = new GizmosInfo() {
				DrawType = EGizmosDrawType.Cube,
				CubeSize = cubeSize,
				Rot = rot,
				CenterPos = centerPos,
				DrawColor = color
			};
			_drawList.Add(cube);
		}

		public void DrawSphere(Vector3 centerPos, float radius, Quaternion rot, Color color) {
			var sphere = new GizmosInfo() {
				DrawType = EGizmosDrawType.Sphere,
				Radius = radius,
				Rot = rot,
				CenterPos = centerPos,
				DrawColor = color
			};
			_drawList.Add(sphere);
		}

		private void drawCube(GizmosInfo gizmosInfo) {
			//设置颜色
			Gizmos.color = gizmosInfo.DrawColor;
			
			// 设置 Gizmos 矩阵，使矩形根据位置和尺寸绘制
			Gizmos.matrix = Matrix4x4.TRS(gizmosInfo.CenterPos, gizmosInfo.Rot, Vector3.one);

			// 绘制填充矩形
			Gizmos.DrawCube(Vector3.zero,gizmosInfo.CubeSize);
		}

		private void drawSphere(GizmosInfo gizmosInfo) {
			//设置颜色
			Gizmos.color = gizmosInfo.DrawColor;
			
			// 设置 Gizmos 矩阵，使矩形根据位置和尺寸绘制
			Gizmos.matrix = Matrix4x4.TRS(gizmosInfo.CenterPos, Quaternion.identity, Vector3.one);

			// 绘制填充矩形
			Gizmos.DrawSphere(Vector3.zero, gizmosInfo.Radius);
		}


		private void OnDrawGizmos() {
			foreach (var gizmosInfo in _drawList) {
				Matrix4x4 oldMatrix = Gizmos.matrix;
				switch (gizmosInfo.DrawType) {
					case EGizmosDrawType.Cube:
						drawCube(gizmosInfo);
						break;
					case EGizmosDrawType.Sphere:
						drawSphere(gizmosInfo);
						break;
				}
				Gizmos.matrix = oldMatrix;
				gizmosInfo.Update(Time.deltaTime);
				if (gizmosInfo.Invalid) {
					_removeList.Add(gizmosInfo);
				}
			}
			
			foreach (var remove in _removeList) {
				_drawList.Remove(remove);
			}
		}
	}
}