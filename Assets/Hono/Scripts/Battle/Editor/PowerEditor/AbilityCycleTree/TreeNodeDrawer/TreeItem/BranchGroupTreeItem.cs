using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class BranchGroupTreeItem : ATreeItem<BranchGroupNodeData>
    {
        public BranchGroupTreeItem(AbilityCycleTree tree, ATreeEditorNode editorNode) : base(tree, editorNode)
        {
            ButtonBackGroundColor = Color.blue;
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildLimit)
            {
                return info.OperationType == ERightClickOperationType.AddBranchChild
                    ? ERightMenuState.Enable
                    : ERightMenuState.Disable;
            }

            return ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            return $"分支组<{Data.BelongGroupId}>";
        }

        protected override void OnBtnClicked(Rect btnRect) { }
    }
}