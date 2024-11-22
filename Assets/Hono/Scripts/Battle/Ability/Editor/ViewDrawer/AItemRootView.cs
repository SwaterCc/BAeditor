using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AItemRootView
    {
        public Dictionary<string, AbilityData> Data;
        public AbilityEditorMainWindow MainWindow;
        public EAbilityType AbilityType;
        
        public AItemRootView(AbilityEditorMainWindow mainWindow,EAbilityType abilityType ,Dictionary<string, AbilityData> data)
        {
            AbilityType = abilityType;
            Data = data;
            MainWindow = mainWindow;
        }
    }

    /// <summary>
    /// Root 节点列出全部能力，然后管理批量删除之类的
    /// </summary>
    public class AItemRootViewDrawer : OdinValueDrawer<AItemRootView>
    {
        private List<int> _removeList = new List<int>();
        private bool _isUsedMulitRemove;
        
        private readonly Dictionary<EAbilityType, string> _abilityExFolders = new()
        {
	        { EAbilityType.Skill, AbilityAssetPath.SkillPath },
	        { EAbilityType.Buff, AbilityAssetPath.BuffPath },
	        { EAbilityType.Bullet, AbilityAssetPath.BuffPath },
        };
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var datas = this.ValueEntry.SmartValue.Data;
            string removePath = null;
            int removeId = -1;
            SirenixEditorGUI.BeginBox();
            /*_isUsedMulitRemove = EditorGUILayout.Toggle("批量删除开启？", _isUsedMulitRemove, GUILayout.Width(20));
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
            }*/
            
            SirenixEditorGUI.BeginVerticalList();


            foreach (var pair in datas)
            {
                SirenixEditorGUI.BeginListItem();
                EditorGUILayout.BeginHorizontal(GUILayout.Width(320));
                /*if (_isUsedMulitRemove)
                {
                    if (EditorGUILayout.Toggle(_removeList.Contains(item.Key), GUILayout.Width(40)))
                    {
                        if (!_removeList.Contains(item.Key))
                        {
                            _removeList.Add(item.Key);
                        }
                    }
                }*/

                EditorGUILayout.LabelField($"ID:{pair.Value.ID}", GUILayout.Width(50));
                EditorGUILayout.LabelField($"Name:{pair.Value.name}", GUILayout.Width(150));
                EditorGUILayout.LabelField($"Desc:{pair.Value.Desc}");

                if (GUILayout.Button("删除"))
                {
                    removePath = pair.Key;
                    removeId = pair.Value.ID;
                }

                EditorGUILayout.EndHorizontal();
                SirenixEditorGUI.EndListItem();
            }

            if (!string.IsNullOrEmpty(removePath))
            {
                AssetDatabase.DeleteAsset(removePath);

                if (_abilityExFolders.ContainsKey(ValueEntry.SmartValue.AbilityType)) {
	                var exPath = _abilityExFolders[ValueEntry.SmartValue.AbilityType] + "/" + removeId + ".asset";
	                AssetDatabase.DeleteAsset(exPath);
                }
                
                ValueEntry.SmartValue.MainWindow.ForceMenuTreeRebuild();
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();
        }
    }
}