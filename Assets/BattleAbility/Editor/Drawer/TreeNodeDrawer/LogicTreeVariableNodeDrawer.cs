using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeVariableNodeDrawer : LogicTreeNodeDrawer
    {
        public LogicTreeVariableNodeDrawer(LogicTreeDrawer treeDrawer,
            BattleAbilitySerializableTree.TreeNode nodeData, LogicTreeNodeDrawer parent) : base(treeDrawer, nodeData,
            parent)
        {
        }

        protected override void drawSelf()
        {
            var text = "创建变量（后续要加事件部分参数预览）";
            treeButton(text, new Color(1, 0.5f, 0.1f), LogicTreeVariableNodeWindow.OpenWindow);
        }
    }

    public class LogicTreeVariableNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow()
        {
            var window = GetWindow<LogicTreeVariableNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            window.titleContent = new GUIContent("创建变量设置");
        }
    }
}