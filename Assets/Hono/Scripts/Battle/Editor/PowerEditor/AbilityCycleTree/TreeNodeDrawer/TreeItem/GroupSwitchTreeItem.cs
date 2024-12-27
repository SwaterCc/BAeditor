using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupSwitchTreeItem : ATreeItem<GroupSwitchNodeData>
    {
        public GroupSwitchTreeItem(AbilityCycleTree tree, AEditorTreeNode node) : base(tree, node) { }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            var text = "";
            if (Data.switchGroupNow)
            {
                text = "立刻切换Group到" + Data.nextGroupId;
            }
            else
            {
                text = "设置下个GroupId为：" + Data.nextGroupId;
            }

            return text;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrModifyTreeItem  or ActionTreeItem)
            {
                return false;
            }

            return true;
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<GroupSwitchSettingWindow>(this);
        }
    }
}