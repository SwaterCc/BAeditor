using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeConditionNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow(LogicTreeViewItem treeViewItem)
        {
            var window = GetWindow<LogicTreeEventNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            window.titleContent = new GUIContent("条件节点设置");
        }
    }
}