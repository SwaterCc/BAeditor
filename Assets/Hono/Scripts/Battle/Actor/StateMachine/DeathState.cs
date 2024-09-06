namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class DeathState : AState {
			public DeathState(ActorStateMachine machine, ActorLogic actorLogicLogic) : base(machine, actorLogicLogic) { }
			protected override EActorState getStateType()
			{
				return EActorState.Death;
			}

			public override void Init()
			{
				_transDict[EActorState.Idle].Add(new AStateTransform(EActorState.Idle));
			}
		}
	}
}