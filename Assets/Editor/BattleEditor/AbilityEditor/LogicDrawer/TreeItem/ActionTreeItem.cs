using Editor.BattleEditor.AbilityEditor;
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
            return new Color(2f, 0.5f, 0.5f);
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
            return "执行函数调用";
        }
        
        protected override void buildMenu()
        {
            _menu = new GenericMenu();
            if (string.IsNullOrEmpty(_actionNode.Function.FuncName))
            {
                _menu.AddDisabledItem(new GUIContent("函数未初始化，无法获取返回值"));
            }
            else if(AbilityFunctionHelper.TryGetFuncInfo(_actionNode.Function.FuncName,out var funcInfo))
            {
                if (funcInfo.ReturnType == typeof(void))
                {
                    _menu.AddDisabledItem(new GUIContent("函数无返回值"));
                }
                else
                {
                    _menu.AddItem(new GUIContent("获取返回值"), false,
                        AddChild, (EAbilityNodeType.EVariableSetter));
                }
            }
            
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