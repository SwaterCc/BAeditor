using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击点设置
    /// </summary>
    [Serializable]
    public abstract class HitSetting
    {
        [Tooltip("脱手打击盒会保存盒子创建时的属性快照，即时攻击者已经死亡仍然可以造成伤害")]
        [LabelText("是否为脱手打击盒")]
        public bool isOffHand;

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
        [HideIf("onlyHitCheck")]
        public DamageSetting damageSetting;
    }

    [Serializable]
    public class SingleHitSetting : HitSetting { }


    /// <summary>
    /// AOE打击数据
    /// </summary>
    [Serializable]
    public class AoeHitSetting : HitSetting
    {
        [LabelText("Aoe筛选配置")]
        public RangeFilterSetting rangeFilterSetting = new();
    }
}