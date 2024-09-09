using System;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击数据
    /// </summary>
    [Serializable] 
    public class HitBoxData
    {
        /// <summary>
        /// 伤害盒子类型
        /// </summary>
        public EHitType HitType;
        
        /// <summary>
        /// 打击点最大检测次数
        /// </summary>
        public int MaxCount = 1;

        /// <summary>
        /// 打击点对单个目标最大有效次数
        /// </summary>
        [Tooltip("打击点对单个目标最大有效次数")]
        public int ValidCount;
        
        /// <summary>
        /// 第一次触发时间
        /// </summary>
        public float FirstInterval;
        
        /// <summary>
        /// 打击点检测间隔
        /// </summary>
        public float Interval;
        
        /// <summary>
        /// 单次打击造成几次伤害
        /// </summary>
        [Tooltip("单次打击造成几次伤害")]
        public int OnceHitDamageCount = 1;

        /// <summary>
        /// Aoe打击点AABB盒子数据
        /// </summary>
        [OdinSerialize]
        public CheckBoxData AoeData;

        /// <summary>
        /// 伤害数据
        /// </summary>
        public int DamageConfigId;
    }
}