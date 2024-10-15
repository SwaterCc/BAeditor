using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class Ability {
		private abstract class AbilityNode {
			public int NodeId { get; }

			protected AbilityNodeData _data;
			public EAbilityNodeType NodeType => _data.NodeType;

			protected AbilityExecutor _executor;
			public bool IsExecuted { get; }
			public AbilityNode Parent { get; private set; }
			public List<AbilityNode> Children { get; private set; }

			public Dictionary<int, bool> IfInfos = new Dictionary<int, bool>();

			public int BelongGroupId => _data.BelongGroupId;

			protected AbilityNode(AbilityExecutor executor, AbilityNodeData data) {
				_data = data;
				_executor = executor;
				IsExecuted = false;
				NodeId = _data.NodeId;
				Children = new List<AbilityNode>();
			}

			public void Build(AbilityNode parent) {
				//设置父节点
				Parent = parent;
				int IfGroupCount = 1;
				int LinkId = -1;
				//填充子节点
				foreach (var nodeId in _data.ChildrenIds) {
					var node = _executor.GetNode(nodeId);
					Children.Add(node);
					node.Build(this);

					if (node._data.NodeType == EAbilityNodeType.EBranchControl) {
						IfInfos.TryAdd(((BranchNodeData)(node._data)).BranchGroup, false);
					}
				}
			}

			/// <summary>
			/// 执行节点功能
			/// </summary>
			public abstract void DoJob();

			public void DoChildrenJob() {
				foreach (var node in Children) {
					if (node._data.NodeType == EAbilityNodeType.EBranchControl) {
						var ifGroupId = ((BranchNodeData)(node._data)).BranchGroup;
						if (IfInfos.TryGetValue(ifGroupId, out var hasSuccess) && hasSuccess) {
							continue;
						}
					}
					
					//Debug.Log($"[Ability] AbilityId:{_executor.AbilityData.ConfigId} nodeId {NodeId} NodeType {NodeType} DoChildrenJob Next Node is {node.NodeType} : {node.NodeId}");
					node.DoJob();
				}

				var keys = new List<int>(IfInfos.Keys); // 获取所有键的列表
				foreach (var key in keys)
				{
					IfInfos[key] = false;
				}
				
				ChildrenJobFinish();
			}

			/// <summary>
			/// 该节点的子节点全部执行完了
			/// </summary>
			public virtual void ChildrenJobFinish() { }

			/// <summary>
			/// 重置该节点，注意，同时该节点会被重置为未执行过
			/// </summary>
			public void Reset() {
				onReset();
				resetChildren();
			}

			protected void resetChildren() { }

			protected virtual void onReset() { }
		}
	}
}