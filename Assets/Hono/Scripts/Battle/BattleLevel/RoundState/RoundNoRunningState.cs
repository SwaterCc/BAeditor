namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundNoRunningState : RoundState
        {
            public RoundNoRunningState(RoundController roundController) : base(roundController) { }

            public override ERoundState GetRoundState()
            {
                return ERoundState.NoRunning;
            }

            protected override void onEnter() { }

            protected override void onExit() { }
        }
    }
}