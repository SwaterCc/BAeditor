namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class ABranchGroupNode : ANode<BranchGroupNodeData>, IGPoolObject
        {
            /// <summary>
            /// 子节点中任意一个branch通过
            /// </summary>
            public bool AnyBranchPass;

            public override void DoJob()
            {
                foreach (var node in Children)
                {
                    if (node is not ABranchNode branchNode)
                    {
                        AContext.LogError("BranchGroup节点中存在不是Branch的节点");
                        continue;
                    }

                    branchNode.DoJob();
                    if (!AnyBranchPass) continue;
                    AnyBranchPass = false;
                    break;
                }
            }

            public override void Recycle()
            {
                GPool<ABranchGroupNode>.Pool.Recycle(this);
            }
        }
    }
}