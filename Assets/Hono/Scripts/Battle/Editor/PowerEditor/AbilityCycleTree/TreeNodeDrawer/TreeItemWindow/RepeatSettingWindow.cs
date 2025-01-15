using System;
using System.Collections;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.Utilities.Editor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class RepeatSettingWindow : ANodeSettingWindow<RepeatNodeData>
    {
        private AParamsField _repeatCount;
        private AParamsField _listField;

        protected override void Init()
        {
            _repeatCount = new AParamsField(TreeItem, TempData.repeatCount,  "设置循环次数", typeof(int));
            _listField = new AParamsField(TreeItem,   TempData.traverseList, "遍历列表", typeof(IList));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("循环设置");
            TempData.operationType= (ERepeatNodeOperationType)SirenixEditorFields.EnumDropdown("循环类型：", TempData.operationType);
            switch (TempData.operationType)
            {
                case ERepeatNodeOperationType.Repeat:
                    _repeatCount.Draw();
                    break;
                case ERepeatNodeOperationType.ETraverseList:
                    _listField.Draw();
                    break;
            }
            SirenixEditorGUI.EndBox();
        }
    }
}