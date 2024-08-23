using System.Collections.Generic;
using AbilityRes;
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
        public AbilityEditorMainWindow MainWindow;
        public BattleAbilityRootView(AbilityEditorMainWindow mainWindow, AbilityDataList data)
        {
            Data = data;
            MainWindow = mainWindow;
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
            _isUsedMulitRemove = EditorGUILayout.Toggle("批量删除开启？", _isUsedMulitRemove,GUILayout.Width(20));
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
                EditorGUILayout.BeginHorizontal(GUILayout.Width(320));
                if (_isUsedMulitRemove)
                {
                    if (EditorGUILayout.Toggle(_removeList.Contains(item.Key),GUILayout.Width(40)))
                    {
                        if (!_removeList.Contains(item.Key))
                        {
                            _removeList.Add(item.Key);
                        }
                    }
                }
                EditorGUILayout.LabelField($"ID:{item.Key}",GUILayout.Width(50));
                EditorGUILayout.LabelField($"Name:{item.Value.name}",GUILayout.Width(150));
                EditorGUILayout.LabelField($"Desc:{item.Value.desc}");

                if (!_isUsedMulitRemove)
                {
                    if (GUILayout.Button("删除"))
                    {
                        _removeIdx = item.Key;
                    }
                }
                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
            }

            if (_removeIdx >= 0)
            {
                string path = AbilityEditorMainWindow.SKILL_DATA_PATH + $"{configItemList.Items[_removeIdx].configId}.asset";
                AssetDatabase.DeleteAsset(path);
                configItemList.Items.Remove(_removeIdx);
                _removeIdx = -1;
                this.ValueEntry.SmartValue.MainWindow.ForceMenuTreeRebuild();
            }
            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
        }
    }
}