namespace Hono.Scripts.Battle.Core
{
    public partial class WorldInstance
    {
        private class EmptyState : WorldState
        {
            public EmptyState(WorldInstance worldInstance) : base(worldInstance, EWorldState.Empty) { }

            protected override void OnEnter()
            {
                //任务流程启动
            }

            protected override void OnTick(float dt)
            {
               
            }

            protected override void OnExit() { }
        }
    }
}