using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Tools.DebugTools
{
    /// <summary>
    /// 战斗编辑器Debug组件
    /// </summary>
    public class AbilityDebugInspector : MonoBehaviour
    {
        public ActorBehavior DebugObj;

        private Actor.ActorDebugHandle _handle;

        private Actor _actor;

        public List<int> AbilityUid = new List<int>();
        
        public void OnEnable()
        {
        }

        private void Update()
        {
            if (DebugObj != null)
            {
                _handle ??= DebugObj.Actor.DebugHandle;
                _actor ??= DebugObj.Actor;
            }
        }

        [Button("学能力")]
        public void Award(int abilityId, bool isRunNow)
        {
            var ability = new Ability(abilityId);
            AbilityUid.Add(ability.Uid);
            _actor.AwardAbility(ability, isRunNow);
        }

        [Button("运行能力")]
        public void ExecuteAbility(int abilityId)
        {
            _actor.GetAbilityController().ExecutingAbility(abilityId);
        }
    }
}