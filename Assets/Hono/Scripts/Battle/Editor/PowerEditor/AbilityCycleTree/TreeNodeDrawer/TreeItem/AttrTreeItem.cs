using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AttrTreeItem : ATreeItem<AttrNodeData>
    {
        public AttrTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
          ButtonBackGroundColor =Color.magenta;
        }

        
        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return ERightMenuState.NoShow;
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
            string attrName = "";
            if (Enum.GetName(typeof(EAttrType), Data.attrType) == null)
            {
                attrName = "未设置";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), Data.attrType);
            }

            return "设置属性 (属性Id:" + attrName + ") = " + Data.Value;
        }
        

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<AttrSettingWindow>(this);
        }
    }
}