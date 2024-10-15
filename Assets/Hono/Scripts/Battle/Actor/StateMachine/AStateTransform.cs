using System;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        protected class StateAutoSwitchCheck
        {
            public EActorLogicStateType SwitchTo { get; }
            private readonly Func<bool> _transCondition;

            public StateAutoSwitchCheck(EActorLogicStateType switchTo)
            {
                SwitchTo = switchTo;
            }

            public bool CanSwitch()
            {
                return false;
            }
        }
    }
}