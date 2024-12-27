#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AAttrNode : ANode<AttrModifyNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
                int attrValue = AParamParser.ParseInt(AContext, Data.value);

                /*if (Data.IsPersistent)
                {
                    AContext.AddCommand(new AttrCommand(AContext.Actor.GetAttrNoParse(Data.attrType), attrValue));
                }*/
                
                DoChildrenJob();
            }

            public override void Recycle()
            {
                APool<AAttrNode>.Pool.Recycle(this);
            }
        }
    }
}