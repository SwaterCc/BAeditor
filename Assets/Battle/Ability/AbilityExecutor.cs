using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 用于执行能力节点序列
        /// </summary>
        private class AbilityExecutor
        {
            private readonly Ability _ability;
            private AbilityNode _curNode;

            /// <summary>
            /// 节点的配置ID-节点对象
            /// </summary>
            private Dictionary<int, AbilityNode> _nodes;

            private Dictionary<EAbilityCycleType, int> _cycleHeads;

            private HashSet<int> _pastNodeId;

            private List<AbilityEventNode> _eventNodes;

            private Stack<int> _repeatNodeIds;

            public AbilityState State => _ability._state;

            public AbilityExecutor(Ability ability)
            {
                _ability = ability;
            }

            /// <summary>
            /// 初始化所有节点
            /// </summary>
            public void Setup()
            {
                var nodeDict = _ability._abilityData.NodeDict;
                if (nodeDict is { Count: > 0 })
                {
                    _nodes ??= new Dictionary<int, AbilityNode>();
                    _cycleHeads ??= new Dictionary<EAbilityCycleType, int>();
                    _eventNodes ??= new List<AbilityEventNode>();
                    _pastNodeId ??= new HashSet<int>();
                    _repeatNodeIds ??= new Stack<int>();
                    //数据转化为实际逻辑节点
                    foreach (var pair in nodeDict)
                    {
                        var node = AbilityNode.CreateNode(this, pair.Value);
                        _nodes.Add(node.ConfigId, node);

                        if (node.NodeData.NodeType == EAbilityNodeType.EAbilityCycle)
                        {
                            _cycleHeads.Add(node.NodeData.CycleNodeData, node.ConfigId);
                        }

                        if (node.NodeData.NodeType == EAbilityNodeType.EEventFired)
                        {
                            _eventNodes.Add((AbilityEventNode)node);
                        }

                        if (node.NodeData.NodeType == EAbilityNodeType.EStage)
                        {
                            var executing = (Executing)State.GetState(EAbilityState.Executing);
                            var stageNode = (AbilityStageNode)node;
                            executing.AddStageProxy(stageNode);
                            if (node.NodeData.StageNodeData.IsDefaultStart)
                            {
                                executing.CurProxy = stageNode;
                            }
                        }
                    }
                }
            }

            public void UnInstall()
            {
                _eventNodes.Clear();
                _cycleHeads.Clear();
                _nodes.Clear();
            }

            public void OnDestroy()
            {
                foreach (var eventNode in _eventNodes)
                {
                    eventNode.UnRegisterEvent();
                }

                foreach (var id in _cycleHeads)
                {
                    _nodes[id.Value].Reset();
                }
            }

            public void RegisterEventNode()
            {
                foreach (var eventNode in _eventNodes)
                {
                    eventNode.RegisterEvent();
                }
            }

            public void PassNode(int id)
            {
                _pastNodeId.Add(id);
            }

            public void RemovePass(int id)
            {
                _pastNodeId.Remove(id);
            }

            public void ResetNode(int id)
            {
                _nodes[id].Reset();
            }

            public bool IsPassedNode(int id)
            {
                return _pastNodeId.Contains(id);
            }

            public AbilityNode GetNode(int id)
            {
                return _nodes[id];
            }

            public bool HeadNodeHasChildren(EAbilityCycleType cycleType)
            {
                var id = _cycleHeads[cycleType];
                return _nodes[id].NodeData.ChildrenUids.Count > 0;
            }


            public void ExecuteNode(int nodeId)
            {
                var curNode = _nodes[nodeId];
                while (nodeId > 0)
                {
                    curNode.DoJob();
                    PassNode(nodeId);

                    if (curNode.NodeData.NodeType == EAbilityNodeType.ERepeat)
                    {
                        _repeatNodeIds.Push(curNode.ConfigId);
                    }

                    nodeId = curNode.GetNextNode();

                    if (nodeId == _repeatNodeIds.Peek())
                    {
                        //重复节点又回来了
                        var repeatNode = ((AbilityRepeatNode)curNode);
                        if (repeatNode.CheckLoopEnd())
                        {
                            _repeatNodeIds.Pop();
                        }
                        else
                        {
                            repeatNode.Repeat();
                            nodeId = _nodes[nodeId].NodeData.ChildrenUids[0];
                            continue;
                        }
                    }

                    if (nodeId == curNode.NodeData.Parent)
                    {
                        while (_nodes[nodeId].NodeData.Parent == _nodes[nodeId].GetNextNode())
                        {
                            //如果不是循环节点则查找可走节点
                            if (nodeId == -1)
                            {
                                break;
                            }

                            nodeId = _nodes[nodeId].NodeData.Parent;
                        }

                        curNode = _nodes[nodeId];
                    }
                }
            }

            public void ExecuteCycleNode(EAbilityCycleType cycleType)
            {
                if (_cycleHeads.TryGetValue(cycleType, out var cycleNodeId))
                {
                    ExecuteNode(cycleNodeId);
                }
                else
                {
                    Debug.Log($"{cycleType} is empty！");
                }
            }

            #region AbilityVariable包装

            public void CreateVariable(string name, IValueBox valueBox)
            {
                _ability.GetVariableCollection().Add(name, valueBox);
            }

            public IValueBox GetVariable(string name)
            {
                return _ability.GetVariableCollection().GetVariable(name);
            }

            public ValueBox<T> GetVariable<T>(string name)
            {
                return _ability.GetVariableCollection().GetVariable(name) as ValueBox<T>;
            }

            public void DeleteVariable(string name)
            {
                _ability.GetVariableCollection().Remove(name);
            }

            #endregion
        }
    }
}