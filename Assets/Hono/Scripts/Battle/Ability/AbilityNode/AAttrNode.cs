#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AAttrNode : ANode<AttrNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
                if (!Data.Value.TryParse(AContext, out RefInt value))
                {
                    Debug.LogError($"设置属性失败 {Data.attrType}");
                    return;
                }

                AContext.AddCommand(new AttrCommand(AContext.Actor.GetAttrNoParse(Data.attrType), value));

                DoChildrenJob();
            }

            public override void Recycle()
            {
                APool<AAttrNode>.Pool.Recycle(this);
            }
        }
    }
}