using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    /// <summary>
    /// 战斗编辑器Debug组件
    /// </summary>
    public class AbilityDebugInspector : MonoBehaviour
    {
        [ShowInInspector] public DamageItem DamageConfig;

        [ShowInInspector] public DamageInfo DamageInfo;

        Actor _attacker;
        Actor _target;

        public void Start()
        {
            _attacker = ActorManager.Instance.CreateActor(1);
            _target = ActorManager.Instance.CreateActor(2);
        }

        [Button("学能力")]
        public void Award(int abilityId, bool isRunNow = false)
        {
            _attacker.Logic.AwardAbility(abilityId, false);
        }

        [Button("添加Tag")]
        public void addTag(int tag)
        {
            _attacker.Logic.AddTag(tag);
        }

        /*[Button("运行能力")]
        public void ExecuteAbility(int abilityId) {
            // _actorLogic.GetAbilityController().ExecutingAbility(abilityId);
        }*/
        
        [Button("打击点测试")]
        public void HitTest()
        {
           
        }
    }
}