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
            _attacker.ActorLogic.AwardAbility(abilityId, false);
        }

        [Button("添加Tag")]
        public void addTag(int tag)
        {
            _attacker.ActorLogic.AddTag(tag);
        }

        /*[Button("运行能力")]
        public void ExecuteAbility(int abilityId) {
            // _actorLogic.GetAbilityController().ExecutingAbility(abilityId);
        }*/

        private DamageLuaInterface _luaInterface = new DamageLuaInterface();

        [Button("打击点测试")]
        public void HitTest()
        {
            _luaInterface.Init();

            _attacker.ActorLogic.SetAttr(ELogicAttr.Attack, 500, false);
            _attacker.ActorLogic.SetAttr(ELogicAttr.Hp, 100, false);
            _attacker.ActorLogic.SetAttr(ELogicAttr.Mp, 100, false);

            /*
            var damageInfo = new DamageInfo();
            damageInfo.DamageConfigId = 1;
            damageInfo.SourceId = 1001;
            damageInfo.SourceType = EAbilityType.Skill;
            damageInfo.HitCount = 5;

            var damageConfig = new DamageItem();
            damageConfig.FormulaName = "normalAttack";
            damageConfig.AddiFuncIds.Add(1);
            damageConfig.AddiFuncParam.Add(new List<int>() { 1001, 30 });
            */

            var res = _luaInterface.GetDamageResults(_attacker.ActorLogic, _target.ActorLogic, DamageInfo,
                DamageConfig);
            if (res != null)
                Debug.Log($"最终伤害 = {res.DamageValue}");
            _luaInterface.Dispose();
        }
    }
}