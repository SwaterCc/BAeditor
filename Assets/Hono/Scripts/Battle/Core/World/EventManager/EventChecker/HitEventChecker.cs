#region

using System;
using Hono.Scripts.Battle.Base;

#endregion

namespace Hono.Scripts.Battle.Event
{
    [Serializable]
    public class HitEventChecker : IEventChecker
    {
        /// <summary>
        /// 来源abilityId
        /// </summary>
        public int abilityId;

        /// <summary>
        /// 来源的伤害Id
        /// </summary>
        public int damageConfigId;

        public bool Check(in VariableBoard board)
        {
            bool res = true;
            if (abilityId > 0)
            {
                res = board.Get(HitDamageInfoKeys.SourceAbilityId) == abilityId;
            }

            if (damageConfigId > 0)
            {
                res = res && board.Get(HitDamageInfoKeys.DamageConfigId) == damageConfigId;
            }

            return res;
        }
    }
}