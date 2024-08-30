

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class IdleState : AState {
			public IdleState(ActorStateMachine machine) : base(machine) { }

			protected override EActorState getStateType() {
				return EActorState.Idle;
			}

			public override void Init() {
				_transDict[EActorState.Battle].Add(new AStateTransform(EActorState.Battle, () => true));
				_transDict[EActorState.Death].Add(new AStateTransform(EActorState.Death, () => true));
				_transDict[EActorState.Move].Add(new AStateTransform(EActorState.Move, () => true));
				_transDict[EActorState.Stiff].Add(new AStateTransform(EActorState.Stiff, () => true));
			}
		}
	}
}