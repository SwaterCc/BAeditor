using System;
using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityForeachNode : AbilityNode
        {
            private ForeachNodeData _foreachNodeData;
            
            public AbilityForeachNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _foreachNodeData = data.ForeachNodeData;
            }

            public override void DoJob()
            {
                switch (_foreachNodeData.ForeachObjType)
                {
                    case EForeachObjType.List:
                        
                        break;
                    case EForeachObjType.Dict:
                        break;
                }
            }
        }    
    }
    
}