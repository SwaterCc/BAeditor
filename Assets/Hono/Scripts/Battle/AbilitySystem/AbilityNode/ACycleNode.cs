namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private class ACycleNode : ANode, ICPoolObject
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