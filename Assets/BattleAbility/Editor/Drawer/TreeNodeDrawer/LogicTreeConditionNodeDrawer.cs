using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeConditionNodeDrawer : LogicTreeNodeDrawer
    {
        public LogicTreeConditionNodeDrawer(LogicTreeDrawer treeDrawer,
            BattleAbilitySerializableTree.TreeNode treeNode, LogicTreeNodeDrawer parent) : base(treeDrawer, treeNode,
            parent)
        {
        }

        protected override void drawSelf()
        {
            var text = "条件按钮（后续要加事件部分参数预览）";
            treeButton(text, Color.green, LogicTreeConditionNodeWindow.OpenWindow);
        }

        public class LogicTreeConditionNodeWindow : OdinEditorWindow
        {
            public static void OpenWindow()
            {
                var window = GetWindow<LogicTreeEventNodeWindow>();
                window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
                window.titleContent = new GUIContent("条件节点设置");
            }
        }
    }
}