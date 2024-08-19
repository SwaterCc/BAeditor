using Battle.Def;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : AbilityLogicTreeItem
    {
        public CycleTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public CycleTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return Color.black;
        }

        protected override string getButtonText()
        {
            return "隐藏节点";
        }

        protected override void OnBtnClicked() { }
    }
}