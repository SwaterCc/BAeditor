#region

using Sirenix.OdinInspector;
using System;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	[Serializable]
	public class CheckBoxData {
		public ECheckBoxShapeType ShapeType;

		public Vector3 Rot;

		//是否应用百分比存储偏移
		public Vector3 Offset;

		/// <summary>
		///     矩形长轴（X）
		/// </summary>
		[ShowIf("ShapeType", ECheckBoxShapeType.Cube)]
		public float Length;

		/// <summary>
		///     矩形宽轴（Z）
		/// </summary>
		[ShowIf("ShapeType", ECheckBoxShapeType.Cube)]
		public float Width;

		/// <summary>
		///     矩形高（Y）
		/// </summary>
		[HideIf("ShapeType", ECheckBoxShapeType.Sphere)]
		public float Height;

		[HideIf("ShapeType", ECheckBoxShapeType.Cube)]
		public float Radius;

		[HideInInspector] public float Angle;
	}
}