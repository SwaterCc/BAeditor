namespace Hono.Scripts.Battle {
	public partial class Ability {
		private class AbilityRepeatNode : AbilityNode {
			private RepeatNodeData _repeatNodeData;
			private int _curLoopCount = 0;
			private float _curValue = 0;


			public AbilityRepeatNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) {
				_repeatNodeData = data.RepeatNodeData;
			}

			protected override void onReset() {
				_curLoopCount = 0;
				_curValue = 0;
			}

			public void Repeat() {
				++_curLoopCount;
				_executor.RemovePass(ConfigId);
				resetChildren();
			}

			public bool CheckLoopEnd() {
				if (_curLoopCount < _repeatNodeData.MaxRepeatCount) {
					return true;
				}

				return false;
			}

			public override void DoJob() {
				
			}

			public override int GetNextNode() {
				if (!CheckLoopEnd()) {
					return NodeData.ChildrenIds[0];
				}

				if (NodeData.NextIdInSameLevel > 0) {
					//没有子节点返回自己下一个相邻节点,不用判执行，因为理论上不会跳着走
					return NodeData.NextIdInSameLevel;
				}

				if (NodeData.Parent > 0) {
					return _executor.GetNode(NodeData.Parent).GetNextNode();
				}

				return -1;
			}
		}
	}
}