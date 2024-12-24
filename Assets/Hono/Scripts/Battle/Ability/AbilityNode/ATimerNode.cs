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
            private int _maxCount;
            private float _interval;
            private float _firstInterval;

            private float _duration;
            private int _count;
            private bool _isFirst;

            public override void DoJob()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;

                _firstInterval = AParamParser.ParseFloat(AContext, Data.FirstInterval);
                _interval = AParamParser.ParseFloat(AContext,      Data.Interval);
                _maxCount = AParamParser.ParseInt(AContext, Data.MaxCount);

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

                _maxCount = 0;
                _interval = 0;
                _firstInterval = 0;
            }

            public override void Recycle()
            {
                APool<ATimerNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                onReset();
            }
        }
    }
}