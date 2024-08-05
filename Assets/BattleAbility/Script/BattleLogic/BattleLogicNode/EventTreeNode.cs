using BattleAbility.Editor;

namespace BattleAbility
{
    public class EventTreeNode : TreeNodeBase
    {
       
        public EventTreeNode(BattleAbilitySerializableTree.TreeNode nodeData) : base(nodeData)
        {
            
            
        }
        
        public override void Run()
        {
            
        }
        
        /// <summary>
        /// 事件触发
        /// </summary>
        public void OnEventFired()
        {
            //节点执行逻辑
            
            
        }
    }
}