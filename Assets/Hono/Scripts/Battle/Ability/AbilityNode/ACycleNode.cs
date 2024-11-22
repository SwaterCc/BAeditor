namespace Hono.Scripts.Battle {
	public partial class Ability {
		private class ACycleNode : ANode, IAPoolObject {
			public override void DoJob() {
				DoChildrenJob();
			}

			public override void Recycle() {
				AObjectPool<ACycleNode>.Pool.Recycle(this);
			}
		}
	}
}