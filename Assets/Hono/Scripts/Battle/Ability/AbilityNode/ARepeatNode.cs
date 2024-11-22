#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public partial class Ability {
		private class ARepeatNode : ANode<RepeatNodeData>, IAPoolObject {
			private RefInt _maxCount;

			public override void Recycle() {
				AObjectPool<ARepeatNode>.Pool.Recycle(this);
			}

			public override void DoJob() {
				if (!Data.MaxRepeatCount.TryParse(AContext, out _maxCount)) {
					Debug.LogError("Foreach节点执行错误");
				}

				for (int i = 0; i < _maxCount; i++) {
					DoChildrenJob();
				}
			}

			protected override void OnChildrenJobFinish() {
				resetChildren();
			}

			public new void OnRecycle() {
				_maxCount = null;
			}
		}
	}
}