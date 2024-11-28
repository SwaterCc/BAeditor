using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class ShowVariableWindow : EditorWindow
    {
        public static void OpenWindow(PowerEditorMainWindow main, AbilityData curData)
        {
            var window = GetWindow<ShowVariableWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 300);
            window.titleContent = new GUIContent("显示当前存在的变量");
            window.init(main, curData);
        }

        private void init(PowerEditorMainWindow main, AbilityData curData) { }
    }
}