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

                        if (node.NodeData.NodeType == EAbilityNodeType.EEvent)
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
                return _nodes[id].NodeData.ChildrenIds.Count > 0;
            }


            public void ExecuteNode(int nodeId)
            {
                var curNode = _nodes[nodeId];
                Debug.Log($"ExecuteNode start  NodeId {nodeId} NodeType {curNode.NodeData.NodeType}");
                while (nodeId > 0)
                {
                    if (!IsPassedNode(nodeId))
                    {
                        curNode.DoJob();
                        PassNode(nodeId);
                        Debug.Log($"DoJob NodeId {nodeId} NodeType {curNode.NodeData.NodeType}");
                    }

                    if (curNode.NodeData.NodeType == EAbilityNodeType.ERepeat)
                    {
                        _repeatNodeIds.Push(curNode.ConfigId);
                    }

                    var nextNodeId = curNode.GetNextNode();
                    Debug.Log($"DoJob nextNodeId {nextNodeId}");
                    if (nextNodeId != -1)
                    {
                        Debug.Log($"NextNodeType {_nodes[nextNodeId].NodeData.NodeType}");
                    }
                    
                    if (_repeatNodeIds.Count > 0 && nextNodeId == _repeatNodeIds.Peek())
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
                            nodeId = _nodes[nextNodeId].NodeData.ChildrenIds[0];
                            continue;
                        }
                    }
                    
                    if (nextNodeId != -1)
                    {
                        curNode = _nodes[nextNodeId];
                    }
                    
                    if (curNode.NodeData.NodeType == EAbilityNodeType.ETimer && IsPassedNode(nextNodeId))
                    {
                        nextNodeId = -1;
                    }
                    
                    nodeId = nextNodeId;
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

            public void ResetCycle(EAbilityCycleType cycleType)
            {
                if (_cycleHeads.TryGetValue(cycleType, out var cycleNodeId))
                {
                    _nodes[cycleNodeId].Reset();
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

            public object GetVariable(string name)
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