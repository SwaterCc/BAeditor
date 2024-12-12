using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class BranchGroupTreeItem : ATreeItem<BranchGroupNodeData>
    {
        public BranchGroupTreeItem(AbilityCycleTree tree, AEditorTreeNode node) : base(tree, node)
        {
            ButtonBackGroundColor = Color.blue;
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return info.OperationType == ERightClickOperationType.AddBranchChild
                    ? ERightMenuState.Enable
                    : ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrTreeItem or VariableTreeItem or ActionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            return true;
        }


        protected override string getButtonText()
        {
            return $"分支组<{Data.NodeId}>";
        }

        protected override void OnBtnClicked(Rect btnRect) { }
    }
}