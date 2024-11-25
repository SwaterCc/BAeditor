namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class SkillState : ActorState
        {
            public SkillState(ActorStateMachine machine, EActorStateType stateType) : base(machine, stateType) { }

            public override bool TryGetAutoSwitchState(out EActorStateType next)
            {
                next = StateType;
                return false;
            }
        }
    }
}