using System;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle.Core
{
    [Serializable]
    public class DamageSetting
    {
        [LabelText("伤害Id")]
        public int DamageConfigId;
        
        [LabelText("必定暴击标记")]
        public bool CriticalFlag;
    }
}