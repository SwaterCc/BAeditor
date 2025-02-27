namespace Hono.Scripts.Battle.Core
{
    public class CondTimerTick : LevelConditionMonitor, IMonitorTickEnable
    {
        private float _duration;

        public CondTimerTick(float duration)
        {
            _duration = duration;
        }

        public override void OnMonitorExecute() { }

        public override void OnPass() { }

        public void Tick(float dt)
        {
            _duration -= dt;
            if (_duration < 0)
            {
                SetPass();
            }
        }
    }
}