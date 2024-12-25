using Editor.AbilityEditor;
using Editor.BattleEditor.AbilityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class ToolboxesInterface
    {
        [MenuItem("Power！！/帕瓦编辑器")]
        private static void OpenWindow()
        {
            var window = EditorWindow.GetWindow<PowerEditorMainWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1600, 900);
            window.titleContent = new GUIContent("MORE 帕瓦！！");
            AbilityFuncInfoCache.Init();
        }
    }
}