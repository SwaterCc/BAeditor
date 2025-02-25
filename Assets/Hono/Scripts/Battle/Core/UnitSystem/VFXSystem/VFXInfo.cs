#region

using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Tools;
using Unity.Mathematics;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public struct VFXInfo {
		/// <summary>
		/// 特效Uid,全局共享同一个Id生成器
		/// </summary>
		public int Uid;

		/// <summary>
		/// 特效持有者的Uid
		/// </summary>
		public int OwnerUid;

		/// <summary>
		/// VFX的实际路径
		/// </summary>
		public string Path;

		/// <summary>
		/// 坐标
		/// </summary>
		public Vector3 Pos;

		/// <summary>
		/// 旋转
		/// </summary>
		public Quaternion Rot;

		/// <summary>
		/// 缩放
		/// </summary>
		public Vector3 Scale;

		/// <summary>
		/// 创建时间
		/// </summary>
		public float CreateTime;

		/// <summary>
		/// 生存时间
		/// </summary>
		public float LifeTime;

		/// <summary>
		/// 是否常驻特效
		/// </summary>
		public bool IsPermanent;
	}
}