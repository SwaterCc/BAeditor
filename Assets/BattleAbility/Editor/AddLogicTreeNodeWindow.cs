using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class AddLogicTreeNodeWindow : OdinEditorWindow
    {
        public static void OpenWindow(ENodeType nodeType)
        {
            var window = GetWindow<AddLogicTreeNodeWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 300);
            window.titleContent = new GUIContent("选择节点");
        }

        [Button("创建事件节点")]
        public void createBtn()
        {
            
        }
    }
}