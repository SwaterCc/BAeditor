using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     打击数据
    /// </summary>
    [Serializable]
    public class HitBoxData
    {
        /// <summary>
        /// 脱手打击
        /// </summary>
        [Tooltip("脱手打击盒会保存盒子创建时的属性快照，即时攻击者已经死亡仍然可以造成伤害")]
        [LabelText("是否为脱手打击盒")]
        public bool isOffHand;
        
        /// <summary>
        /// 伤害盒子类型
        /// </summary>
        [LabelText("打击类型")]
        public EHitType hitType;

        [LabelText("Aoe筛选配置")]
        [ShowIf("hitType", EHitType.Aoe)]
        public ConditionFilterSetting rangeFilterSetting = new();
        
        [ShowIf("isOffHand")]
        public int maxHitCount;
        
        [ShowIf("isOffHand")]
        public float firstDelay;
        
        [ShowIf("isOffHand")]
        public float interval;
        
        /// <summary>
        /// 屏蔽命中事件
        /// </summary>
        public bool disableHitEvent;
        
        /// <summary>
        /// 仅检测
        /// </summary>
        public bool onlyHitCheck;
        
        /// <summary>
        /// 伤害设定
        /// </summary>
        [ShowIf("onlyHitCheck")]
        public DamageSetting damageSetting;
    }
}