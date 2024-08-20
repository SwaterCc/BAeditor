using Battle.Def;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : AbilityLogicTreeItem
    {
      

        public CycleTreeItem(AbilityNodeData nodeData) : base(nodeData)
        {
            base.depth = 0;
            NodeData.Depth = 0;
        }

        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            return NodeData.CycleNodeData.ToString();
        }

        protected override void OnBtnClicked() { }
    }
}