using System.Collections.Generic;
using Battle.Def;
using UnityEngine;

namespace BattleAbility
{
    /// <summary>
    /// 打击数据
    /// </summary>
    public class HitData
    {
        /// <summary>
        /// 打击点唯一ID
        /// </summary>
        public int HitUid;

        /// <summary>
        /// 打击点检测开始时间
        /// </summary>
        public long BeginTime;

        /// <summary>
        /// 打击点持续时长
        /// </summary>
        public long Duration;

        /// <summary>
        /// 打击点检测结束时间
        /// </summary>
        public long EndTime;

        /// <summary>
        /// 打击点有效次数
        /// </summary>
        public int ValidCount;

        /// <summary>
        /// 打击点检测间隔
        /// </summary>
        public long Interval;

        /// <summary>
        /// 来自于什么能力创建的打击点
        /// </summary>
        public EAbilityType BelongType;

        /// <summary>
        /// 打击点归属物的ID
        /// </summary>
        public int SourceId;
    }

    /// <summary>
    /// 范围打击点定义
    /// </summary>
    public abstract class AoeHitDataAbstract : HitData
    {
        public EHitAreaType HitAreaType;

        protected readonly AreaData HitAreaData;
        
        protected AoeHitDataAbstract(EHitAreaType hitAreaType, AreaData hitAreaData)
        {
            HitAreaData = hitAreaData;
            HitAreaType = hitAreaType;
        }

        public abstract class AreaData
        {
            public Vector3 Pos;
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

    public class AoeHitData<TAreaData> : AoeHitDataAbstract where TAreaData : AoeHitDataAbstract.AreaData
    {
        public new TAreaData HitAreaData => base.HitAreaData == null ? null : base.HitAreaData as TAreaData;

        public AoeHitData(EHitAreaType hitAreaType, AreaData hitAreaData) : base(hitAreaType, hitAreaData)
        {
            
        }
    }

    /// <summary>
    /// 锁定打击点
    /// </summary>
    public class LockTargetHitData : HitData
    {
        /// <summary>
        /// 锁定多个目标
        /// </summary>
        public int[] TargetIds;
        
        /// <summary>
        /// 锁定单个目标
        /// </summary>
        public int SingleTargetId;
    }
}