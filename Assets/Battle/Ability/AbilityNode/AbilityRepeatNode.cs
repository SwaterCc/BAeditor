using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Def;
using Battle.Tools;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityRepeatNode : AbilityNode
        {
            private RepeatNodeData _repeatNodeData;
            private const string LOOP_VALUE = "LOOP_VALUE";
            private int _curLoopCount = 0;
            private float _curValue = 0;
            private Queue<Param> _params;
            private IList _enumerable = null;
            
            public AbilityRepeatNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _repeatNodeData = data.RepeatNodeData;
                _params = new Queue<Param>(_repeatNodeData.CallFuncData);
            }

            protected override void onReset()
            {
                _curLoopCount = 0;
                _curValue = 0;
                _enumerable = null;
            }

            public override void DoJob()
            {
                switch (_repeatNodeData.RepeatOperationType)
                {
                    case ERepeatOperationType.OnlyRepeat:
                        _executor.CreateVariable(LOOP_VALUE,new ValueBox<int>(_curLoopCount));
                        if (_curLoopCount++ < _repeatNodeData.MaxRepeatCount)
                        {
                            _executor.GoNext(NodeData.ChildrenUids[0]);
                        }
                        break;
                    case ERepeatOperationType.NumberLoop:
                        _executor.CreateVariable(LOOP_VALUE,new ValueBox<float>(_repeatNodeData.StartValue));
                        if (_curLoopCount++ < _repeatNodeData.StepCount)
                        {
                            _curValue += _repeatNodeData.StartValue;
                            _executor.GoNext(NodeData.ChildrenUids[0]);
                        }
                        break;
                }
            }

            public override int GetNextNode()
            {
                switch (_repeatNodeData.RepeatOperationType)
                {
                    case ERepeatOperationType.OnlyRepeat:
                        if (_curLoopCount < _repeatNodeData.MaxRepeatCount)
                        {
                            _executor.GetVariable<int>(LOOP_VALUE).Set(_curLoopCount++);
                            resetChildren();
                            return NodeData.ChildrenUids[0];
                        }
                        break;
                    case ERepeatOperationType.NumberLoop:
                        if (_curLoopCount++ < _repeatNodeData.StepCount)
                        {
                            _curValue += _repeatNodeData.StepValue;
                            _executor.GetVariable<float>(LOOP_VALUE).Set(_curValue);
                            resetChildren();
                            return NodeData.ChildrenUids[0];
                        }
                        break;
                }

                if (NodeData.NextIdInSameLevel > 0)
                {
                    //没有子节点返回自己下一个相邻节点,不用判执行，因为理论上不会跳着走
                    return NodeData.NextIdInSameLevel;
                }

                if (NodeData.Parent > 0)
                {
                    return _executor.GetNode(NodeData.Parent).GetNextNode();
                }

                return -1;
            }
        }    
    }
    
}