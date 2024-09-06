using System;
using Hono.Scripts.Battle.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    public class ActorModelHandle : MonoBehaviour
    {
        [ReadOnly] public int ActorUid;

        [Button]
        public void UseSkill(int skillId = 10001)
        {
            BattleEventManager.Instance.TriggerEvent(EBattleEventType.UseSkill,
                new SkillUsedEventInfo() { SkillId = skillId, ActorUid = ActorUid });
        }
    }
}