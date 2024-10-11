namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        private class RoundSuccessScoringState : RoundState
        {
            public RoundSuccessScoringState(Round round) : base(round) { }
            public override ERoundState GetRoundState() => ERoundState.SuccessScoring;

            public override ERoundState GetNextState() => ERoundState.NoActive;

            public override bool IsAutoEnd()
            {
                return _stateDuration > _round.RoundData.SucessScoringStageTime;
            }

            protected override void onEnter() { }

            protected override void onTick(float dt) { }

            protected override void onExit()
            {
                _round.Controller.onRoundFinish();
            }
        }
    }
}