#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle
{
    public class DamageInfo
    {
        public EAbilityType SourceAbilityType;

        public int SourceAbilityConfigId;

        public List<int> Tags = new List<int>();

        /// <summary>
        ///     一共命中了几个目标
        /// </summary>
        public int HitCount;

        /// <summary>
        ///     这是第几次命中
        /// </summary>
        public int HitNumberCount;

        /// <summary>
        ///     是否必定暴击
        /// </summary>
        public bool IsCriticalOnce;
    }
}