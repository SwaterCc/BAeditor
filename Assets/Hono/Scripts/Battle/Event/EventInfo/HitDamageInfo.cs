using System.Reflection;

namespace Hono.Scripts.Battle.Event {

	public class HitInfo : IEventInfo {
		/// <summary>
		/// 伤害来源Actor
		/// </summary>
		public int SourceActorId;

		/// <summary>
		/// 伤害来源AbilityUid(现在UID就是ConfigId)
		/// </summary>
		public int SourceAbilityUId;

		/// <summary>
		/// 伤害来源AbilityConfig
		/// </summary>
		public int SourceAbilityConfigId;

		/// <summary>
		/// 单次打击盒命中数量
		/// </summary>
		public int HitBoxHitCount;

		/// <summary>
		/// 伤害Id
		/// </summary>
		public int DamageConfigId;

		/// <summary>
		/// 属于哪个能力类型
		/// </summary>
		public int SourceAbilityType;
	}
	
	
    public class HitDamageInfo : HitInfo
    {
	    /// <summary>
	    /// 命中目标的Uid;
	    /// </summary>
	    public int HitTargetUid;
	    
        /// <summary>
        /// 最终伤害
        /// </summary>
        public float FinalDamageValue;

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCritical;

        /// <summary>
        /// 最终冲击力
        /// </summary>
        public float ImpactValue;

        /// <summary>
        /// 伤害是否免疫
        /// </summary>
        public bool IsImmunity;

        /// <summary>
        /// 是否杀死目标
        /// </summary>
        public bool IsKillTarget;

        public HitDamageInfo(){}
        
        public HitDamageInfo(HitInfo hitInfo) {
	        base.SourceActorId = hitInfo.SourceActorId;
	        base.SourceAbilityUId = hitInfo.SourceAbilityUId;
	        base.SourceAbilityConfigId = hitInfo.SourceAbilityConfigId;
	        base.HitBoxHitCount = hitInfo.HitBoxHitCount;
	        base.DamageConfigId = hitInfo.DamageConfigId;
	        base.SourceAbilityType = hitInfo.SourceAbilityType;
        }
        
        public void ParseDamageResult(DamageResults results)
        {
            ImpactValue = results.ImpactValue;
            IsCritical = results.IsCritical;
            FinalDamageValue = (int)results.DamageValue;
        }
    }
}