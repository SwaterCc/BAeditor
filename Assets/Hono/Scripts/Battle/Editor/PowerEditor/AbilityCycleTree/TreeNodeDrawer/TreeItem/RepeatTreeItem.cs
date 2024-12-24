using System;
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
        public RepeatTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data) { }

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
            if (newParent is AttrTreeItem or VariableTreeItem or ActionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            return true;
        }
        
        protected override string getButtonText()
        {
            return "Foreach";
        }

        protected override void OnBtnClicked(Rect btnRect) { }
    }

    public class RepeatSettingWindow : ANodeSettingWindow<RepeatNodeData>
    {
        private AParamsField _maxCount;

        protected override void Init()
        {
            _maxCount = new AParamsField<RefInt>(TempData.MaxRepeatCount, "循环次数");
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置循环次数");
            _maxCount.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}