using System;
using Sirenix.OdinInspector;


namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击点设置(即时打击)
    /// </summary>
    [Serializable]
    public class HitParams
    {
        [LabelText("是否为Aoe")]
        public bool isAOE;

        [LabelText("AOE设置")]
        [ShowIf("isAOE")]
        public RangeFilterSetting aoeSetting = new();

        [LabelText("禁用命中事件发送")]
        public bool disableHitEvent;

        [LabelText("伤害Id，找不到伤害id仅会产生一次命中检测")]
        public int damageConfigId;

        [LabelText("本次伤害一定会暴击")]
        public bool criticalFlag;
    }

    /// <summary>
    /// 打击盒子配置(脱手打击，会产生一个对象)
    /// </summary>
    [Serializable]
    public class HitBoxParams : HitParams
    {
        [LabelText("打击前延迟时间")]
        public float delayTime;

        [LabelText("打击检测间隔")]
        public float interval;

        [LabelText("打击检测次数")]
        public int hitNumber;
    }
}