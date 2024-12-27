using System;
using System.Collections.Generic;
using Editor.AbilityEditor.TreeItemWindow;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class VariableTreeItem : ATreeItem<VariableNodeData>
    {
        public VariableTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(1.2f, 0.3f, 1.95f);
        }


        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            //禁止添加子节点
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }


        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrModifyTreeItem or VariableTreeItem or ActionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            return true;
        }

        protected override string getButtonText()
        {
            string operation = Data.isModify ? "修改" : "创建";

          
            return operation + Data.key + " = ";
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<VariableSettingWindow>(this);
        }
    }
}