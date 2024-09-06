namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class BattleState : AState {
			public BattleState(ActorStateMachine machine, ActorLogic actorLogicLogic) : base(machine, actorLogicLogic) { }
			protected override EActorState getStateType()
			{
				return EActorState.Battle;
			}

			public override void Init()
			{
				_transDict[EActorState.Battle].Add(new AStateTransform(EActorState.Battle));
				_transDict[EActorState.Death].Add(new AStateTransform(EActorState.Death));
				_transDict[EActorState.Move].Add(new AStateTransform(EActorState.Move));
				_transDict[EActorState.Stiff].Add(new AStateTransform(EActorState.Stiff));
				_transDict[EActorState.Idle].Add(new AStateTransform(EActorState.Idle));
			}
		}
	}
}