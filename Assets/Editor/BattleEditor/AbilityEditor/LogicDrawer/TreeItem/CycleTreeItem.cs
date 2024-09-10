using Hono.Scripts.Battle;
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
            return NodeData.allowEditCycleNodeData.ToString();
        }

        protected override string getItemEffectInfo()
        {
            return "周期根节点，仅做展示";
        }

        protected override void OnBtnClicked() { }
    }
}