using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        [Flags]
        public enum AbilityErrorCode
        {
            NoError = 0,
            DataInitFailed = 1,
        }
        
        /// <summary>
        /// 用于执行能力节点序列
        /// </summary>
        private class AbilityExecutor
        {
            private readonly Ability _ability;
            
            private AbilityNode _curNode;

            private Dictionary<int, AbilityNode> _nodes;

            private Dictionary<EAbilityAllowEditCycle, int> _cycleHeads;

            private HashSet<int> _pastNodeId;

            private List<AbilityEventNode> _eventNodes;

            private Stack<int> _repeatNodeIds;

            private bool _hasError;
            
            public Ability Ability => _ability;
            public AbilityState State => _ability._state;
            
            private AbilityData _abilityData;
            public AbilityData AbilityData => _abilityData;
            
            public AbilityExecutor(Ability ability)
            {
                _ability = ability;
                _nodes = new Dictionary<int, AbilityNode>();
                _cycleHeads = new Dictionary<EAbilityAllowEditCycle, int>();
                _eventNodes = new List<AbilityEventNode>();
                _pastNodeId = new HashSet<int>();
                _repeatNodeIds = new Stack<int>();
            }

            /// <summary>
            /// 初始化所有节点
            /// </summary>
            public void Setup()
            {
                if (!AssetManager.Instance.TryGetData(_ability.ConfigId, out _abilityData))
                {
                    _hasError = true;
                    Debug.LogError($"Ability {_ability.ConfigId} SetupFailed,this Ability Can not Running");
                    return;
                }
                
                var nodeDict = _abilityData.NodeDict;
                if (nodeDict is { Count: > 0 })
                {
                    //数据转化为实际逻辑节点
                    foreach (var pair in nodeDict)
                    {
                        var node = AbilityNode.CreateNode(this, pair.Value);
                        _nodes.Add(node.ConfigId, node);

                        if (node.NodeData.NodeType == EAbilityNodeType.EAbilityCycle)
                        {
                            _cycleHeads.Add(node.NodeData.allowEditCycleNodeData, node.ConfigId);
                        }

                        if (node.NodeData.NodeType == EAbilityNodeType.EEvent)
                        {
                            _eventNodes.Add((AbilityEventNode)node);
                        }

                        if (node.NodeData.NodeType == EAbilityNodeType.EGroup)
                        {
                            var executing = (ExecutingCycle)State.GetState(EAbilityState.Executing);
                            var stageNode = (AbilityGroupNode)node;
                            executing.AddStageProxy(stageNode);
                            executing.NextGroupId = _abilityData.DefaultStartGroupId;
                        }
                    }
                }
            }

            public void Reset()
            {
                _hasError = false;
            }
            
            public void UnInstall()
            {
                foreach (var eventNode in _eventNodes)
                {
                    eventNode.UnRegisterEvent();
                }
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

            public bool HeadNodeHasChildren(EAbilityAllowEditCycle allowEditCycle)
            {
                var id = _cycleHeads[allowEditCycle];
                return _nodes[id].NodeData.ChildrenIds.Count > 0;
            }


            public void ExecuteNode(int nodeId)
            {
                if(_hasError) return;
                
                var curNode = _nodes[nodeId];
                while (nodeId > 0)
                {
                    if (!IsPassedNode(nodeId))
                    {
                        curNode.DoJob();
                        PassNode(nodeId);
                    }

                    if (curNode.NodeData.NodeType == EAbilityNodeType.ERepeat)
                    {
                        _repeatNodeIds.Push(curNode.ConfigId);
                    }

                    var nextNodeId = curNode.GetNextNode();
                    
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

            public void ExecuteCycleNode(EAbilityAllowEditCycle allowEditCycle)
            {
                if(_hasError) return;
                
                if (_cycleHeads.TryGetValue(allowEditCycle, out var cycleNodeId))
                {
                    ExecuteNode(cycleNodeId);
                }
                else
                {
                    Debug.Log($"{allowEditCycle} is empty！");
                }
            }

            public void ResetCycle(EAbilityAllowEditCycle allowEditCycle)
            {
                if(_hasError) return;
                
                if (_cycleHeads.TryGetValue(allowEditCycle, out var cycleNodeId))
                {
                    _nodes[cycleNodeId].Reset();
                }
                else
                {
                    Debug.Log($"{allowEditCycle} is empty！");
                }
            }

            #region AbilityVariable包装

            public void CreateVariable(string name, IValueBox valueBox)
            {
                _ability.GetVariables().Add(name, valueBox);
            }

            public object GetVariable(string name)
            {
                return _ability.GetVariables().GetVariable(name);
            }

            public ValueBox<T> GetVariable<T>(string name)
            {
                return _ability.GetVariables().GetVariable(name) as ValueBox<T>;
            }

            public void DeleteVariable(string name)
            {
                _ability.GetVariables().Remove(name);
            }

            #endregion
        }
    }
}