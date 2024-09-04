using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击数据
    /// </summary>
    public class HitBoxData : ScriptableObject, IAllowedIndexing
    {
        public int ID => ConfigId;

        /// <summary>
        /// 打击点唯一ID
        /// </summary>
        public int ConfigId;

        /// <summary>
        /// 伤害盒子类型
        /// </summary>
        public EHitType HitType;

        /// <summary>
        /// 延迟生效时间
        /// </summary>
        public long DelayTime;

        /// <summary>
        /// 打击点持续时长
        /// </summary>
        public long Duration;

        /// <summary>
        /// 打击点有效次数
        /// </summary>
        public int ValidCount;

        /// <summary>
        /// 单次检测时长
        /// </summary>
        public int EffectTime;

        /// <summary>
        /// 打击点检测间隔
        /// </summary>
        public long Interval;

        /// <summary>
        /// 打击点形状类型
        /// </summary>
        public ECheckBoxShapeType ShapeType;

        /// <summary>
        /// Aoe打击点AABB盒子数据
        /// </summary>
        public CheckBoxData AoeData;

        /// <summary>
        /// 伤害数据
        /// </summary>
        public int DamageConfigId;
    }
}