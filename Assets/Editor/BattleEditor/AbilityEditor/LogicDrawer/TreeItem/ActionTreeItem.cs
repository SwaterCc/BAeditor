using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class ActionTreeItem : AbilityLogicTreeItem
    {
        private ActionNodeData _actionNode;
        

        public ActionTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _actionNode = (ActionNodeData)_nodeData;
        }
        
        protected override Color getButtonColor()
        {
            return new Color(1.5f, 0.3f, 0.3f);
        }

        protected override string getButtonText()
        {
            if (_actionNode.Function.ParameterType != EParameterType.Function)
            {
                return "未初始化";
            }

            return _actionNode.Function.ToString();
        }

        protected override string getButtonTips()
        {
            return "执行无返回值的函数调用";
        }


        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("创建节点/获取返回值"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("删除"), false,
                Remove);
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = FuncWindow.Open(_actionNode.Function, EParameterValueType.Any, (param) => _actionNode.Function = param);
            SettingWindow.position = new Rect(btnRect.position, new Vector2(680, 480));
        }
    }
}