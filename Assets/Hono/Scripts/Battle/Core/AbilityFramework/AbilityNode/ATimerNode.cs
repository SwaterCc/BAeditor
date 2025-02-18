#region

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        /// <summary>
        /// 计时器节点，每次触发都会重新执行该定时器
        /// </summary>
        private class ATimerNode : ANode<TimerNodeData>, IGPoolObject, ITickANode , ILocalVariableBoardHandle
        {
            private int _maxCount;
            private float _interval;
            private float _firstInterval;

            private float _duration;
            private int _count;
            private bool _isFirst;

            /// <summary>
            /// 计时器是否正在运行
            /// </summary>
            private bool _isRunning;

            /// <summary>
            /// 监听获取的信息黑板
            /// </summary>
            public VariableBoard LocalVariableBoard { get; private set;}
            
            public override void DoJob()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;

                _firstInterval = ParseFloat(Data.firstInterval);
                _interval = ParseFloat(Data.interval);
                _maxCount = ParseInt(Data.maxCount);

                if (!_isRunning)
                {
                    ACycles.RegisterTick(this);
                    _isRunning = true;
                }

                if (TryGetParent<AListenerNode>(out var listenerNode))
                {
                    LocalVariableBoard = listenerNode.LocalVariableBoard;
                    LocalVariableBoard.RefCount.AddReference();
                }
            }

            public void Stop()
            {
                _isRunning = false;
            }

            public void Tick(float dt)
            {
                if (!_isRunning)
                {
                    return;
                }
                
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
                _isRunning = false;

                GPool<VariableBoard>.Pool.Recycle(LocalVariableBoard);
                LocalVariableBoard = null;
            }

            public override void Recycle()
            {
                GPool<ATimerNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                onReset();
            }
        }
    }
}