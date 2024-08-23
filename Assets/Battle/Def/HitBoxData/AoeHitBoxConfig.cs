using Battle;
using UnityEngine;

namespace BattleAbility
{
    /// <summary>
    /// 范围打击点定义
    /// </summary>
    [CreateAssetMenu(menuName = "战斗编辑器/AoeHitBoxConfig")] 
    public class AoeHitBoxConfig : HitBoxConfig
    {
        public EHitAreaType HitAreaType;

        public AreaData HitAreaData;

        public abstract class AreaData
        {
            public Vector3 Offset;
            public Vector3 Scale;
            public Quaternion Rot;
        }

        public class RectData : AreaData
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
        public class Sphere : AreaData
        {
            public float Radius;
        }

        /// <summary>
        /// 圆柱
        /// </summary>
        public class Cylinder : AreaData
        {
            public float Radius;
            public float Height;
        }

        /// <summary>
        /// 扇形
        /// </summary>
        public class Sector : AreaData
        {
            public float Radius;
            public float Height;
            public float Angle;
        }
    }
}