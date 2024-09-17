using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class Ability {
		private class AbilityRepeatNode : AbilityNode {
			private readonly RepeatNodeData _repeatNodeData;
			private int _curLoopCount = 0;
			private float _curValue = 0;
			private int _maxCount;

			public AbilityRepeatNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) {
				_repeatNodeData = (RepeatNodeData)data;
			}

			protected override void onReset() {
				_curLoopCount = 0;
				_curValue = 0;
			}

			public override void ChildrenJobFinish() {
				if (++_curLoopCount < _maxCount) {
					resetChildren();
				}
			}

			public override void DoJob() {
				if (!_repeatNodeData.MaxRepeatCount.Parse(out _maxCount))
				{
					Debug.LogError("Foreach节点执行错误");
				}
				
				for (int i = 0; i < _maxCount; i++) {
					DoChildrenJob();
				}
			}
		}
	}
}