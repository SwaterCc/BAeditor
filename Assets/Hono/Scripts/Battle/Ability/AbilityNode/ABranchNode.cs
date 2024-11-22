#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public partial class Ability {
		private class ABranchNode : ANode<BranchNodeData>, IAPoolObject {
			public override void DoJob() {
				if (Parent.GetBranchGroupState(Data.BranchGroupId)) {
					return;
				}
				
				if (!Data.CompareFunc.TryParse(AContext, out RefBool branchResult)) {
					Debug.LogError("Branch节点执行错误");
					return;
				}

				if (branchResult) {
					Parent.SetBranchGroupPass(Data.BranchGroupId);
					DoChildrenJob();
				}
			}

			public override void Recycle() {
				AObjectPool<ABranchNode>.Pool.Recycle(this);
			}
		}
	}
}