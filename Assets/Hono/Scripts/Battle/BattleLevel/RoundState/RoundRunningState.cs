using System;

namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        private class RoundRunningState : RoundState
        {
            public RoundRunningState(Round round) : base(round) { }
            public override ERoundState GetRoundState() => ERoundState.Running;
            
            public override ERoundState GetNextState() => ERoundState.Scoring;
            
            public override bool IsAutoEnd()
            {
                return _round.RoundData.RunningStageTime > 0 && _stateDuration > _round.RoundData.RunningStageTime;
            }

            protected override void onEnter()
            {
                
            }

            protected override void onTick(float dt)
            {
               
            }

            protected override void onExit()
            {
                
            }
        }
    }
}