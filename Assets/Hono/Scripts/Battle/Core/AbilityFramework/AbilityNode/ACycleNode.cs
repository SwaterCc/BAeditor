namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class ACycleNode : ANode, IGPoolObject
        {
            public override void DoJob()
            {
                DoChildrenJob();
            }

            public override void Recycle()
            {
                GPool<ACycleNode>.Pool.Recycle(this);
            }
        }
    }
}