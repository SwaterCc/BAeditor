namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class StiffState : ActorState
        {
            public StiffState(ActorStateMachine machine, EActorStateType stateType) : base(machine, stateType) { }

            public override bool TryGetAutoSwitchState(out EActorStateType next)
            {
                next = StateType;

                if (StateDuration > 1f)
                {
                    next = EActorStateType.Idle;
                    return true;
                }

                return false;
            }
        }
    }
}