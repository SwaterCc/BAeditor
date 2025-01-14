#region

using System;
using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private class AAttrNode : ANode<AttrModifyNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
                int attrValue = ParseInt(Data.value);

                switch (Data.commandType)
                {
                    case EAbilityCommandType.Permanent:
                        AContext.Actor.SetAttr(Data.attrType, attrValue);
                        break;
                    case EAbilityCommandType.UndoWhenAbilityEndCycle:
                        var cycleCmd = APool<AttrCommand>.Pool.Rent();
                        cycleCmd.SetAttr(AContext.Actor,Data.attrType,attrValue);
                        AContext._cycleCmdCollection.DoCommand(cycleCmd);
                        break;
                    case EAbilityCommandType.UndoWhenAbilityRemove:
                        var abilityCmd = APool<AttrCommand>.Pool.Rent();
                        abilityCmd.SetAttr(AContext.Actor,Data.attrType,attrValue);
                        AContext._cycleCmdCollection.DoCommand(abilityCmd);
                        break;
                }
            }

            public override void Recycle()
            {
                APool<AAttrNode>.Pool.Recycle(this);
            }
        }
    }
}