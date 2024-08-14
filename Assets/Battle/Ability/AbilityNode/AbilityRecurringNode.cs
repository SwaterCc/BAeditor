using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityRepeatNode : AbilityNode
        {
            private ForeachNodeData _foreachNodeData;
            
            public AbilityRepeatNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
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