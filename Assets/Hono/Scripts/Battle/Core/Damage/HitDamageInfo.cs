using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle
{
    public struct HitDamageInfo
    {
        /// <summary>
        /// 伤害来源Actor
        /// </summary>
        public int SourceActorId;

        /// <summary>
        /// 伤害来源Ability
        /// </summary>
        public int SourceAbilityId;

        /// <summary>
        /// 伤害Id
        /// </summary>
        public int DamageConfigId;

        /// <summary>
        /// 单次打击盒命中数量
        /// </summary>
        public int HitBoxHitCount;

        /// <summary>
        /// 命中目标的Uid;
        /// </summary>
        public int HitTargetUid;

        /// <summary>
        /// 最终伤害
        /// </summary>
        public int FinalDamageValue;

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCritical;
        
        /// <summary>
        /// 伤害是否免疫
        /// </summary>
        public bool IsImmunity;

        /// <summary>
        /// 是否杀死目标
        /// </summary>
        public bool IsKillTarget;
    }
}