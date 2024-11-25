namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class IdleState : ActorState
        {
            public IdleState(ActorStateMachine machine, EActorStateType stateType) : base(machine, stateType) { }

            public override bool TryGetAutoSwitchState(out EActorStateType next)
            {
                next = StateType;
                var isStunned = _actorLogic.GetAttr(EAttrType.AttrStunned);
                if (_actorLogic._actorInput.MoveInputValue.magnitude > 0 && isStunned == 0)
                {
                    next = EActorStateType.Move;
                    return true;
                }

                return false;
            }
        }
    }
}