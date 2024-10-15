namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class StiffState : ActorLogicState
		{
			public StiffState(ActorStateMachine machine, EActorLogicStateType stateType) : base(machine, stateType) { }
			public override bool TryGetAutoSwitchState(out EActorLogicStateType next)
			{
				next = StateType;

				if (StateDuration > 1f)
				{
					next = EActorLogicStateType.Idle;
					return true;
				}

				return false;
			}
		}
	}
}