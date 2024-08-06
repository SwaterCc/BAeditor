using BattleAbility.Editor;

namespace BattleAbility
{
    public class ConditionTreeNode : TreeNodeBase
    {
        public ConditionTreeNode(BattleAbilityBlock block, BattleAbilitySerializableTree.TreeNode treeNode) : base(block,treeNode)
        {
        }

        public override void RunLogic()
        {
          
        }

        public override TreeNodeBase GetNext()
        {//条件节点会根据执行逻辑确定是否走下一个if
            throw new System.NotImplementedException();
        }
    }
}