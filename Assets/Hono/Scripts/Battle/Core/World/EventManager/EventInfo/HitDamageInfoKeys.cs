using System.Collections.Generic;
using Hono.Scripts.Battle.ObjectPool;

namespace Hono.Scripts.Battle.Event
{
    public static class HitDamageInfoKeys
    {
        /// <summary>
        /// 伤害来源Actor
        /// </summary>
        public static readonly EvtInfoField<int> AttackerUid = new();

        /// <summary>
        /// 伤害来源Ability
        /// </summary>
        public static readonly EvtInfoField<int> SourceAbilityId = new();

        /// <summary>
        /// 伤害Id
        /// </summary>
        public static readonly EvtInfoField<int> DamageConfigId = new();

        /// <summary>
        /// 单次打击盒命中数量
        /// </summary>
        public static readonly EvtInfoField<int> HitBoxHitCount = new();

        /// <summary>
        /// 命中目标的Uid;
        /// </summary>
        public static readonly EvtInfoField<GList<int>> HitTargetUid = new();

        /// <summary>
        /// 最终伤害
        /// </summary>
        public static readonly EvtInfoField<int> FinalDamageValue = new();

        /// <summary>
        /// 是否暴击
        /// </summary>
        public static readonly EvtInfoField<bool> IsCritical = new();

        /// <summary>
        /// 伤害是否免疫
        /// </summary>
        public static readonly EvtInfoField<bool> IsImmunity = new();

        /// <summary>
        /// 是否杀死目标
        /// </summary>
        public static readonly EvtInfoField<bool> IsKillTarget = new();
    }
}