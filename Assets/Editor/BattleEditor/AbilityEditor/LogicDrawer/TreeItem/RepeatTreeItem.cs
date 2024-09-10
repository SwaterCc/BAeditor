
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
            
            return "";
        }

        protected override string getItemEffectInfo()
        {
            return "循环节点，循环指定次数";
        }

        protected override void OnBtnClicked()
        {
            RepeatNodeDataWindow.Open(NodeData);
        }
    }
}