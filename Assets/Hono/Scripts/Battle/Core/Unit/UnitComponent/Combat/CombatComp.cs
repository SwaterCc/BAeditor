using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 战斗组件
    /// </summary>
    public partial class CombatComp : UnitComponent
    {
        /// <summary>
        /// 每个技能的修改器
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

        public CombatComp()
        {
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
            _buffDriver.Tick(dt);
            _energyCtrl.Tick(dt);
        }

        protected override void onClear()
        {
            foreach (var skill in _skills)
            {
                GPool<Skill>.Pool.Recycle(skill.Value);
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

        /// <summary>
        /// 使用技能
        /// </summary>
        public bool TryUseSkill(int skillId, out Skill skill)
        {
            if (!_skills.TryGetValue(skillId, out skill))
            {
                Debug.LogError($"Unit:{Unit} 未学会该技能");
                return false;
            }

            if (skill.IsExecuting)
            {
                return false;
            }

            if (skill.SkillData.isExclusive && _curExclusiveSkill != null)
            {
                return false;
            }

            if (!checkSkillResourceEnough(skill.SkillData))
            {
                return false;
            }

            if (skill.SkillData) { }


            return true;
        }

        /// <summary>
        /// 资源检测
        /// </summary>
        private bool checkSkillResourceEnough(SkillData skillData)
        {
            if (skillData.skillResCheck.Count == 0)
            {
                return true;
            }

            bool _resEnough = false;
            foreach (var resItems in skillData.skillResCheck)
            {
                foreach (var resItem in resItems.Items)
                {
                    bool flag = true;
                    switch (resItem.ResourceType)
                    {
                        case EBattleResourceType.Energy:
                            flag = _energyCtrl.CheckEnergyEnough(resItem.ResId, resItem.Value);
                            break;
                        case EBattleResourceType.Buff:

                            break;
                        case EBattleResourceType.Hp:
                            break;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 资源消耗
        /// </summary>
        private void costResource(SkillData skillData)
        {
            foreach (var resItems in skillData.skillResCost)
            {
                //组检测
                foreach (var resItem in resItems.Items)
                {
                    //单项检测
                    switch (resItem.ResourceType)
                    {
                        case EBattleResourceType.Energy:
                            var mp = Unit.GetAttr((EAttrType)resItem.ResId);
                            Unit.SetAttr((EAttrType)resItem.ResId, mp - resItem.Value, false);
                            break;
                        case EBattleResourceType.Hp:
                            break;
                        case EBattleResourceType.Buff:
                            break;
                    }
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

        public void RemoveBuff(int buffId, int sourceUnitUid)
        {
            _buffDriver.RemoveBuff(sourceUnitUid, buffId);
        }

        #endregion
    }
}