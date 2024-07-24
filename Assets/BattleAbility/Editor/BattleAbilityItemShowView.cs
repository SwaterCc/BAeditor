using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力配置界面 （基础配置 + 逻辑配置）
    /// </summary>
    
    public class BattleAbilityItemShowView
    {
        public BattleAbilityBaseConfig BaseConfig;
        
        public BattleAbilityLogicTreeView LogicTreeView;
        
        public BattleAbilityItemShowView(BattleAbilityBaseConfig baseConfig, BattleAbilitySerializableTree serializableTree)
        {
            BaseConfig = baseConfig;
            LogicTreeView = new BattleAbilityLogicTreeView(serializableTree);
        }

        public string GetOdinMenuTreeItemLabel()
        {
            return $"{BaseConfig.ConfigId}->{BaseConfig.Name}";
        }
        
    }

    public class BattleAbilityItemShowViewDrawer : OdinValueDrawer<BattleAbilityItemShowView>
    {
        private bool foldout = false;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var itemShowView = this.ValueEntry.SmartValue;

            EditorGUILayout.BeginScrollView(Vector2.zero, false, true);
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginFadeGroup(1.0f);
            SirenixEditorGUI.BeginBoxHeader();
            foldout = EditorGUILayout.Foldout(foldout,"基础配置");
            SirenixEditorGUI.EndBoxHeader();
            if (foldout)
            {
                EditorGUILayout.LabelField("------------------------------------------------"); 
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------"); 
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
            }
            SirenixEditorGUI.EndFadeGroup();
            SirenixEditorGUI.EndBox();
            
            
            
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            foldout = SirenixEditorGUI.Foldout(foldout,"逻辑开发");
            SirenixEditorGUI.EndBoxHeader();
            if (foldout)
            {
                EditorGUILayout.LabelField("------------------------------------------------"); 
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------"); 
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
                EditorGUILayout.LabelField("------------------------------------------------");
            }
            SirenixEditorGUI.EndBox();
            
            EditorGUILayout.EndScrollView();
        }
    }
}