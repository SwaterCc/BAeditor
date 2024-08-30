using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class ActionTreeItem : AbilityLogicTreeItem
    {
        public ActionTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public ActionTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        private ParameterMaker _actionFunc;
        
        protected override Color getButtonColor()
        {
            return new Color(1.5f, 0.3f, 0.3f);
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
            _actionFunc = new ParameterMaker();
            
            ParameterMaker.Init(_actionFunc,NodeData.ActionNodeData);
            
            _actionFunc.OnSave = (parameters) => { NodeData.ActionNodeData = parameters; };

            FuncWindow.Open(_actionFunc, EFuncCacheFlag.Action);
        }
    }
}