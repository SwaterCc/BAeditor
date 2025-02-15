#region

#endregion

using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class AAttrNode : ANode<AttrModifyNodeData>, IGPoolObject
        {
            public override void DoJob()
            {
                int attrValue = ParseInt(Data.value);

                switch (Data.commandType)
                {
                    case EAbilityCommandType.Permanent:
                        AContext.Unit.SetAttr(Data.attrType, attrValue);
                        break;
                    case EAbilityCommandType.UndoWhenAbilityEndCycle:
                        var cycleCmd = new AttrCommand();
                        cycleCmd.InitCommand(AContext.Unit, Data.attrType, attrValue);
                        AContext._cycleCmdCollection.DoCommand(cycleCmd);
                        break;
                    case EAbilityCommandType.UndoWhenAbilityRemove:
                        var abilityCmd = new AttrCommand();
                        abilityCmd.InitCommand(AContext.Unit, Data.attrType, attrValue);
                        AContext._cycleCmdCollection.DoCommand(abilityCmd);
                        break;
                }
            }

            public override void Recycle()
            {
                GPool<AAttrNode>.Pool.Recycle(this);
            }
        }
    }
}