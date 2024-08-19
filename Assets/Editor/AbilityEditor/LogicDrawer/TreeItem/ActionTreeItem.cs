using Battle.Def;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class ActionTreeItem : AbilityLogicTreeItem
    {
        public ActionTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public ActionTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.9f, 0.1f, 0.3f);
        }

        protected override string getButtonText()
        {
            return NodeData.ActionNodeData[0].FuncName;
        }

        protected override void OnBtnClicked()
        {
            var action = new ParameterNode();
            action.Parse(NodeData.ActionNodeData, 0);
            FuncWindow.Open(action);
        }
    }
}