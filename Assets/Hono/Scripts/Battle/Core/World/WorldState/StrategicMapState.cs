namespace Hono.Scripts.Battle.Core
{
    public partial class World
    {
        private class StrategicMapState : WorldState
        {
            public StrategicMapState(World world) : base(world, EWorldState.StrategicMap) { }
            protected override void OnEnter()
            {
                UIManager.Instance.SetStrategicMap(true);
            }

            protected override void OnTick(float dt)
            {
                throw new System.NotImplementedException();
            }

            protected override void OnExit()
            {
                UIManager.Instance.SetStrategicMap(false);
            }
        }
    }
}