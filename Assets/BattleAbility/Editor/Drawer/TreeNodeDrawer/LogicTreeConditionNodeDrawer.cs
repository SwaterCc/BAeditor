using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeConditionNodeDrawer : LogicTreeNodeDrawer
    {
        public LogicTreeConditionNodeDrawer(BattleAbilitySerializableTree treeData,
            BattleAbilitySerializableTree.TreeNode treeNode, LogicTreeNodeDrawer parent) : base(treeData, treeNode,
            parent)
        {
        }

        protected override void drawSelf()
        {
            var text = "条件按钮（后续要加事件部分参数预览）";
            if (GUILayout.Button(text, GUILayout.Width(150)))
            {
                //打开事件配置页面
                LogicTreeConditionNodeWindow.OpenWindow();
            }
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