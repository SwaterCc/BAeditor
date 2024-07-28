using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeEventNodeDrawer : LogicTreeNodeDrawer
    {
        public LogicTreeEventNodeDrawer(BattleAbilitySerializableTree treeData, BattleAbilitySerializableTree.TreeNode treeNode) : base(treeData, treeNode,null)
        {
        }

        protected override void drawSelf()
        {
            var text = "事件按钮（后续要加事件部分参数预览）";
            if (GUILayout.Button(text, GUILayout.Width(150)))
            {
                //打开事件配置页面
                LogicTreeEventNodeWindow.OpenWindow();
            }
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