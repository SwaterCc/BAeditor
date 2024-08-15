using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityTimerNode : AbilityNode, ITimer
        {
            private readonly TimerNodeData _timerData;
            private float _duration;
            private int _count;
            private bool _isFirst;

            public AbilityTimerNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _timerData = NodeData.TimerNodeData;
            }

            public override void DoJob()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;
                _executor.State.Current.TimerStart(this);
            }

            public bool IsFinish()
            {
                return _count > _timerData.MaxCount;
            }

            public void Add(float dt)
            {
                _duration += dt;
            }

            public bool NeedCall()
            {
                bool res;
                if (_isFirst)
                {
                    _isFirst = false;
                    res = _duration >= _timerData.FirstInterval;
                    _duration = 0;
                }
                else
                {
                    res = _duration >= _timerData.Interval;
                    _duration = 0;
                }

                return res;
            }

            public void OnCallTimer()
            {
                ++_count;
                
            }
        }
    }
}