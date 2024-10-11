namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        private class RoundFailedScoringState : RoundState
        {
            public RoundFailedScoringState(Round round) : base(round) { }
            public override ERoundState GetRoundState() => ERoundState.FailedScoring;

            public override ERoundState GetNextState() => ERoundState.NoActive;

            public override bool IsAutoEnd()
            {
                return _stateDuration > _round.RoundData.FailedScoringStageTime;
            }

            protected override void onEnter() { }

            protected override void onTick(float dt) { }

            protected override void onExit()
            {
                _round.Controller.onRoundFailed();
            }
        }
    }
}