#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core
{
    public partial class CombatComp
    {
        /// <summary>
        /// 释放技能后会产生这样一个快照对象
        /// </summary>
        private class SkillDriver
        {
            public CombatComp CombatComp { get; }

            /// <summary>
            /// 每个技能的修改器
            /// </summary>
            private Dictionary<int, Skill> _skills;

            /// <summary>
            /// 当前正在运行的唯一技能
            /// </summary>
            private Skill _curExclusiveSkill;

            ///技能流程 -> UseSkill -> 技能选择目标(如果是玩家操控，打开指示器等待指示器回调，如果是Npc使用AI先判定hate目标是否符合该技能，不符合则根据tag或其他条件选目标) ->
            /// <summary>
            /// 技能需要实时更新的东西，cd，资源检测，
            /// </summary>
            /// <param name="combatComp"></param>
            public SkillDriver(CombatComp combatComp)
            {
                CombatComp = combatComp;
            }

            public void Tick(float dt)
            {
                
            }

            public void Clear()
            {
                foreach (KeyValuePair<int, Skill> skill in _skills)
                {
                    GPool<Skill>.Pool.Recycle(skill.Value);
                }

                _skills.Clear();
            }

            /// <summary>
            /// 学习技能
            /// </summary>
            public void LearnSkill(int skillId)
            {
                if (!_skills.ContainsKey(skillId))
                {
                    Debug.LogError($"重复学习技能 {skillId}");
                    return;
                }
                
                if(!AssetManager.Instance.TryGetData<SkillData>(skillId, out var skillData))
                {
                    Debug.LogError($"找不到对应的技能 {skillId} 的配置");
                    return;
                }
                
                Skill skill = GPool<Skill>.Pool.Rent();
                var ability = CombatComp.Unit.AddAbility(skillData.abilityId);
                skill.OnRent(ability, skillData);
                _skills.Add(skillId, skill);
            }

            /// <summary>
            /// 释放技能
            /// </summary>
            /// <param name="skillId"></param>
            public void UseSkill(int skillId)
            {
                if (!_skills.TryGetValue(skillId, out var skill))
                {
                    Debug.LogError($"Unit:{CombatComp.Unit} 未学会该技能");
                    return;
                }

                if (skill.IsExecuting)
                {
                    skill
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
        }
    }
}