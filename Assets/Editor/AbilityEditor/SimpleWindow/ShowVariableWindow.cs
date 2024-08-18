using Battle.Def;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class ShowVariableWindow : EditorWindow
    {
        public static void OpenWindow(AbilityEditorMainWindow main, AbilityData curData)
        {
            var window = GetWindow<ShowVariableWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 300);
            window.titleContent = new GUIContent("显示当前存在的变量");
            window.init(main, curData);
        }

        private void init(AbilityEditorMainWindow main, AbilityData curData) { }
    }
}