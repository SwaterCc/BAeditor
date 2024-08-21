using System;
using System.Collections;
using System.Collections.Generic;
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
           
            
            public AbilityRepeatNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _repeatNodeData = data.RepeatNodeData;
            }

            protected override void onReset()
            {
                _curLoopCount = 0;
                _curValue = 0;
            }
            
            public void Repeat()
            {
                switch (_repeatNodeData.RepeatOperationType)
                {
                    case ERepeatOperationType.OnlyRepeat:
                        _executor.GetVariable<int>(LOOP_VALUE).Set(_curLoopCount);
                        ++_curLoopCount;
                        break;
                    case ERepeatOperationType.NumberLoop:
                        ++_curLoopCount;
                        _curValue += _repeatNodeData.StepValue;
                        _executor.GetVariable<float>(LOOP_VALUE).Set(_curValue);
                        break;
                }
                _executor.RemovePass(ConfigId);
                resetChildren();
            }

            public bool CheckLoopEnd()
            {
                switch (_repeatNodeData.RepeatOperationType)
                {
                    case ERepeatOperationType.OnlyRepeat:
                        if (_curLoopCount < _repeatNodeData.MaxRepeatCount)
                        {
                            return true;
                        }
                        break;
                    case ERepeatOperationType.NumberLoop:
                        if (_curLoopCount++ < _repeatNodeData.StepCount)
                        {
                            return true;
                        }
                        break;
                }

                return false;
            }
            
            public override void DoJob()
            {
                switch (_repeatNodeData.RepeatOperationType)
                {
                    case ERepeatOperationType.OnlyRepeat:
                        _executor.CreateVariable(LOOP_VALUE,new ValueBox<int>(_curLoopCount));
                      
                        break;
                    case ERepeatOperationType.NumberLoop:
                        _executor.CreateVariable(LOOP_VALUE,new ValueBox<float>(_repeatNodeData.StartValue));
                        break;
                }
            }

            public override int GetNextNode()
            {
                if (!CheckLoopEnd())
                {
                    return NodeData.ChildrenIds[0];
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