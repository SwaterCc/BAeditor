namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class IdleState : AState
        {
            public IdleState(ActorStateMachine machine, ActorLogic actorLogicLogic) : base(machine, actorLogicLogic) { }

            protected override EActorState getStateType()
            {
                return EActorState.Idle;
            }

            public override void Init()
            {
                _transDict[EActorState.Battle].Add(new AStateTransform(EActorState.Battle));
                _transDict[EActorState.Death].Add(new AStateTransform(EActorState.Death));
                _transDict[EActorState.Move].Add(new AStateTransform(EActorState.Move,
                    () => _actorLogic.ActorInput.MoveInputValue.magnitude > 0));
                _transDict[EActorState.Stiff].Add(new AStateTransform(EActorState.Stiff));
            }
        }
    }
}