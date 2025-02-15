#region

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class ABranchNode : ANode<BranchNodeData>, IGPoolObject
        {
            public override void DoJob()
            {
                var conditionResult = ParseBoolean(Data.condition);
                if (Parent is ABranchGroupNode branchGroupNode)
                {
                    branchGroupNode.AnyBranchPass = conditionResult;
                }

                if (conditionResult)
                    DoChildrenJob();
            }

            public override void Recycle()
            {
                GPool<ABranchNode>.Pool.Recycle(this);
            }
        }
    }
}