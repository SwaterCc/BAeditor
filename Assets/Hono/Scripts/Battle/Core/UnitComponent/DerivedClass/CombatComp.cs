using System;
using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{

    [JsonUnitCompCtorParams(typeof(CombatComp))]
    public class CombatCompCtorParams : UnitCompCtorParams
    {
        [JsonUnitCompCtorParam("默认拥有的技能列表")]
        public List<int> SkillList = new();
        [JsonUnitCompCtorParam("默认拥有的战斗资源")]
        public List<int> CombatEnergyIds = new();
    }
    
    /// <summary>
    /// 战斗组件
    /// </summary>
    [JsonUnitComponent]
    public partial class CombatComp : UnitComponent, IGPoolObject
    {
        /// <summary>
        /// 技能列表
        /// </summary>
        private readonly Dictionary<int, Skill> _skills;

        /// <summary>
        /// 当前正在运行的唯一技能
        /// </summary>
        private Skill _curExclusiveSkill;

        /// <summary>
        /// 战斗资源管理
        /// </summary>
        private readonly CombatEnergyCtrl _energyCtrl;

        public CombatComp()
        {
            _skills = new Dictionary<int, Skill>();
            _energyCtrl = new CombatEnergyCtrl(this);
        }

        public override void Ctor(UnitCompCtorParams ctorParams)
        {
            var combatCtorParams = (CombatCompCtorParams)ctorParams;
            
            foreach (var skillId in combatCtorParams.SkillList)
            {
                LearnSkill(skillId);
            }

            foreach (var energyId in combatCtorParams.CombatEnergyIds)
            {
                AddEnergyType(energyId);
            }
        }

        public override void Init()
        {
            _energyCtrl.Init();
        }

        protected override void onTick(float dt)
        {
            foreach (var skill in _skills.Values)
            {
                skill.OnTick(dt);
            }

            _energyCtrl.Tick(dt);
        }

        public override void Recycle()
        {
            GPool<CombatComp>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            foreach (var skill in _skills.Values)
            {
                GPool<Skill>.Pool.Recycle(skill);
            }

            _skills.Clear();
            _energyCtrl.Clear();
        }

        public void LearnSkill(int skillId)
        {
            if (_skills.ContainsKey(skillId))
            {
                Debug.LogError($"重复学习技能 {skillId}");
                return;
            }

            if (!AssetManager.Instance.TryGetData<SkillData>(skillId, out var skillData))
            {
                Debug.LogError($"找不到对应的技能 {skillId} 的配置");
                return;
            }

            Skill skill = GPool<Skill>.Pool.Rent();
            skill.OnRent(this, skillData);
            _skills.Add(skillId, skill);

            if (skill.SkillData.skillType == ESkillType.PassiveSkill)
            {
                skill.Play();
            }
        }

        /// <summary>
        /// 忘记技能
        /// </summary>
        /// <param name="skillId"></param>
        public void ForgetSkill(int skillId)
        {
            if (_skills.Remove(skillId, out Skill skill))
            {
                GPool<Skill>.Pool.Recycle(skill);
            }
        }

        public bool TryGetSkillModifier(int skillId, out SkillModifier skillModifier)
        {
            skillModifier = null;
            if (_skills.TryGetValue(skillId, out var skill))
            {
                skillModifier = skill.Modifier;
                return true;
            }

            return false;
        }

        public SkillModifier GetSkillModifier(int skillId)
        {
            if (_skills.TryGetValue(skillId, out var skill))
            {
                return skill.Modifier;
            }

            return null;
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public void UseSkill(int skillId)
        {
            if (!_skills.TryGetValue(skillId, out var skill))
            {
                Debug.LogError($"Unit:{Unit} 未学会该技能");
                return;
            }

            //技能为独占技能且当前已经有在运行的独占技能
            if (skill.SkillData.isExclusive && _curExclusiveSkill != null)
            {
                return;
            }

            //技能是否正在运行中或者被禁用
            if (skill.Flag != 0)
            {
                return;
            }

            if (Unit.Uid == World.Current.PlayerCtrlUnit.Uid)
            {
                //玩家操控，创建技能筛选器
                SkillIndicator.Instance.Show(skill);
            }
            else
            {
                SimpleSkillAICtrl.Instance.UseSkill(Unit, skill);
            }
        }

        /// <summary>
        /// 添加能量类型
        /// </summary>
        /// <param name="energyId"></param>
        public void AddEnergyType(int energyId)
        {
            _energyCtrl.AddEnergyType(energyId);
        }

        /// <summary>
        /// 删除能量类型
        /// </summary>
        /// <param name="energyId"></param>
        public void RemoveEnergyType(int energyId)
        {
            _energyCtrl.RemoveEnergyType(energyId);
        }

        /// <summary>
        /// 资源检测
        /// </summary>
        private bool checkSkillResourceEnough(Skill skill)
        {
            var skillData = skill.SkillData;

            if (skillData.skillResCheck.Count == 0)
            {
                return true;
            }

            foreach (var resItem in skillData.skillResCheck)
            {
                switch (resItem.resourceType)
                {
                    case EBattleResourceType.Energy:
                        var modifier = skill.Modifier;
                        var energyType = resItem.param1;
                        var energyCostValue = resItem.param2;
                        var realValue = (energyCostValue + modifier.GetEnergyAdd(energyType)) *
                                        (int)(1 + modifier.GetEnergyPCT(energyType) / 10000f);
                        if (!_energyCtrl.CheckEnergyEnough(energyType, realValue))
                        {
                            return false;
                        }

                        break;
                    case EBattleResourceType.Buff:
                        var buffId = resItem.param1;
                        var layerCount = resItem.param2;
                        if (!Unit.TryGetComponent(out BuffComp buffComp) ||
                            buffComp.GetBuffLayer(buffId, Unit.Uid) < layerCount)
                        {
                            return false;
                        }

                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// 资源消耗
        /// </summary>
        private void costEnergy(Skill skill)
        {
            var skillData = skill.SkillData;
            foreach (var resItem in skillData.skillResCost)
            {
                //目前只有能量消耗流程
                if (resItem.resourceType == EBattleResourceType.Energy)
                {
                    var modifier = skill.Modifier;
                    var energyType = resItem.param1;
                    var energyCostValue = resItem.param2;
                    var realValue = (energyCostValue + modifier.GetEnergyAdd(energyType)) *
                                    (int)(1 + modifier.GetEnergyPCT(energyType) / 10000f);
                    _energyCtrl.CostEnergy(energyType, realValue);
                }
            }
        }
    }
}