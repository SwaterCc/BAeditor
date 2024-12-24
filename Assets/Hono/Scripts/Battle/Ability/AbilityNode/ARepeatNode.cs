#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class ARepeatNode : ANode<RepeatNodeData>, IAPoolObject
        {
            public override void Recycle()
            {
                APool<ARepeatNode>.Pool.Recycle(this);
            }

            public override void DoJob()
            {
                var maxCount = AParamParser.ParseInt(AContext, Data.MaxRepeatCount);
                for (int i = 0; i < maxCount; i++)
                {
                    DoChildrenJob();
                }
            }

            protected override void OnChildrenJobFinish()
            {
                resetChildren();
            }
        }
    }
}