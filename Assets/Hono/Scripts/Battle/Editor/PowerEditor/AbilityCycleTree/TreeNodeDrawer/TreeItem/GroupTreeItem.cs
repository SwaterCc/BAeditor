using System;
using System.Collections.Generic;
using Editor.AbilityEditor.TreeItemWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    /// <summary>
    /// 仅允许放置在CycleItem下，声明一个组，该组的子节点会在ability执行完该周期的所有节点后再开始执行，起始组由
    /// </summary>
    public class GroupTreeItem : ATreeItem<GroupNodeData>
    {
        public GroupTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(0, 0.5f, 1.5f);
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType is 
                ERightClickOperationType.AddGroupChild or 
                ERightClickOperationType.AddListenerChild)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }
        
        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is not CycleTreeItem)
            {
                return false;
            }

            return true;
        }
        
        protected override string getButtonText()
        {
            return $"GroupID <{Data.groupId}> ";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<GroupSettingWindow>(this);
        }
    }
}