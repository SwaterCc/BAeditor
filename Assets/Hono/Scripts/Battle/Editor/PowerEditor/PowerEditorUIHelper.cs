using System;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor.AbilityEditor
{
    public static class PowerEditorUIHelper
    {
        private static readonly float _simpleFieldLineHeight = 25f;
        private static float _simpleFieldLineHeightChangeValue = 0f;
        private static int _fontSize = 18;
        private static int _fontSizeChange = 0;

        public static float SimpleFieldLineHeight
        {
            get => _simpleFieldLineHeightChangeValue > 0 ? _simpleFieldLineHeightChangeValue : _simpleFieldLineHeight;
            set => _simpleFieldLineHeightChangeValue = value;
        }

        public static int FontSize
        {
            get => _fontSizeChange > 0 ? _fontSizeChange : _fontSize;
            set => _fontSizeChange = value;
        }

        public static void ResetLineHeight()
        {
            _simpleFieldLineHeightChangeValue = 0;
        }

        public static void ResetFontSize()
        {
            _fontSizeChange = 0;
        }

        /// <summary>
        /// 绘制简单字段，支持类型string，int，bool，float，enum
        /// </summary>
        /// <param name="field"></param>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <param name="drawSplitLine"></param>
        public static void DrawSimpleField<T>(ref T field, string label, object value, bool drawSplitLine = false)
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
            bool drawSplitLine = false)
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
        public static void DrawSimpleField<T>(ref T field, GUIContent label, object value, bool drawSplitLine = false)
        {
            var type = typeof(T);
            object temp = null;

            if (drawSplitLine)
            {
                SirenixEditorGUI.BeginListItem();
            }

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
                temp = BoolDropField(label, (bool)value);
            }
            else if (type.IsEnum)
            {
                temp = SirenixEditorFields.EnumDropdown(label, (Enum)value);
            }

            field = (T)temp;

            if (drawSplitLine)
            {
                SirenixEditorGUI.EndListItem();
            }
        }

        /// <summary>
        /// 绘制带有下拉框的bool字段
        /// </summary>
        /// <param name="content"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool BoolDropField(GUIContent content, bool value)
        {
            var select =
                SirenixEditorFields.Dropdown(content, value.ToString(), new[] { "true", "false" });
            return bool.Parse(select);
        }

        public static bool BoolDropField(string label, bool value)
        {
            return BoolDropField(new GUIContent(label), value);
        }

        public static bool BoolDropField(string label, string toolTip, bool value)
        {
            return BoolDropField(new GUIContent(label, toolTip), value);
        }

        /// <summary>
        /// 绘制Icon
        /// </summary>
        /// <param name="iconField"></param>
        /// <param name="height"></param>
        public static string DrawIconField(string iconField, float height = 33f)
        {
            // 尝试加载Sprite
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconField);

            // 绘制Sprite选择框
            sprite = (Sprite)SirenixEditorFields.UnityObjectField(new GUIContent("Icon"),
                                                                  sprite, typeof(Sprite), false);

            // 如果选择了新的Sprite，更新路径
            if (sprite == null) return "";
            string newPath = AssetDatabase.GetAssetPath(sprite);
            return newPath;
        }

        /// <summary>
        /// GenericMenu添加Item
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="label"></param>
        /// <param name="function"></param>
        /// <param name="isDisable"></param>
        public static void AddMenuItem(this GenericMenu menu,
            string label,
            GenericMenu.MenuFunction function,
            bool isDisable = false)
        {
            if (!isDisable)
            {
                menu.AddItem(new GUIContent(label), false, function);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(label));
            }
        }

        /// <summary>
        /// GenericMenu添加Item
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="label"></param>
        /// <param name="function"></param>
        /// <param name="param"></param>
        /// <param name="isDisable"></param>
        public static void AddMenuItem(this GenericMenu menu,
            string label,
            GenericMenu.MenuFunction2 function,
            object param,
            bool isDisable = false)
        {
            if (!isDisable)
            {
                menu.AddItem(new GUIContent(label), false, function, param);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(label));
            }
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