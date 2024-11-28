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
        public static void Open(ref RangeFilterSetting rangeFilterSetting)
        {
            var window = GetWindow<FilterSettingWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(450, 500);
            window.titleContent = new GUIContent("设置筛选器");
            window.init(ref rangeFilterSetting);
        }

        private void init(ref RangeFilterSetting rangeFilterSetting)
        {
            rangeFilterSetting ??= new RangeFilterSetting();
            Setting = rangeFilterSetting;
        }
        
        [VerticalGroup("setting")]
        public RangeFilterSetting Setting;

        [VerticalGroup("clear")]
        [Button("重置")]
        public void clear()
        {
            Setting = new RangeFilterSetting();
        }

        [VerticalGroup("end")]
        [Button("保存")]
        public void Save()
        {
            Close();
        }
    }
}