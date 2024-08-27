using UnityEngine;

namespace Battle
{
    public abstract class AabbData
    {
        public EAabbType AabbType;
        public Vector3 Offset;
        public Vector3 Scale;
        public Quaternion Rot;
    }

    public class AabbRect : AabbData
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
    public class AabbSphere : AabbData
    {
        public float Radius;
    }

    /// <summary>
    /// 圆柱
    /// </summary>
    public class AabbCylinder : AabbData
    {
        public float Radius;
        public float Height;
    }

    /// <summary>
    /// 扇形
    /// </summary>
    public class AabbSector : AabbData
    {
        public float Radius;
        public float Height;
        public float Angle;
    }
}