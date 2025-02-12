namespace Hono.Scripts.Battle.Core
{
    public partial class WorldInstance
    {
        private class StrategicMapState : WorldState
        {
            public StrategicMapState(WorldInstance worldInstance) : base(worldInstance, EWorldState.StrategicMap) { }
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