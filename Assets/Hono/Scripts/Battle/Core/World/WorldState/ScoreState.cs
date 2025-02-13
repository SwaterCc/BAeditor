namespace Hono.Scripts.Battle.Core
{
    public partial class WorldInstance
    {
        private class ScoreState : WorldState
        {
            public ScoreState(WorldInstance worldInstance) : base(worldInstance, EWorldState.Score) { }
            protected override void OnEnter()
            {
                UIManager.Instance.SetScoreUI(true);
            }

            protected override void OnTick(float dt)
            {
                throw new System.NotImplementedException();
            }

            protected override void OnExit()
            {
                UIManager.Instance.SetScoreUI(false);
                BattleManager.Instance.ExitWorld();
            }
        }
    }
}