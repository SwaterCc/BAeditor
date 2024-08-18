using System.Collections.Generic;
using BattleAbility.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{

    public class BattleAbilityRootView
    {
        public AbilityDataList Data;

        public BattleAbilityRootView(AbilityDataList data)
        {
            Data = data;
        }
    }
    
    /// <summary>
    /// Root 节点列出全部能力，然后管理批量删除之类的
    /// </summary>
    public class BattleAbilityRootViewDrawer : OdinValueDrawer<BattleAbilityRootView>
    {
        private List<int> _removeList = new List<int>();
        private bool _isUsedMulitRemove;
        private int _removeIdx = -1;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var configItemList = this.ValueEntry.SmartValue.Data;
            
            SirenixEditorGUI.BeginBox();
            _isUsedMulitRemove = EditorGUILayout.Toggle("批量删除开启？", _isUsedMulitRemove,GUILayout.Width(80));
            if (_isUsedMulitRemove)
            {
                if (SirenixEditorGUI.Button("删除选中项", ButtonSizes.Medium))
                {
                    foreach (var id in _removeList)
                    {
                        configItemList.Items.Remove(id);
                    }
                    _removeList.Clear();
                }
            }
            SirenixEditorGUI.ToolbarSearchField("通过Id搜索");

            SirenixEditorGUI.BeginVerticalList();

            foreach (var item in configItemList.Items)
            {
                SirenixEditorGUI.BeginListItem();
                EditorGUILayout.BeginHorizontal();
                if (_isUsedMulitRemove)
                {
                    if (EditorGUILayout.Toggle(_removeList.Contains(item.Key)))
                    {
                        if (!_removeList.Contains(item.Key))
                        {
                            _removeList.Add(item.Key);
                        }
                    }
                }
                EditorGUILayout.LabelField($"ID:{item.Key}");
                EditorGUILayout.LabelField($"Name:{item.Value.name}");
                EditorGUILayout.LabelField($"Desc:{item.Value.desc}");

                if (!_isUsedMulitRemove)
                {
                    if (SirenixEditorGUI.Button("删除", ButtonSizes.Medium))
                    {
                        _removeIdx = item.Key;
                    }
                }
                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
            }

            if (_removeIdx >= 0)
            {
                configItemList.Items.Remove(_removeIdx);
                _removeIdx = -1;
            }
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
            
            
        }
    }
}