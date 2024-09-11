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
        private ParameterMaker _actionFunc;
        public ActionTreeItem(AbilityNodeData nodeData) : base(nodeData)
        {
            _actionFunc = new ParameterMaker();
            ParameterMaker.Init(_actionFunc,NodeData.ActionNodeData);
        }
        
        protected override Color getButtonColor()
        {
            return new Color(1.5f, 0.3f, 0.3f);
        }

        protected override string getButtonText()
        {
            if (NodeData.ActionNodeData == null || NodeData.ActionNodeData.Length == 0 || string.IsNullOrEmpty(NodeData.ActionNodeData[0].FuncName))
                return "未初始化";
            
            return _actionFunc.ToString();
        }

        protected override string getItemEffectInfo()
        {
            return "执行无返回值的函数调用";
        }

        protected override void OnBtnClicked()
        {
            if (SettingWindow == null)
            {
                var settingWindow = FuncWindow.CreateInstance<FuncWindow>();
                settingWindow.Init(_actionFunc, EFuncCacheFlag.Action);
                settingWindow.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
                settingWindow.FromString = $"→ ActionNodeId<{DrawCount}>";
                SettingWindow = settingWindow;
            }
            SettingWindow.Show();
            SettingWindow.Focus();
        }
    }
}