using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Core.Base {
    // 定义字典特性
    public class EffectPathDictionaryAttribute : Attribute
    {
        public bool ShowKeyLabel = true;
    }

// 自定义字典绘制器
    public class EffectPathDictionaryDrawer : OdinAttributeDrawer<EffectPathDictionaryAttribute, Dictionary<string, string>>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // 调用默认字典绘制
            this.CallNextDrawer(label);

            // 为每个value添加拖拽支持
            foreach (var entry in this.ValueEntry.SmartValue)
            {
                var rect = GUIHelper.GetCurrentLayoutRect();
                rect.xMin += EditorGUIUtility.labelWidth; // 调整输入框区域

                // 绘制路径输入框
                DrawPathField(rect, entry.Key, entry.Value);
            }
        }

        private void DrawPathField(Rect rect, string key, string path)
        {
            GameObject currentPrefab = !string.IsNullOrEmpty(path) ?
                AssetDatabase.LoadAssetAtPath<GameObject>(path) : null;

            // 创建拖拽区域
            EditorGUI.BeginChangeCheck();
            var newPrefab = (GameObject)SirenixEditorFields.UnityObjectField(rect,(string)
                                                                             null, // 不显示标签
                                                                             currentPrefab,
                                                                             typeof(GameObject),
                                                                             false);

            if (EditorGUI.EndChangeCheck() && newPrefab != null)
            {
                string newPath = AssetDatabase.GetAssetPath(newPrefab);
                if (PrefabUtility.GetPrefabAssetType(newPrefab) != PrefabAssetType.Regular)
                {
                    Debug.LogError($"{key} 必须选择有效预制体");
                    return;
                }
                this.ValueEntry.SmartValue[key] = newPath;
            }

            // 显示悬浮提示
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
            {
                GUI.Label(rect, new GUIContent("", $"当前路径: {path}"));
            }
        }
    }
}