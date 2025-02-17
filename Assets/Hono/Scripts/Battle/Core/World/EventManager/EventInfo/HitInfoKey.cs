using Hono.Scripts.Battle.ObjectPool;

namespace Hono.Scripts.Battle.Event
{
    public static class HitInfoKey
    {
        /// <summary>
        /// 伤害来源Actor
        /// </summary>
        public static readonly EvtInfoField<int> AttackerUid = new();

        /// <summary>
        /// 伤害来源类型
        /// </summary>
        public static readonly EvtInfoField<EDamageSourceType> DamageSourceType = new();
        
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
    }
}