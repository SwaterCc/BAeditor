using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public class CombatCompCtorParams : ComponentCtorParams
    {
        /// <summary>
        /// 允许产生异常伤害效果
        /// </summary>
        public bool AllowElementEffect;

        /// <summary>
        /// 禁用buff
        /// </summary>
        public bool DisableBuffAdd;
    }

    /// <summary>
    /// 战斗组件
    /// </summary>
    public partial class CombatComp : UnitComponent
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
        /// buff控制器
        /// </summary>
        private readonly BuffDriver _buffDriver;

        /// <summary>
        /// 战斗资源管理
        /// </summary>
        private readonly CombatEnergyCtrl _energyCtrl;

        /// <summary>
        /// 元素异常关系链
        /// </summary>
        private readonly static List<ElementLink> ElementLinks = new()
        {
            new(EAttrType.AttrWaterElementStackAdd, EAttrType.AttrWaterElementHp, EAttrType.AttrWaterElementMaxHp,
                1),
            new(EAttrType.AttrFireElementStackAdd, EAttrType.AttrFireElementHp, EAttrType.AttrFireElementMaxHp, 203201),
            new(EAttrType.AttrWindElementStackAdd, EAttrType.AttrWindElementHp, EAttrType.AttrWindElementMaxHp, 1),
            new(EAttrType.AttrRockElementStackAdd, EAttrType.AttrRockElementHp, EAttrType.AttrRockElementMaxHp, 1),
            new(EAttrType.AttrLightElementStackAdd, EAttrType.AttrLightElementHp, EAttrType.AttrLightElementMaxHp,
                1),
            new(EAttrType.AttrDarkElementStackAdd, EAttrType.AttrDarkElementHp, EAttrType.AttrDarkElementMaxHp, 1),
        };


        public CombatComp(ComponentCtorParams ctorParams) : base(ctorParams)
        {
            var combatParams = (ComponentCtorParams)ctorParams;


            _skills = new Dictionary<int, Skill>();
            _buffDriver = new BuffDriver(this);
            _energyCtrl = new CombatEnergyCtrl(Unit);
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

            _buffDriver.Tick(dt);
            _energyCtrl.Tick(dt);
        }

        protected override void onClear()
        {
            foreach (var skill in _skills.Values)
            {
                GPool<Skill>.Pool.Recycle(skill);
            }

            _skills.Clear();
            _buffDriver.Clear();
            _energyCtrl.Clear();
        }

        #region Skill

        public void LearnSkill(int skillId)
        {
            if (!_skills.ContainsKey(skillId))
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
                        if (_buffDriver.GetBuffLayer(buffId, Unit.Uid) < layerCount)
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

        #endregion

        #region Buff

        public void AddBuff(int buffId, int sourceUnitUid, int buffLayer = 1)
        {
            if (!AssetManager.Instance.TryGetData<BuffData>(buffId, out var buffData))
            {
                Debug.LogError($"找不到指定Buff:{buffId}数据");
                return;
            }

            _buffDriver.AddBuff(sourceUnitUid, buffId, buffLayer, buffData);
        }

        public int GetBuffLayer(int buffId, int sourceUnitUid = -1)
        {
            return _buffDriver.GetBuffLayer(sourceUnitUid, buffId);
        }

        public bool HasBuff(int buffId, int sourceUnitUid = -1)
        {
            return _buffDriver.HasBuff(sourceUnitUid, buffId);
        }

        public void RemoveBuff(int buffId, int sourceUnitUid)
        {
            _buffDriver.RemoveBuff(sourceUnitUid, buffId);
        }

        #endregion

        #region ElementEffect

        public void CumulativeElementValue(Unit attacker, DamageTable.DamageRow damageRow)
        {
            //无元素类型不处理
            if (damageRow.ElementsDamage.Count != 2)
            {
                return;
            }

            foreach (var link in ElementLinks)
            {
                link.Process(attacker, this, damageRow);
            }
        }

        #endregion
    }
}