using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Script.Editor
{
    /// <summary>
    /// 战斗编辑器主窗口
    /// </summary>
    public class BattleAbilityEditorMainWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/战斗编辑器")]
        private static void OpenWindow()
        {
            var window = GetWindow<BattleAbilityEditorMainWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 800);
        }
        
        
        protected override OdinMenuTree BuildMenuTree()
        {
            return null;
        }
    }
}