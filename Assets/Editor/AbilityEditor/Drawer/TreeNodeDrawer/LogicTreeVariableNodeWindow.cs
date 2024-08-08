using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeVariableNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow(LogicTreeViewItem treeViewItem)
        {
            var window = GetWindow<LogicTreeVariableNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
            window.titleContent = new GUIContent("创建变量设置");
        }
    }
}