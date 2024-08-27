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

        public float HP;

        public float MP;

        public float Attack;


        private Actor _actor;

        public List<int> AbilityUid = new List<int>();

        public void OnEnable() { }

        private void Update()
        {
            /*if (DebugObj != null)
            {
                _handle ??= DebugObj.Actor.DebugHandle;
                _actor ??= DebugObj.Actor;
            }

            if (_actor != null)
            {
                var hpAttr = (SimpleAttribute<float>)_actor.GetAttrCollection().GetAttr(EAttributeType.Hp);
                HP = hpAttr.GetValue();

                var mpAttr = (SimpleAttribute<float>)_actor.GetAttrCollection().GetAttr(EAttributeType.Mp);
                MP = mpAttr.GetValue();

                var atkAttr = (SimpleAttribute<float>)_actor.GetAttrCollection().GetAttr(EAttributeType.Attack);
                Attack = atkAttr.GetValue();
            }*/
        }

        [Button("学能力")]
        public void Award(int abilityId = 1231, bool isRunNow = true)
        {
            /*var ability = new Ability(abilityId);
            AbilityUid.Add(ability.Uid);
            _actor.AwardAbility(ability, isRunNow);*/
        }

        [Button("运行能力")]
        public void ExecuteAbility(int abilityId)
        {
            // _actor.GetAbilityController().ExecutingAbility(abilityId);
        }
    }
}