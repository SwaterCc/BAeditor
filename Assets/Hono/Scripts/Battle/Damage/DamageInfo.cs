#region

#endregion

namespace Hono.Scripts.Battle
{
    public class DamageInfo
    {
        /// <summary>
        /// 来源ability类型
        /// </summary>
        public EAbilityType SourceAbilityType;

        /// <summary>
        /// 来源abilityID
        /// </summary>
        public int SourceAbilityConfigId;

        /// <summary>
        /// 一共命中了几个目标
        /// </summary>
        public int HitCount;

        /// <summary>
        /// 这是第几次命中
        /// </summary>
        public int HitNumberCount;

        /// <summary>
        /// 是否必定暴击
        /// </summary>
        public bool IsCriticalOnce;

        public void Clear()
        {
            SourceAbilityType = EAbilityType.Skill;
            SourceAbilityConfigId = 0;
            HitCount = 0;
            HitNumberCount = 0;
            IsCriticalOnce = false;
        }
    }
}