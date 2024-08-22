using System.Collections.Generic;
using Battle;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility
{
    /// <summary>
    /// 打击数据
    /// </summary>
    public class HitData : SerializedScriptableObject
    { 
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
        /// 打击点检测间隔
        /// </summary>
        public long Interval;
        
        /// <summary>
        /// Damage 静态字段 TODO:临时做法，后续要接Excel
        /// </summary>
        public Dictionary<string, object> Damage = new Dictionary<string, object>();
    }
}