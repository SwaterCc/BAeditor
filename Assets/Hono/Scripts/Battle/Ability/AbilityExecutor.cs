using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class Ability {
		/// <summary>
		/// 用于执行能力节点序列
		/// </summary>
		private class AbilityExecutor {
			private readonly Ability _ability;

			private AbilityNode _curNode;

			private readonly Dictionary<int, AbilityNode> _nodes;

			private readonly Dictionary<EAbilityAllowEditCycle, AbilityNode> _cycleHeads;

			private readonly List<AbilityEventNode> _eventNodes;

			private bool _hasError;

			public Ability Ability => _ability;
			public AbilityState State => _ability._state;
			public AbilityData AbilityData { get; private set; }
			public VarCollection Variables { get; private set; }

			public AbilityExecutor(Ability ability) {
				_ability = ability;
				Variables = ability.Variables;
				_nodes = new Dictionary<int, AbilityNode>();
				_cycleHeads = new Dictionary<EAbilityAllowEditCycle, AbilityNode>();
				_eventNodes = new List<AbilityEventNode>();
			}

			private void setError() {
				_hasError = true;
				Debug.LogError($"Ability {_ability.ConfigId} SetupFailed,this Ability Can not Running");
			}

			private AbilityNode createNode(AbilityExecutor executor, AbilityNodeData data) {
				switch (data.NodeType) {
					case EAbilityNodeType.EAbilityCycle:
						return new AbilityCycleNode(executor, data);
					case EAbilityNodeType.ETimer:
						return new AbilityTimerNode(executor, data);
					case EAbilityNodeType.EBranchControl:
						return new AbilityBranchNode(executor, data);
					case EAbilityNodeType.EVariableControl:
						return new AbilityVariableNode(executor, data);
					case EAbilityNodeType.ERepeat:
						return new AbilityRepeatNode(executor, data);
					case EAbilityNodeType.EAction:
						return new AbilityActionNode(executor, data);
					case EAbilityNodeType.EGroup:
						return new AbilityGroupNode(executor, data);
				}

				Debug.LogError($"创建节点失败，节点ID{data.NodeId} 类型 {data.NodeType}");
				return null;
			}

			/// <summary>
			/// 初始化所有节点
			/// </summary>
			public void Setup() {
				if (!AssetManager.Instance.TryGetData<AbilityData>(_ability.ConfigId, out var abilityData)) {
					setError();
					return;
				}

				AbilityData = abilityData;
				foreach (var tag in abilityData.Tags) {
					Ability.Tags.Add(tag);
				}

				var nodeDict = AbilityData.NodeDict;
				if (nodeDict is { Count: > 0 }) {
					//数据转化为实际逻辑节点
					foreach (var pair in nodeDict) {
						var node = createNode(this, pair.Value);
						if (node == null) {
							setError();
							return;
						}

						_nodes.Add(node.NodeId, node);

						if (pair.Value.NodeType == EAbilityNodeType.EEvent) {
							_eventNodes.Add((AbilityEventNode)node);
						}

						if (pair.Value.NodeType == EAbilityNodeType.EGroup) {
							var executing = (ExecutingCycle)State.GetState(EAbilityState.Executing);
							var stageNode = (AbilityGroupNode)node;
							executing.AddStageProxy(stageNode);
							executing.NextGroupId = AbilityData.DefaultStartGroupId;
						}
					}

					//保存周期头节点
					foreach (var headPair in AbilityData.HeadNodeDict) {
						if (!_nodes.TryGetValue(headPair.Value, out var cycleNode)) {
							setError();
							return;
						}

						_cycleHeads.Add(headPair.Key, cycleNode);
						//构建树结构
						cycleNode.Build(null);
					}
				}
			}

			public void Reset() {
				_hasError = false;
			}

			public void UnInstall() {
				foreach (var eventNode in _eventNodes) {
					eventNode.UnRegisterEvent();
				}

				_eventNodes.Clear();
				_cycleHeads.Clear();
				_nodes.Clear();
			}

			public void OnDestroy() {
				UnInstall();
			}

			public void RegisterEventNode() {
				foreach (var eventNode in _eventNodes) {
					eventNode.RegisterEvent();
				}
			}

			public AbilityNode GetNode(int id) {
				return _nodes[id];
			}

			public bool HeadNodeHasChildren(EAbilityAllowEditCycle allowEditCycle) {
				return _cycleHeads[allowEditCycle].Children.Count > 0;
			}


			public void ExecuteNode(int nodeId) {
				if (_hasError) return;

				if (_nodes.TryGetValue(nodeId, out var node)) {
					node.DoJob();
				}
			}

			public void ExecuteCycleNode(EAbilityAllowEditCycle allowEditCycle) {
				if (_hasError) return;

				if (_cycleHeads.TryGetValue(allowEditCycle, out var cycleNode)) {
					cycleNode.DoJob();
				}
				else {
					Debug.Log($"{allowEditCycle} is empty！");
				}
			}
		}
	}
}