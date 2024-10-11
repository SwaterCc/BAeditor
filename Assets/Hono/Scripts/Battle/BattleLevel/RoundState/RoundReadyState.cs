using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        private class RoundReadyState : RoundState
        {
            public RoundReadyState(Round round) : base(round) { }

            public override ERoundState GetRoundState() => ERoundState.Ready;
            public override ERoundState GetNextState() => ERoundState.Running;

            public override bool IsAutoEnd()
            {
                return _round.RoundData.ReadyStageTime > 0 && _stateDuration > _round.RoundData.ReadyStageTime;
            }

            protected override void onEnter()
            {
                if (_round.RoundData.ReadyStageAbilityId > 0)
                {
                    _round.Controller.AbilityController.AwardAbility(_round.RoundData.ReadyStageAbilityId, true);
                }
            }

            protected override void onTick(float dt) { }

            protected override void onExit() { }
        }
    }
}