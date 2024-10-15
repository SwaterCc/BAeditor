namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class SkillState : ActorLogicState {
			public SkillState(ActorStateMachine machine, EActorLogicStateType stateType) : base(machine, stateType) { }
			public override bool TryGetAutoSwitchState(out EActorLogicStateType next)
			{
				next = StateType;
				return false;
			}
		}
	}
}