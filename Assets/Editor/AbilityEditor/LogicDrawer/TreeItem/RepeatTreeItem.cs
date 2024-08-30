
using Hono.Scripts.Battle;

using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : AbilityLogicTreeItem
    {
        public RepeatTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public RepeatTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            var rData = NodeData.RepeatNodeData;
            string a = rData.RepeatOperationType == ERepeatOperationType.OnlyRepeat
                ? $"循环次数{rData.MaxRepeatCount}"
                : $"for step = {rData.StartValue} ; count = {rData.StepCount} ; step + {rData.StepValue}";

            return a;
        }

        protected override void OnBtnClicked()
        {
            RepeatNodeDataWindow.Open(NodeData);
        }
    }
}