using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeActionNodeDrawer : LogicTreeNodeDrawer
    {
        public LogicTreeActionNodeDrawer(BattleAbilitySerializableTree treeData,
            BattleAbilitySerializableTree.TreeNode treeNode, LogicTreeNodeDrawer parent) : base(treeData, treeNode,
            parent)
        {
        }

        protected override void drawSelf()
        {
            var text = "Action节点（后续要加事件部分参数预览）";
            if (GUILayout.Button(text, GUILayout.Width(150)))
            {
                //打开事件配置页面
                LogicTreeActionNodeWindow.OpenWindow();
            }
        }
    }

    public class LogicTreeActionNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow()
        {
            var window = GetWindow<LogicTreeActionNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            window.titleContent = new GUIContent("Action节点设置");
        }
    }
}