using System;
using System.Collections.Generic;

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

            private Dictionary<EAbilityCycleType, AbilityCycleNode> _cycleHeads;

            private HashSet<int> _pastNodeId;

            private List<AbilityEventNode> _eventNodes;

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
                    _cycleHeads ??= new Dictionary<EAbilityCycleType, AbilityCycleNode>();
                    _eventNodes ??= new List<AbilityEventNode>();
                    _pastNodeId ??= new HashSet<int>();
                    //数据转化为实际逻辑节点
                    foreach (var pair in nodeDict)
                    {
                       var node = AbilityNode.CreateNode(this, pair.Value);
                       _nodes.Add(node.ConfigId, node);

                       if (node.NodeData.NodeType == EAbilityNodeType.EAbilityCycle)
                       {
                           _cycleHeads.Add(node.NodeData.CycleNodeData, (AbilityCycleNode)node);
                       }

                       if (node.NodeData.NodeType == EAbilityNodeType.EEventFired)
                       {
                           _eventNodes.Add((AbilityEventNode)node);
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
            
            public void RegisterEventNode()
            {
                foreach (var eventNode in _eventNodes)
                {
                    eventNode.RegisterEvent();
                }
            }

            public bool nodeIsPast(int id)
            {
               return _pastNodeId.Contains(id);
            }
            
            public AbilityNode GetNode(int id)
            {
                return _nodes[id];
            }

            public bool HeadNodeHasChildren(EAbilityCycleType cycleType)
            {
                return _cycleHeads[cycleType].NodeData.ChildrenUids.Count > 0;
            }
            
            public void ExecuteCycleNode(EAbilityCycleType cycleType)
            {
                if (_cycleHeads.TryGetValue(cycleType,out var cycleNode))
                {
                    RunNode(cycleNode);
                }
                else
                {
                    //打日志
                }
            }
            
            public void RunNode(AbilityNode node)
            {
               node.DoJob();
            }
        }
    }
}