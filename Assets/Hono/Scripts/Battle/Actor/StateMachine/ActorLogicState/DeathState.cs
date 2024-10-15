using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class DeathState : ActorLogicState {
			public DeathState(ActorStateMachine machine, EActorLogicStateType stateType) : base(machine, stateType) { }

			protected override void onEnter()
			{
				_actorLogic.Actor.TriggerEvent(EBattleEventType.OnActorDead, null);
				ActorManager.Instance.RemoveActor(_actorLogic.Uid);
			}

			public override bool TryGetAutoSwitchState(out EActorLogicStateType next)
			{
				next = StateType;
				return false;
			}
		}
	}
}