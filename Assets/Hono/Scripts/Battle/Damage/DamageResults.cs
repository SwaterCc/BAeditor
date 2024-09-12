using System.Collections.Generic;

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
        /// 打中的对象列表
        /// </summary>
        public List<int> HitUids;

        /// <summary>
        /// 伤害类型
        /// </summary>
        public EDamageType DamageType;
        public DamageResults(float damageValue,bool isCritical,float finalImpactValue) {
	        DamageValue = damageValue;
	        IsCritical = isCritical;
	        ImpactValue = finalImpactValue;
        }
    }
}