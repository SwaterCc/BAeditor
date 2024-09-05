using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Editor.AbilityEditor.SimpleWindow
{
    public class FilterSettingWindow : OdinEditorWindow
    {
        public static void Open(ref FilterSetting filterSetting)
        {
            var window = GetWindow<FilterSettingWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(450, 500);
            window.titleContent = new GUIContent("创建Ability");
            window.init(ref filterSetting);
        }

        private void init(ref FilterSetting filterSetting)
        {
            filterSetting ??= new FilterSetting();
            Setting = filterSetting;
        }
        
        [OdinSerialize]
        [NonSerialized]
        [VerticalGroup("setting")]
        public FilterSetting Setting;

        [VerticalGroup("clear")]
        [Button("重置")]
        public void clear()
        {
            Setting = new FilterSetting();
        }

        [VerticalGroup("end")]
        [Button("保存")]
        public void Save()
        {
            Close();
        }
    }
}