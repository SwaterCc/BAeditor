using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityStageNode : AbilityNode
        {
            public AbilityStageNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }
            public override void DoJob()
            {
                
            }
        }   
    }
}