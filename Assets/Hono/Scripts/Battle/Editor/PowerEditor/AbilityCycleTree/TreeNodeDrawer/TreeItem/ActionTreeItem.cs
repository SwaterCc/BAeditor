using Editor.AbilityEditor.TreeItemWindow;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class ActionTreeItem : ATreeItem<ActionNodeData>
    {
        public ActionTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(2f, 0.5f, 0.5f);
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
            if (newParent is AttrModifyTreeItem or ActionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            return true;
        }

        protected override string getButtonText()
        {
            string text = "";
            if (Data.action.paramType != EParamType.Function)
            {
                return "未指定函数！";
            }

            text = Data.action.ToString();

            if (Data.isCreateVariable)
            {//获取变量
                var varName = string.IsNullOrEmpty(Data.returnValueKey) ? "变量名未设置" : Data.returnValueKey;
                text = varName + " = " + text;
            }
            
            return text;
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<ActionSettingWindow>(this);
        }
    }
}