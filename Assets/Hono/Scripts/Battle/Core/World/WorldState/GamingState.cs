namespace Hono.Scripts.Battle.Core
{
    public partial class World
    {
        private class GamingState : WorldState
        {
            public GamingState(World world) : base(world, EWorldState.Gaming) { }

            protected override void OnEnter()
            {
                //任务流程启动
            }

            protected override void OnTick(float dt)
            {
                World._worldNodeRoot.Tick(dt);
            }

            protected override void OnExit()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}