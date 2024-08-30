using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
	[Serializable]
    public abstract class AabbData
    {
        public EAabbType AabbType;
        public Vector3 Scale = Vector3.one;
        public Quaternion Rot = Quaternion.identity;
        //是否应用百分比存储偏移
        public bool OffsetUsePercent = true;
        public Vector3 Offset = Vector3.one;
    }

	[Serializable]
    public class AabbCube : AabbData
    {
        /// <summary>
        /// 矩形长轴（X）
        /// </summary>
        public float Length;

        /// <summary>
        /// 矩形宽轴（Z）
        /// </summary>
        public float Width;

        /// <summary>
        /// 矩形高（Y）
        /// </summary>
        public float Height;
    }

    /// <summary>
    /// 球
    /// </summary>
    [Serializable]
    public class AabbSphere : AabbData
    {
        public float Radius;
    }

    /// <summary>
    /// 圆柱
    /// </summary>
    [Serializable]
    public class AabbCylinder : AabbData
    {
        public float Radius;
        public float Height;
    }

    /// <summary>
    /// 扇形
    /// </summary>
    [Serializable]
    public class AabbSector : AabbData
    {
        public float Radius;
        public float Height;
        public float Angle;
    }
}