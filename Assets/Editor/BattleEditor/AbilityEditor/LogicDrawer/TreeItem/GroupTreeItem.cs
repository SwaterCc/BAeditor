
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupTreeItem : AbilityLogicTreeItem
    {
        public GroupTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public GroupTreeItem(AbilityNodeData nodeData) : base(nodeData) { }
        protected override Color getButtonColor()
        {
            return new Color(0, 0.5f, 1.5f);
        }

        protected override string getButtonText()
        {
            return $"GroupID <{NodeData.GroupNodeData.GroupId}> " + NodeData.GroupNodeData.Desc;
        }

        protected override string getItemEffectInfo()
        {
            return "Group节点的子节点 是在Group被调用后的下一帧开始执行";
        }

        protected override void OnBtnClicked()
        {
            GroupNodeDataWindow.Open(NodeData);
        }
    }
}