namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class AGroupSwitchNode : ANode<GroupSwitchNodeData>, IGPoolObject
        {
            public override void DoJob()
            {
                var nextGroupId = ParseInt(Data.nextGroupId);
                ACycles.NextGroupId = nextGroupId;
                if (Data.switchGroupNow)
                {
                    ACycles.SwitchGroup();
                }

                DoChildrenJob();
            }

            public override void Recycle()
            {
                GPool<AGroupSwitchNode>.Pool.Recycle(this);
            }
        }
    }
}