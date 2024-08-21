namespace Battle
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
                resetChildren();
                _executor.ExecuteNode(NodeData.ChildrenIds[0]);
            }

            public override int GetNextNode()
            {
                if (NodeData.NextIdInSameLevel > 0)
                {
                    //没有子节点返回自己下一个相邻节点,不用判执行，因为理论上不会跳着走
                    return NodeData.NextIdInSameLevel;
                }

                if (NodeData.Parent > 0)
                {
                    return NodeData.Parent;
                }

                return -1;
            }
        }
    }
}