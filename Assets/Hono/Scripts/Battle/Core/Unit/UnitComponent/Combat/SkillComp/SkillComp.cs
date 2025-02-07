#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle
{
    public class SkillComp : UnitComponent, IReloadHandle
    {
        /// <summary>
        /// 技能列表
        /// </summary>
        public Dictionary<int, Skill> Skills { get; } = new();

        public override void Init()
        {
            AssetManager.Instance.AddReloadHandle(this);
        }

        /// <summary>
        /// Debug模式下的函数，不用考虑GC问题
        /// </summary>
        public void Reload()
        {
            Debug.Log($"Actor {Unit.Uid} Reload SkillComp");

            List<(int, int)> skillList = new(Skills.Count);

            foreach (KeyValuePair<int, Skill> pSkill in Skills)
            {
                skillList.Add((pSkill.Value.Id, pSkill.Value.Level));
            }

            foreach (KeyValuePair<int, Skill> skill in Skills)
            {
                GPool<Skill>.Pool.Recycle(skill.Value);
            }

            Skills.Clear();

            foreach ((int, int) skillInfo in skillList)
            {
                LearnSkill(skillInfo.Item1, skillInfo.Item2);
            }
        }

        protected override void onClear()
        {
            //_eventChecker.UnRegister();
            AssetManager.Instance.RemoveReloadHandle(this);

            foreach (KeyValuePair<int, Skill> skill in Skills)
            {
                GPool<Skill>.Pool.Recycle(skill.Value);
            }

            Skills.Clear();
        }

        /// <summary>
        /// 尝试获取技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool TryGetSkill(int skillId, out Skill skill)
        {
            return Skills.TryGetValue(skillId, out skill);
        }

        protected override void onTick(float dt)
        {
            foreach (var pSKill in Skills)
            {
                pSKill.Value.OnTick(dt);
            }
        }

        /// <summary>
        /// 学习技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="level"></param>
        public void LearnSkill(int skillId, int level)
        {
            Skill skill = GPool<Skill>.Pool.Rent();
            skill.OnRent(Unit, skillId, level);
            if (!Skills.TryAdd(skillId, skill))
            {
                Debug.LogWarning($"重复学习技能 {skillId}");
            }
        }

        /// <summary>
        /// 忘记技能
        /// </summary>
        /// <param name="skillId"></param>
        public void ForgetSkill(int skillId)
        {
            if (Skills.Remove(skillId, out Skill skill))
            {
                GPool<Skill>.Pool.Recycle(skill);
            }
        }

        /*private void UseSkillByEvent(IEventInfo eventInfo)
        {
            UsedSkillEventInfo skillInfo = (UsedSkillEventInfo)eventInfo;

            if (skillInfo.IsPlayerControl)
            {
                if (_curSkill is { IsExecuting: true })
                {
                    _curSkill.Ability.Stop();
                }

                PlayerUseSkill(skillInfo.SkillId);
            }
            else
            {
                TryUseSkill(skillInfo.SkillId);
            }
        }*/

        /// <summary>
        /// skillComp需要拆分，怪物释放技能，玩家释放技能两者可能会有差异
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool PlayerUseSkill(int skillId)
        {
            if (!Skills.TryGetValue(skillId, out var skill))
            {
                return false;
            }

            if (Unit.GetAttr(EAttrType.AttrStunned) > 0)
            {
                return false;
            }

            if (skill.TryUseSkill())
            {
                _curSkill = skill;
                return true;
            }

            return false;
        }

        public bool TryUseSkill(int skillId)
        {
            //Debug.Log($"[UseSkill] Actor{Actor.Uid} -->尝试执行技能 {skillId}");

            if (!Skills.TryGetValue(skillId, out var skill))
            {
                return false;
            }

            if (Unit.GetAttr(EAttrType.AttrStunned) > 0)
            {
                return false;
            }

            if (skill.TryUseSkill())
            {
                _curSkill = skill;
                return true;
            }

            //skill.SendFailedMsg();
            return false;
        }
    }
}