namespace Hono.Scripts.Battle {
	/// <summary>
	/// 自动输入
	/// </summary>
	public class AutoInput : ActorInput {
		public AutoInput(ActorLogic logic) : base(logic) {
			
		}
		
		//遍历主动技能
		//如果有可以释放的则释放技能
		//如果没有则向仇恨目标移动

		protected virtual void AutoMove() {
			//移动向仇恨目标
			if(Logic.CurState() == EActorState.Battle) return;

			var hateTargetUid = Logic.Actor.GetAttr<int>(ELogicAttr.AttrHateTargetUid);
			
			if(hateTargetUid <= 0) return;

			if (ActorManager.Instance.TryGetActor(hateTargetUid, out var hateActor)) {
				
			}
			
		}

		protected virtual void AutoUseSkill() {
			if(NoSkillComp) return;

			foreach (var pSkill in SkillComp.Skills) {
				var skill = pSkill.Value;
				if (skill.IsEnable) {
						
				}
			}
		}
		
		
	}
}