using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityRepeatNode : AbilityNode
        {
            private readonly RepeatNodeData _repeatNodeData;
            private int _curLoopCount = 0;
            private int _maxCount;
            public int CurLoopCount => _curLoopCount;
            
            public AbilityRepeatNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _repeatNodeData = data as RepeatNodeData;
            }

            public override void DoJob()
            {
                if (!_repeatNodeData.MaxRepeatCount.ParseParameters(out var count))
                {
                    Debug.LogError("Foreach节点执行错误");
                }

                _maxCount = count;

                for (_curLoopCount = 0; _curLoopCount < _maxCount; _curLoopCount++)
                {
                    DoChildrenJob();
                }

                _curLoopCount = 0;
            }
        }
    }
}