using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public class GizmosHelper : MonoSingleton<GizmosHelper> {
		private (Vector3 centerPos, Vector3 half, Quaternion rot, Color color) _cube;
		private bool _hasCubeDraw;

		private (Vector3 centerPos, float radius, Quaternion rot, Color color) _sphere;
		private bool _hasSphereDraw;

		private float _dTime;

		public void DrawCube(Vector3 centerPos, Vector3 half, Quaternion rot, Color color) {
			_cube.centerPos = centerPos;
			_cube.half = half;
			_cube.rot = rot;
			_cube.color = color;
			_hasCubeDraw = true;
		}

		public void DrawSphere(Vector3 centerPos, float radius, Quaternion rot, Color color) {
			_sphere.centerPos = centerPos;
			_sphere.radius = radius;
			_sphere.rot = rot;
			_sphere.color = color;
			_hasSphereDraw = true;
		}

		private void onDrawCube() {
			// 设置 Gizmos 颜色
			Gizmos.color = new Color(0.3f,0.8f,0.1f,0.5f);

			// 保存当前 Gizmos 矩阵
			Matrix4x4 oldMatrix = Gizmos.matrix;

			// 设置 Gizmos 矩阵，使矩形根据位置和尺寸绘制
			Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

			// 绘制填充矩形
			Gizmos.DrawCube(_cube.centerPos, _cube.half);

			// 恢复 Gizmos 矩阵
			Gizmos.matrix = oldMatrix;
		}

		private void onDrawSphere() {
			// 设置 Gizmos 颜色
			Gizmos.color = new Color(0.3f,0.8f,0.1f,0.5f);

			// 保存当前 Gizmos 矩阵
			Matrix4x4 oldMatrix = Gizmos.matrix;

			// 设置 Gizmos 矩阵，使矩形根据位置和尺寸绘制
			Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

			// 绘制填充矩形
			Gizmos.DrawSphere(_sphere.centerPos, _sphere.radius);

			// 恢复 Gizmos 矩阵
			Gizmos.matrix = oldMatrix;
		}


		private void OnDrawGizmos() {
			if (_hasCubeDraw) {
				onDrawCube();
			}

			if (_hasSphereDraw) {
				onDrawSphere();
			}

			if (_hasCubeDraw || _hasSphereDraw) {
				_dTime += Time.deltaTime;
			}

			if (_dTime > 10) {
				_hasCubeDraw = false;
				_hasSphereDraw = false;
				_dTime = 0;
			}
		}
	}
}