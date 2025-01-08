using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class BranchTreeItem : ATreeItem<BranchNodeData>
    {
        public BranchTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = Color.cyan;
            addCustomMenu("合并为分支组", ERightClickOperationType.JoinBranchGroup, null, null);
        }
        
        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType is 
                ERightClickOperationType.AddListenerChild or 
                ERightClickOperationType.AddGroupChild)
            {
                return ERightMenuState.NoShow;
            }
            
            if (this.HasParent<TimerTreeItem>() && info.OperationType == ERightClickOperationType.AddTimerChild)
                return ERightMenuState.Disable;

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrModifyTreeItem or FunctionTreeItem)
            {
                return false;
            }

            return true;
        }
        
        protected override string getButtonText()
        {
            string label = "If:";
            if (parent is BranchGroupTreeItem)
            {
                label = "Case:";
            }

            return label + " : " + Data.condition;
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<BranchSettingWindow>(this);
        }
    }
}