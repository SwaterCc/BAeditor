﻿namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundFailedScoringState : RoundState
        {
            public RoundFailedScoringState(RoundController roundController) : base(roundController) { }
            public override ERoundState GetRoundState() => ERoundState.FailedScoring;


            protected override void onEnter() { }

            protected override void onTick(float dt)
            {
                if (!(Duration > CurrentRoundData.FailedScoringStageTime)) return;

                if (Round.CanRepeat)
                {
                    Round.SwitchState(ERoundState.Ready);
                }
                else
                {
                    Round.SwitchState(ERoundState.NoRunning);
                    //结算战场
                    Round.GameRunningState.ScoreBattle(false);
                }
            }

            protected override void onExit() { }
        }
    }
}