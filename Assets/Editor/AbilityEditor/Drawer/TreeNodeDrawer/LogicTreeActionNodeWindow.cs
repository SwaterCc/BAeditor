using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeActionNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow(LogicTreeViewItem treeViewItem)
        {
            var window = GetWindow<LogicTreeActionNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            window.titleContent = new GUIContent("Action节点设置");
        }
    }
}