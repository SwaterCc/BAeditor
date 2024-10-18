namespace Hono.Scripts.Battle
{
    public class DamageResults
    {
        /// <summary>
        /// 伤害最终值
        /// </summary>
        public float DamageValue;
        
        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCritical;

        /// <summary>
        /// 最终冲击力
        /// </summary>
        public float ImpactValue;

		/// <summary>
		/// 伤害类型
		/// </summary>
		public EDamageType DamageType;


		public DamageResults(float damageValue,bool isCritical,float finalImpactValue, EDamageType damageType) {
	        DamageValue = damageValue;
	        IsCritical = isCritical;
	        ImpactValue = finalImpactValue;
			DamageType = damageType;

		}
    }
}