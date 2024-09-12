using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public static class LayerMask {
		public static int ActorMask = 1;
	}


	public static class CommonUtility {
		/// <summary>
		/// 根据时间使用MD5进行哈希计算并返回一个32位整型值
		/// </summary>
		/// <returns></returns>
		public static int GenerateTimeBasedHashId32() {
			// 获取当前时间，并转化为字符串格式
			string currentTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

			// 使用MD5进行哈希计算并返回一个32位整型值
			using (MD5 md5 = MD5.Create()) {
				byte[] bytes = Encoding.UTF8.GetBytes(currentTime);
				byte[] hashBytes = md5.ComputeHash(bytes);

				// 将前4个字节转换为32位整型值
				int hashValue = BitConverter.ToInt32(hashBytes, 0);
				hashValue = Math.Abs(hashValue);
				return hashValue;
			}
		}

		public class IdGenerator {
			private int currentId = 0;

			/// <summary>
			/// 生成新的ID。ID 会自增并且始终大于0。
			/// </summary>
			/// <returns>新的ID</returns>
			public int GenerateId() {
				// 使用Interlocked.Increment确保线程安全
				int newId = Interlocked.Increment(ref currentId);

				// 检查是否溢出（如果超出int.MaxValue，重置为1）
				if (newId == int.MaxValue) {
					Interlocked.Exchange(ref currentId, 0);
					newId = Interlocked.Increment(ref currentId);
				}

				return newId;
			}
		}

		/// <summary>
		/// 获取Id生成器
		/// </summary>
		/// <returns></returns>
		public static IdGenerator GetIdGenerator() {
			return new IdGenerator();
		}

		public static bool HitRayCast(CheckBoxData data, Vector3 selectCenterPos, Quaternion followAttackerRot,
			out List<int> actorIds) {
			actorIds = null;
			//获取中心坐标
			//var offset = data.OffsetUsePercent ? vector3Sub(data.Scale, data.Offset) : data.Offset;
			var finalRot = followAttackerRot * data.Rot;
			var centerPos = selectCenterPos + finalRot * data.Offset;

			//最大命中数量
			List<RaycastHit> raycastHits = new List<RaycastHit>();
			switch (data.ShapeType) {
				case ECheckBoxShapeType.Cube:
					var half = new Vector3(data.Length/2, data.Height/2, data.Width/2);
					raycastHits.AddRange(Physics.BoxCastAll(centerPos, half, followAttackerRot * Vector3.forward,
						data.Rot, 0.001f));
					GizmosHelper.Instance.DrawCube(centerPos, half, finalRot, Color.green);
					break;
				case ECheckBoxShapeType.Sphere:
					raycastHits.AddRange(Physics.SphereCastAll(centerPos, data.Radius,
						followAttackerRot * Vector3.forward,
						0.001f));
					GizmosHelper.Instance.DrawSphere(centerPos, data.Radius, finalRot, Color.green);
					break;
				case ECheckBoxShapeType.Cylinder:
					raycastHits.AddRange(Physics.CapsuleCastAll(centerPos + Vector3.up * (data.Height / 2),
						centerPos + Vector3.down * (data.Height / 2),
						0.1f,
						followAttackerRot * Vector3.forward, 0.001f));
					GizmosHelper.Instance.DrawCube(centerPos,
						new Vector3(data.Radius, data.Height, data.Radius), finalRot,
						Color.green);
					break;
				default:
					Debug.LogError("使用了未实现的检测");
					return false;
			}

			foreach (var raycastHit in raycastHits) {
				Debug.Log($"handle Name {raycastHit.collider.gameObject.name}");
				if (!raycastHit.collider.TryGetComponent<ActorModelHandle>(out var handle)) {
					continue;
				}

				actorIds ??= new List<int>();
				actorIds.Add(handle.ActorUid);
			}

			return actorIds != null;
		}
	}

	#region ParamExtension

	

	#endregion
}