

namespace Hono.Scripts.Battle {
	public partial class ActorLogic { 
	
		public class MoveState : AState {
			public MoveState(ActorStateMachine machine) : base(machine) { }

			protected override EActorState getStateType() {
				return EActorState.Move;
			}

			public override void Init() {
				_transDict[EActorState.Battle].Add(new AStateTransform(EActorState.Battle, () => true));
				_transDict[EActorState.Death].Add(new AStateTransform(EActorState.Death, () => true));
				_transDict[EActorState.Idle].Add(new AStateTransform(EActorState.Idle, noInput));
				_transDict[EActorState.Stiff].Add(new AStateTransform(EActorState.Stiff, () => true));
			}

			private bool noInput() {
				return true;
			}
		}
	}
}