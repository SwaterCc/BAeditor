using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
       private class AbilityCycleNode : AbilityNode
        {
            public AbilityCycleNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }
            public override void DoJob()
            {
                _executor.GoNext(ConfigId, GetNextNode());
            }
        }   
    }
}