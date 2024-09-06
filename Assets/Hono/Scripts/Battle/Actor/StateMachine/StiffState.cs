namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class StiffState : AState
		{
			public StiffState(ActorStateMachine machine, ActorLogic actorLogicLogic) : base(machine, actorLogicLogic) { }
			protected override EActorState getStateType()
			{
				return EActorState.Stiff;
			}

			public override void Init()
			{
				_transDict[EActorState.Idle].Add(new AStateTransform(EActorState.Idle));
			}
		}
	}
}