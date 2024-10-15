namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class IdleState : ActorLogicState
        {
            public IdleState(ActorStateMachine machine, EActorLogicStateType stateType) : base(machine, stateType) { }

            public override bool TryGetAutoSwitchState(out EActorLogicStateType next)
            {
                next = StateType;
                if (_actorLogic._actorInput.MoveInputValue.magnitude > 0)
                {
                    next = EActorLogicStateType.Move;
                    return true;
                }

                return false;
            }
        }
    }
}