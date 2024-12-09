using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : ATreeItem<RepeatNodeData>
    {
        public RepeatTreeItem(AbilityCycleTree tree, ATreeEditorNode data) : base(tree, data) { }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            throw new NotImplementedException();
        }

        protected override string getButtonText()
        {
            return "Foreach";
        }

        protected override void OnBtnClicked(Rect btnRect) { }
    }

    public class RepeatSettingWindow : ANodeSettingWindow<RepeatNodeData>
    {
        private ParameterField _maxCount;

        protected override void Init()
        {
            _maxCount = new ParameterField(TempData.MaxRepeatCount, "循环次数", typeof(int));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置循环次数");
            _maxCount.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}