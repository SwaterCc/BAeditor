using BattleAbility.Editor;

namespace BattleAbility
{
    public class ActionTreeNode : TreeNodeBase
    {
        public override void Run()
        {
            
        }

        public ActionTreeNode(BattleAbilitySerializableTree.TreeNode nodeData) : base(nodeData)
        {
        }
    }
}