namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class ACycleNode : ANode, IAPoolObject
        {
            public override void DoJob()
            {
                DoChildrenJob();
            }

            public override void Recycle()
            {
                APool<ACycleNode>.Pool.Recycle(this);
            }
        }
    }
}