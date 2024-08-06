using BattleAbility.Editor;

namespace BattleAbility
{
    public class ActionTreeNode : TreeNodeBase
    {
        public ActionTreeNode(BattleAbilityBlock block, BattleAbilitySerializableTree.TreeNode treeNode) : base(block,treeNode)
        {
        }
        
        public override void RunLogic()
        {
            
        }
    }
}