#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 时间节点的子节点不可以有时间节点
        /// </summary>
        private class ATimerNode : ANode<TimerNodeData>, IAPoolObject, ITickANode
        {
            private RefInt _maxCount;
            private RefFloat _interval;
            private RefFloat _firstInterval;

            private float _duration;
            private int _count;
            private bool _isFirst;

            public override void DoJob()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;

                if (!Data.FirstInterval.TryParse(AContext, out _firstInterval))
                {
                    Debug.LogError("Timer节点解析FirstInterval错误");
                    return;
                }

                if (!Data.MaxCount.TryParse(AContext, out _maxCount))
                {
                    Debug.LogError("Timer节点解析MaxCount错误");
                    return;
                }

                if (!Data.Interval.TryParse(AContext, out _interval))
                {
                    Debug.LogError("Timer节点解析Interval错误");
                    return;
                }

                ACycles.RegisterTick(this);
            }

            public void Tick(float dt)
            {
                if (isSchedule())
                {
                    invoke();
                    resetChildren();
                }

                if (isExit())
                {
                    ACycles.UnRegisterTick(this);
                    Reset();
                }

                _duration += dt;
            }

            private bool isSchedule()
            {
                if (_isFirst)
                {
                    return _duration >= _firstInterval;
                }

                return _duration >= _interval;
            }

            private void invoke()
            {
                ++_count;
                _duration = 0;
                _isFirst = false;
                DoChildrenJob();
            }

            private bool isExit()
            {
                return _maxCount != -1 && _count >= _maxCount;
            }

            protected override void onReset()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;

                _maxCount = null;
                _interval = null;
                _firstInterval = null;
            }

            public override void Recycle()
            {
                AObjectPool<ATimerNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                onReset();
            }
        }
    }
}