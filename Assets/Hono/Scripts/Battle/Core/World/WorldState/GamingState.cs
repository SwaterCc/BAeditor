namespace Hono.Scripts.Battle.Core
{
    public partial class WorldInstance
    {
        private class GamingState : WorldState
        {
            public GamingState(WorldInstance worldInstance) : base(worldInstance, EWorldState.Gaming) { }

            protected override void OnEnter()
            {
                //任务流程启动
            }

            protected override void OnTick(float dt)
            {
                WorldInstance._worldNodeRoot.Tick(dt);
            }

            protected override void OnExit()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}