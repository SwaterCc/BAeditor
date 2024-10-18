
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 时间节点的子节点不可以有时间节点
        /// </summary>
        private class AbilityTimerNode : AbilityNode, ITimer
        {
            private readonly TimerNodeData _timerData;
            private float _duration;
            private int _count;
            private bool _isFirst;

            private int _maxCount;
            private float _firstInterval;
            private float _interval;

            public AbilityTimerNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _timerData = (TimerNodeData)_data;
            }

            public override void DoJob()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;
                
                if (!_timerData.FirstInterval.Parse(_executor.Ability,out _firstInterval))
                {
                    Debug.LogError("Timer节点解析FirstInterval错误");
                    return;
                }
                
                if (!_timerData.MaxCount.Parse(_executor.Ability,out _maxCount))
                {
                    Debug.LogError("Timer节点解析MaxCount错误");
                    return;
                }
                
                if (!_timerData.Interval.Parse(_executor.Ability,out _interval))
                {
                    Debug.LogError("Timer节点解析Interval错误");
                    return;
                }

                _executor.State.Current.TimerStart(this);
            }

            public bool IsFinish()
            {
                return _maxCount != -1 && _count >= _maxCount;
            }

            public void Step(float dt)
            {
                _duration += dt;
            }

            public bool NeedCall()
            {
                bool res;
                if (_isFirst)
                {
                    res = _duration >= _firstInterval;
                }
                else
                {
                    res = _duration >= _interval;
                }

                return res;
            }

            public void OnCallTimer()
            {
                ++_count;
                _duration = 0;
                _isFirst = false;
                DoChildrenJob();
            }
        }
    }
}