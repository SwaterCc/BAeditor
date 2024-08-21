using Battle.Def;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class StageTreeItem : AbilityLogicTreeItem
    {
        public StageTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public StageTreeItem(AbilityNodeData nodeData) : base(nodeData) { }
        protected override Color getButtonColor()
        {
            return new Color(0, 0.5f, 1f);
        }

        protected override string getButtonText()
        {
            return $"ID <{NodeData.StageNodeData.StageId}> :" + NodeData.StageNodeData.Desc;
        }

        protected override void OnBtnClicked()
        {
            StageNodeDataWindow.Open(NodeData);
        }
    }
}