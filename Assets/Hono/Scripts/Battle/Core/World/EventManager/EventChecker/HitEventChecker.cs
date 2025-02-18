#region

using System;
using Hono.Scripts.Battle.Base;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class HitEventChecker : IEventChecker, IGPoolObject
    {
        /// <summary>
        /// 攻击者Uid
        /// </summary>
        private int _attackerUid;

        /// <summary>
        /// 伤害来源类型
        /// </summary>
        private EDamageSourceType _damageSourceType;

        /// <summary>
        /// 来源abilityId
        /// </summary>
        private int _sourceAbilityId;

        /// <summary>
        /// 来源的伤害Id
        /// </summary>
        private int _damageConfigId;

        public HitEventChecker() { }

        public HitEventChecker(int attackerUid, EDamageSourceType damageSourceType, int sourceAbilityId, int damageConfigId)
        {
            _attackerUid = attackerUid;
            _damageSourceType = damageSourceType;
            _sourceAbilityId = sourceAbilityId;
            _damageConfigId = damageConfigId;
        }
        
        public void OnRent(int attackerUid, EDamageSourceType damageSourceType, int sourceAbilityId, int damageConfigId)
        {
            _attackerUid = attackerUid;
            _damageSourceType = damageSourceType;
            _sourceAbilityId = sourceAbilityId;
            _damageConfigId = damageConfigId;
        }

        public bool Check(in VariableBoard board)
        {
            bool res = true;

            if (_attackerUid > 0)
            {
                res = res && board.Get<int>("AttackerUid") == _attackerUid;
            }

            if (_damageSourceType > 0)
            {
                res = res && board.Get<EDamageSourceType>("DamageSourceType") == _damageSourceType;
            }

            if (_sourceAbilityId > 0)
            {
                res = res && board.Get<int>("SourceAbilityId") == _sourceAbilityId;
            }

            if (_damageConfigId > 0)
            {
                res = res && board.Get<int>("DamageConfigId") == _damageConfigId;
            }

            return res;
        }

        public void OnRecycle()
        {
            _attackerUid = 0;
            _damageSourceType = 0;
            _sourceAbilityId = 0;
            _damageConfigId = 0;
        }
    }
}