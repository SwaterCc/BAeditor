using System;
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : ATreeItem<CycleNodeData>
    {
        public CycleTreeItem(AbilityCycleTree tree, ATreeEditorNode data) : base(tree, data)
        {
            ButtonWidth = 200;
            ButtonTextAnchor = TextAnchor.MiddleCenter;
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            return info.OperationType == ERightClickOperationType.RemoveSelf
                ? ERightMenuState.Disable
                : ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            string desc = "";
            switch (Data.cycleNodeData)
            {
                case EAbilityCycle.Init:
                    desc = "Init(初始化阶段)";
                    break;
                case EAbilityCycle.PreExecute:
                    desc = "PreExecute(预启动阶段)";
                    break;
                case EAbilityCycle.Executing:
                    desc = "Executing(执行阶段)";
                    break;
                case EAbilityCycle.EndExecute:
                    desc = "EndExecute(执行结束阶段)";
                    break;
            }

            return desc;
        }

        protected override void OnBtnClicked(Rect btnRect) { }
    }
}