namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class DeathState : AState {
			public DeathState(ActorStateMachine machine, ActorLogic actorLogicLogic) : base(machine, actorLogicLogic) { }

			private bool _isDead;
			
			protected override EActorState getStateType()
			{
				return EActorState.Death;
			}

			public override void Init()
			{
				_transDict[EActorState.Idle].Add(new AStateTransform(EActorState.Idle));
			}
			
			protected override void onTick(float dt) {
				if(_isDead) return;
				ActorManager.Instance.RemoveActor(_actorLogic.Actor);
				_isDead = true;
			}
		}
	}
}