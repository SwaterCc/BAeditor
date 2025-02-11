using System;
using Sirenix.OdinInspector;


namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击点设置
    /// </summary>
    [Serializable]
    public abstract class HitParams
    {
        [LabelText("是否为Aoe")]
        public bool isAOE;
        
        [LabelText("AOE设置")]
        [ShowIf("isAOE")]
        public RangeFilterSetting aoeSetting = new();
        
        [LabelText("禁用命中事件发送")]
        public bool disableHitEvent;
        
        [LabelText("伤害Id，找不到伤害id仅会产生一次m检测")]
        public int damageConfigId;
        
        [LabelText("本次伤害一定会暴击")]
        public bool criticalFlag;
    }
}