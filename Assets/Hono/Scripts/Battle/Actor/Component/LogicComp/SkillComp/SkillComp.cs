using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class SkillComp : ALogicComponent , IReloadHandle
        {
            //管理技能cd
            //管理技能释放前消耗检测
            //管理技能消耗资源
            //管理技能释放打断优先级(有问题需要讨论)
            //管理技能升级养成数据
            //技能的内核逻辑依靠Ability
            //Ability可通过该组件获取技能数据
            private Dictionary<int, Skill> _skills = new();
            public Dictionary<int, Skill> Skills => _skills;

            /// <summary>
            /// 当前在运行的技能
            /// </summary>
            private Skill _curSkill;

            private readonly UseSkillChecker _eventChecker;

            private Func<IntTable> _getSkills;

            public SkillComp(ActorLogic logic,Func<IntTable> getSkills) : base(logic) {
	            _eventChecker = new UseSkillChecker(EBattleEventType.UseSkill, ActorLogic.Actor, -1, UseSkillByEvent);
	            _getSkills = getSkills;
            }

            public override void Init()
            {
	            if (_getSkills == null) {
		            Debug.LogWarning($"Actor{Actor.Uid} 获取技能失败");
		            return;
	            }
	            
	            foreach (IntArray skillInfo in _getSkills.Invoke())
	            {
		            Skill skill = new(ActorLogic, skillInfo[0], skillInfo[1]);
		            _skills.Add(skill.Id, skill);
	            }
                
                BattleEventManager.Instance.Register(_eventChecker);
                AssetManager.Instance.AddReloadHandle(this);
            }

            public void Reload()
            {
	            Debug.Log($"Actor {ActorLogic.Uid} Reload SkillComp");
	            Clear();

	            if (_getSkills == null) {
		            Debug.LogWarning($"Actor{Actor.Uid} 获取技能失败");
		            return;
	            }
	            
                foreach (IntArray skillInfo in _getSkills.Invoke())
                {
                    Skill skill = new(ActorLogic, skillInfo[0], skillInfo[1]);
                    _skills.Add(skill.Id, skill);
                }
            }

            public void Clear()
            {
                foreach (var skill in _skills)
                {
                    skill.Value.Destroy();
                }

                _skills.Clear();
            }

            public override void UnInit()
            {
	            Clear();
                BattleEventManager.Instance.UnRegister(_eventChecker);
                AssetManager.Instance.RemoveReloadHandle(this);
            }

			public bool TryGetSkill(int skillId,out Skill skill) {
				return _skills.TryGetValue(skillId, out skill);
			}

            protected override void onTick(float dt)
            {
                foreach (var pSKill in _skills)
                {
                    pSKill.Value.OnTick(dt);
                }
            }

            public void LearnSkill(int skillId,int level) {
	            var skillCtrl = new Skill(ActorLogic, skillId, level);
	            if (_skills.TryAdd(skillCtrl.Id, skillCtrl)) {
		            Debug.LogWarning($"重复学习技能 {skillId}");
	            }
            }
            
            public void ForgetSkill(int skillId) {
	           
	            if (_skills.ContainsKey(skillId)) {
		            _skills[skillId].Destroy();
		            _skills.Remove(skillId);
	            }
            }

            public void UseSkillByEvent(IEventInfo eventInfo) {
	            UseSkill(((UsedSkillEventInfo)eventInfo).SkillId);
            }
            
            public void UseSkill(int skillId)
            {
                if (!_skills.TryGetValue(skillId, out var skillState))
                {
                    return;
                }

                if (_curSkill != null && _curSkill > skillState)
                {
                    return;
                }

                if (skillState.IsEnable)
                {
                    ActorLogic._stateMachine.ChangeState(EActorState.Battle);
                    skillState.OnSkillUsed();
                }
                else
                {
                    skillState.SendFailedMsg();
                }
            }
        }
    }
}