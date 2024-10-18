﻿namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundSuccessScoringState : RoundState
        {
            public RoundSuccessScoringState(RoundController roundController) : base(roundController) { }
            public override ERoundState GetRoundState() => ERoundState.SuccessScoring;

            protected override void onEnter() {
	            Round.GameRunningState.BattleGroundHandle.RuntimeInfo.RPCount += CurrentRoundData.RPCount;
            }

            protected override void onTick(float dt)
            {
                if (!(Duration > CurrentRoundData.SucessScoringStageTime)) return;
                
                Round.GameRunningState.BattleGroundHandle.RuntimeInfo.CurRoundLastTime = CurrentRoundData.SucessScoringStageTime - Duration;
                
                Round.RoundCountAdd();
                
                if (Round.IsFinalRound)
                {
                    Round.SwitchState(ERoundState.NoRunning);
                    //结算战场
                    Round.GameRunningState.ScoreBattle(true);
                }
                else
                {
                    //进入下一个轮次
                    Round.SwitchState(ERoundState.Ready);
                }
            }

            protected override void onExit() { }
        }
    }
}