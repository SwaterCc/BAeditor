using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor.Graphs;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeEventNodeDrawer : LogicTreeNodeDrawer
    {
        public LogicTreeEventNodeDrawer(LogicTreeDrawer treeDrawer, BattleAbilitySerializableTree.TreeNode treeNode) : base(treeDrawer, treeNode,null)
        {
        }

        protected override void drawSelf()
        {
            var text = "事件按钮（后续要加事件部分参数预览）";
            treeButton(text, Color.cyan, LogicTreeEventNodeWindow.OpenWindow);
        }
    }

    public class LogicTreeEventNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow()
        {
            var window = GetWindow<LogicTreeEventNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            window.titleContent = new GUIContent("Event节点配置");
        }
    }
}