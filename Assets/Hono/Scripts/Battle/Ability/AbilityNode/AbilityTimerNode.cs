
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

            private float _maxCount;
            private float _firstInterval;
            private float _interval;

            public AbilityTimerNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _timerData = NodeData.TimerNodeData;
            }

            public override void DoJob()
            {
                _duration = 0;
                _count = 0;
                _isFirst = true;
                
                if (!_timerData.FirstInterval.ParseParameters(out var firstInterval))
                {
                    Debug.LogError("Branch节点执行错误");
                }

                _firstInterval = (float)firstInterval;
                
                if (!_timerData.MaxCount.ParseParameters(out var maxCount))
                {
                    Debug.LogError("Branch节点执行错误");
                }

                _maxCount = (int)maxCount;
                
                if (!_timerData.Interval.ParseParameters(out var interval))
                {
                    Debug.LogError("Branch节点执行错误");
                }

                _interval = (float)interval;
                
                
                _executor.State.Current.TimerStart(this);
            }

            public bool IsFinish()
            {
                return _count >= _maxCount;
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