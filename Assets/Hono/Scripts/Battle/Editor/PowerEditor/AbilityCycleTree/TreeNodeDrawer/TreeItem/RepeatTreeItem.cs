using System;
using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : ATreeItem<RepeatNodeData>
    {
        public RepeatTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(2,0.9f,0.1f);
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType is 
                ERightClickOperationType.AddGroupChild or 
                ERightClickOperationType.AddListenerChild or 
                ERightClickOperationType.AddTimerChild)
            {
                return ERightMenuState.Disable;
            }

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrModifyTreeItem  or ActionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            return true;
        }
        
        protected override string getButtonText()
        {
            return "Foreach";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<RepeatSettingWindow>(this);
        }
    }
}