#region

using Hono.Scripts.Battle.Event;

#endregion

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class DeathState : ActorState {
			public DeathState(ActorStateMachine machine, EActorStateType stateType) : base(machine, stateType) { }

			protected override void onEnter() {
				//_actorLogic.Self.TriggerEvent(EBattleEventType.OnActorDead, null);
				ActorManager.Instance.RemoveActor(_actorLogic.Uid);
			}

			public override bool TryGetAutoSwitchState(out EActorStateType next) {
				next = StateType;
				return false;
			}
		}
	}
}