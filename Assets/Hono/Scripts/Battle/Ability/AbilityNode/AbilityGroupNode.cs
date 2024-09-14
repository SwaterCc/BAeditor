

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private interface IGroupNodeProxy
        {
            public int GetGroupId();
            public void GroupBegin();
            public void GroupEnd();
        }
        
        private class AbilityGroupNode : AbilityNode,IGroupNodeProxy
        {
            private GroupNodeData _groupData;
            
            public AbilityGroupNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _groupData = data.GroupNodeData;
            }
            public override void DoJob()
            {
                
            }
            
            public int GetGroupId()
            {
                return _groupData.GroupId;
            }
            
            public void GroupBegin()
            {
                ((ExecutingCycle)_executor.State.Current).NextGroupId = -1;
                DoChildrenJob();
            }

            public void GroupEnd()
            {
               
            }
        }   
    }
}