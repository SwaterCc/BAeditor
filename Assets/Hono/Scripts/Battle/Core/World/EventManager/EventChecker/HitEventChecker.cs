#region

using System;
using Hono.Scripts.Battle.Base;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle.Event
{
    [Serializable]
    public class HitEventChecker : IEventChecker
    {
        /// <summary>
        /// 攻击者Uid
        /// </summary>
        public int attackerUid;

        /// <summary>
        /// 伤害来源类型
        /// </summary>
        public EDamageSourceType damageSourceType;

        /// <summary>
        /// 来源abilityId
        /// </summary>
        public int sourceAbilityId;

        /// <summary>
        /// 来源的伤害Id
        /// </summary>
        public int damageConfigId;

        public bool Check(in VariableBoard board)
        {
            bool res = true;

            if (attackerUid > 0)
            {
                res = res && board.Get<int>("AttackerUid") == attackerUid;
            }

            if (damageSourceType > 0)
            {
                res = res && board.Get<EDamageSourceType>("DamageSourceType") == damageSourceType;
            }
            
            if (sourceAbilityId > 0)
            {
                res = res && board.Get<int>("SourceAbilityId") == sourceAbilityId;
            }

            if (damageConfigId > 0)
            {
                res = res && board.Get<int>("DamageConfigId") == damageConfigId;
            }

            return res;
        }
    }
}