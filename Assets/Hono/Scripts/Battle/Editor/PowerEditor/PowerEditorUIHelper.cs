using System;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor.AbilityEditor
{
    public static class PowerEditorUIHelper
    {
        /// <summary>
        /// 绘制简单字段，支持类型string，int，bool，float，enum
        /// </summary>
        /// <param name="field"></param>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <param name="drawSplitLine"></param>
        public static void DrawSimpleField<T>(ref T field, string label, object value, bool drawSplitLine = true)
        {
            DrawSimpleField(ref field, new GUIContent(label), value, drawSplitLine);
        }

        /// <summary>
        /// 绘制简单字段，支持类型string，int，bool，float，enum
        /// </summary>
        /// <param name="field"></param>
        /// <param name="label"></param>
        /// <param name="tooltip"></param>
        /// <param name="value"></param>
        /// <param name="drawSplitLine"></param>
        public static void DrawSimpleField<T>(ref T field,
            string label,
            string tooltip,
            object value,
            bool drawSplitLine = true)
        {
            DrawSimpleField(ref field, new GUIContent(label, tooltip), value, drawSplitLine);
        }

        /// <summary>
        /// 绘制简单字段，支持类型string，int，bool，float，enum
        /// </summary>
        /// <param name="field">赋值字段</param>
        /// <param name="label"></param>
        /// <param name="value">当前值</param>
        /// <param name="drawSplitLine">绘制分割线</param>
        public static void DrawSimpleField<T>(ref T field, GUIContent label, object value, bool drawSplitLine = true)
        {
            var type = field.GetType();
            object temp = null;
            if (type == typeof(string))
            {
                temp = SirenixEditorFields.TextField(label, (string)value);
            }
            else if (type == typeof(int))
            {
                temp = SirenixEditorFields.IntField(label, (int)value);
            }
            else if (type == typeof(float))
            {
                temp = SirenixEditorFields.FloatField(label, (float)value);
            }
            else if (type == typeof(bool))
            {
                var select =
                    SirenixEditorFields.Dropdown(new GUIContent(""), value.ToString(), new[] { "true", "false" });
                temp = bool.Parse(select);
            }
            else if (type == typeof(Enum))
            {
                temp = SirenixEditorFields.EnumDropdown(label, (Enum)value);
            }

            field = (T)temp;

            if (drawSplitLine)
            {
                SirenixEditorGUI.HorizontalLineSeparator();
            }
        }

        /// <summary>
        /// 绘制Icon
        /// </summary>
        /// <param name="iconField"></param>
        public static void DrawIconField(ref string iconField)
        {
            // 尝试加载Sprite
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconField);

            // 绘制Sprite选择框
            sprite = (Sprite)SirenixEditorFields.UnityObjectField(sprite, typeof(Sprite), false);

            // 如果选择了新的Sprite，更新路径
            if (sprite == null) return;
            string newPath = AssetDatabase.GetAssetPath(sprite);
            iconField = newPath;
        }
        
        /// <summary>
        /// 绘制简单 int List
        /// </summary>
        /// <param name="list"></param>
        /// <param name="label"></param>
        /// <param name="labelWidth"></param>
        public static void DrawIntList(List<int> list, string label, float labelWidth)
        {
            int removeIdx = -1;
            list ??= new List<int> { 0 };
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            for (int idx = 0; idx < list.Count; ++idx)
            {
                if (GUILayout.Button("-", GUILayout.Width(22)))
                {
                    removeIdx = idx;
                }

                list[idx] = SirenixEditorFields.IntField(list[idx], GUILayout.Width(30));
            }

            if (removeIdx >= 0)
            {
                list.RemoveAt(removeIdx);
            }

            if (GUILayout.Button("+", GUILayout.Width(22)))
            {
                list.Add(0);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}