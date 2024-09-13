using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class ActionTreeItem : AbilityLogicTreeItem
    {
        public ActionTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        private readonly ActionNodeData _actionNodeData;

        private ParameterMaker _funcMaker;
        
        public ActionTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _actionNodeData = (ActionNodeData)nodeData;
            _funcMaker = new ParameterMaker();
            if (_actionNodeData.Function == null || _actionNodeData.Function.Length == 0)
            {//函数默认给初始值
                _funcMaker.InitFunc("NothingToDo");
            }
            else
            {
                int start = 0;
                _funcMaker.Parse(_actionNodeData.Function,ref start);
            }
        }

        protected override Color getButtonColor()
        {
            return new Color(1.5f, 0.3f, 0.3f);
        }

        protected override string getButtonText()
        {
            if ( _actionNodeData.Function == null || _actionNodeData.Function.Length == 0)
                return "未初始化";

            return "";
        }

        protected override string getButtonTips()
        {
            return "执行函数";
        }

        public override GenericMenu GetGenericMenu()
        {
            var menu = new GenericMenu();
            if (_nodeData.ChildrenIds.Count == 0)
            {
                menu.AddItem(new GUIContent("创建节点/获取返回值"), false,
                    AddNode, EAbilityNodeType.EVariableControl);
            }
            return menu;
        }

        protected override void OnBtnClicked()
        {
            //直接调用FuncWindow
        }
    }
}