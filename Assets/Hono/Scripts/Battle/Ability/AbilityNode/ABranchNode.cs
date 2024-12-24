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
               
                
              
            }

            public override void Recycle()
            {
                APool<ABranchNode>.Pool.Recycle(this);
            }
        }
    }
}