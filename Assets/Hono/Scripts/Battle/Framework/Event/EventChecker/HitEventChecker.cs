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
            /*if (board.Get<int>())
            {
                res = hitInfo.SourceActorId == _abilitySourceUid;
            }

            if (_damageConfigId > 0)
            {
                res = res && hitInfo.DamageConfigId == _damageConfigId;
            }*/
            return true;
        }
    }
}