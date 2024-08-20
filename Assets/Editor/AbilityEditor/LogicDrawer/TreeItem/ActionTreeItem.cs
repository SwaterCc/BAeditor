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
            return new Color(1.5f, 0.5f, 0.6f);
        }

        protected override string getButtonText()
        {
            if (NodeData.ActionNodeData == null || NodeData.ActionNodeData.Length == 0)
                return "未初始化";
            
            string text = string.IsNullOrEmpty(NodeData.ActionNodeData[0].FuncName)
                ? "未初始化"
                : NodeData.ActionNodeData[0].FuncName;
            
            return text;
        }

        protected override void OnBtnClicked()
        {
            var action = new ParameterNode();
            action.Parse(NodeData.ActionNodeData, 0);
            FuncWindow.Open(action);
        }
    }
}