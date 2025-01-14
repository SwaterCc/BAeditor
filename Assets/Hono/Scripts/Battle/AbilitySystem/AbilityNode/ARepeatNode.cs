#region

using System;
using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
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
                switch (Data.operationType)
                {
                    case ERepeatNodeOperationType.Repeat:
                        for (int i = 0; i < Data.repeatCount; i++)
                        {
                            DoChildrenJob();
                        }
                        break;
                    case ERepeatNodeOperationType.ETraverseList:
                        /*var maxCount = AParamParser.ParseRef<>(AContext, Data.MaxRepeatCount);
                        foreach (var VARIABLE in COLLECTION)
                        {
                            
                        }*/
                        break;
                }
                
            
               
            }

            protected override void OnChildrenJobFinish()
            {
                resetChildren();
            }
        }
    }
}