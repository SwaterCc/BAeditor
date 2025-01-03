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
            string operation1 = Data.isModify ? "修改 " : "创建 ";
            string operation2 = " = ";
            if (Data.isModify)
            {
                switch (Data.operationType)
                {
                    case EVariableOperationType.Reset:
                        operation2 = " = ";
                        break;
                    case EVariableOperationType.Add:
                        operation2 = $" = {Data.key} + ";
                        break;
                    case EVariableOperationType.Sub:
                        operation2 =$" = {Data.key} * ";
                        break;
                    case EVariableOperationType.Reverse:
                        operation2 =" bool取反 ";
                        break;
                }
            }
            
            return operation1 + Data.key + operation2 + Data.value;
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<VariableSettingWindow>(this);
        }
    }
}