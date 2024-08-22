using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Editor.AbilityEditor.TreeItem
{
    public class TimerNodeDataWindow : BaseNodeWindow<TimerNodeDataWindow>, IWindowInit
    {
        protected override void onInit()
        {
            
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("配置计时器");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("第一次调用间隔");
           
            EditorGUILayout.EndHorizontal();
            SirenixEditorGUI.EndBox();
        }
    }
}