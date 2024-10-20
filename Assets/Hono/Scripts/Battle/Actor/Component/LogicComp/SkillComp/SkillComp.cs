using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class SkillComp : ALogicComponent {
			
			private readonly Dictionary<int, Skill> _skills = new();
			public Dictionary<int, Skill> Skills => _skills;
			
			private readonly UseSkillChecker _eventChecker;
			
			public SkillComp(ActorLogic logic) : base(logic) {
				_eventChecker = new UseSkillChecker(EBattleEventType.UseSkill, ActorLogic.Actor, -1, UseSkillByEvent);
			}

			public override void Init() {
				BattleEventManager.Instance.Register(_eventChecker);
			}

			public override void UnInit() {
				Clear();
				BattleEventManager.Instance.UnRegister(_eventChecker);
			}

			public void Clear() {
				foreach (var skill in _skills) {
					skill.Value.Destroy();
				}

				_skills.Clear();
			}

			public bool TryGetSkill(int skillId, out Skill skill) {
				return _skills.TryGetValue(skillId, out skill);
			}

			protected override void onTick(float dt) {
				foreach (var pSKill in _skills) {
					pSKill.Value.OnTick(dt);
				}
			}

			public void LearnSkill(int skillId, int level) {
				var skillCtrl = new Skill(ActorLogic, skillId, level);
				if (!_skills.TryAdd(skillCtrl.Id, skillCtrl)) {
					Debug.LogWarning($"重复学习技能 {skillId}");
				}
			}

			public void ForgetSkill(int skillId) {
				if (_skills.ContainsKey(skillId)) {
					_skills[skillId].Destroy();
					_skills.Remove(skillId);
				}
			}

			private void UseSkillByEvent(IEventInfo eventInfo) {
				TryUseSkill(((UsedSkillEventInfo)eventInfo).SkillId);
			}

			public bool TryUseSkill(int skillId) {
				Debug.Log($"[UseSkill] Actor{Actor.Uid} -->尝试执行技能 {skillId}");

				if (!_skills.TryGetValue(skillId, out var skill)) {
					return false;
				}
				
				if (Actor.Logic.CurState() != EActorLogicStateType.Idle &&
				    Actor.Logic.CurState() != EActorLogicStateType.Move) {
					return false;
				}

				if (skill.TryUseSkill()) {
					return true;
				}

				skill.SendFailedMsg();
				return false;
			}
		}
	}
}