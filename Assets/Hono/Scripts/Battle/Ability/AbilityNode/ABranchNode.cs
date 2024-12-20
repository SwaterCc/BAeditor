#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class ABranchNode : ANode<BranchNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
               

                if (!Data.CompareFunc.TryParse(AContext, out RefBoolean branchResult))
                {
                    Debug.LogError("Branch节点执行错误");
                    return;
                }

              
            }

            public override void Recycle()
            {
                APool<ABranchNode>.Pool.Recycle(this);
            }
        }
    }
}