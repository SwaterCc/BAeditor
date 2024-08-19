using System;
using System.Collections.Generic;
using System.Reflection;
using Battle.Skill;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public static class AbilityEditorHelper
    {
        /*private static void checkFieldType(FieldInfo fieldInfo, Type checkType,
            BAEditorShowLabelTag.ELabeType labeType,
            bool showMsg = true)
        {
            if (fieldInfo.FieldType != checkType && showMsg)
                Debug.LogWarning($"字段的类型和Label设置的类型不一致，字段类型为{fieldInfo.FieldType},Label类型为{labeType}");
        }

        public static (string, BAEditorShowLabelTag.ELabeType) GetFiledLabelAndType(FieldInfo fieldInfo)
        {
            string label = "NoInit";
            BAEditorShowLabelTag.ELabeType labeType = BAEditorShowLabelTag.ELabeType.None;
            foreach (var attr in fieldInfo.GetCustomAttributes())
            {
                if (attr.GetType() == typeof(BAEditorShowLabelTag))
                {
                    if (attr is not BAEditorShowLabelTag labelTag) continue;
                    label = labelTag.LabelText;
                    labeType = labelTag.LabeType;
                    break;
                }
            }

            return (label, labeType);
        }*/
        
        public static Dictionary<string, MethodInfo> AbilityMethodCache = new Dictionary<string, MethodInfo>();

        public static void InitMethodCache()
        {
            
        }
        
        

        /// <summary>
        /// 根据变量类型来创建对应类型的Filed
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="label"></param>
        /// <param name="updateValue"></param>
        /// <returns></returns>
        public static object DrawLabelByType(Type elementType, string label, object updateValue)
        {
            object afterValue = null;
            if (elementType == typeof(int))
            {
                afterValue = EditorGUILayout.IntField(label, (int)updateValue);
            }
            else if (elementType == typeof(long))
            {
                afterValue = EditorGUILayout.LongField(label, (long)updateValue);
            }
            else if (elementType == typeof(float))
            {
                afterValue = EditorGUILayout.FloatField(label, (float)updateValue);
            }
            else if (elementType == typeof(string))
            {
                afterValue = EditorGUILayout.TextField(label, (string)updateValue);
            }
            else if (elementType.IsEnum)
            {
                afterValue = EditorGUILayout.EnumPopup(label, (Enum)updateValue);
            }

            return afterValue;
        }
        
        
        public static object DrawLabelByType(SpecializationDataType elementType, string label, object updateValue)
        {
            object afterValue = null;
            if (elementType == SpecializationDataType.Int)
            {
                afterValue = EditorGUILayout.IntField(label, (int)updateValue);
            }
            else if (elementType == SpecializationDataType.Long)
            {
                afterValue = EditorGUILayout.LongField(label, (long)updateValue);
            }
            else if (elementType ==SpecializationDataType.Float)
            {
                afterValue = EditorGUILayout.FloatField(label, (float)updateValue);
            }
            else if (elementType == SpecializationDataType.String)
            {
                afterValue = EditorGUILayout.TextField(label, (string)updateValue);
            }
            else if (elementType == SpecializationDataType.Enum)
            {
                afterValue = EditorGUILayout.EnumPopup(label, (Enum)updateValue);
            }

            return afterValue;
        }
        
        
        /*/// <summary>
        /// 根据自定义特性BattleAbilityLabelTag 来创建不同的Field，不能创建列表，列表有单独的函数绘制
        /// </summary>
        /// <param name="classObj"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="label"></param>
        /// <param name="labeType"></param>
        public static Object DrawLabelAndUpdateValueByAttr(Object classObj, FieldInfo fieldInfo, string label,
            BAEditorShowLabelTag.ELabeType labeType)
        {
            Object afterUpdateValue = null;
            switch (labeType)
            {
                case BAEditorShowLabelTag.ELabeType.Int32:
                    checkFieldType(fieldInfo, typeof(int), labeType);
                    afterUpdateValue = SirenixEditorFields.IntField(label, (int)fieldInfo.GetValue(classObj));
                    break;
                case BAEditorShowLabelTag.ELabeType.Long:
                    checkFieldType(fieldInfo, typeof(long), labeType);
                    afterUpdateValue = SirenixEditorFields.LongField(label, (long)fieldInfo.GetValue(classObj));
                    break;
                case BAEditorShowLabelTag.ELabeType.String:
                    string text = fieldInfo.GetValue(classObj) == null ? "" : fieldInfo.GetValue(classObj).ToString();
                    afterUpdateValue = SirenixEditorFields.TextField(label, text);
                    break;
                case BAEditorShowLabelTag.ELabeType.Enum:
                    afterUpdateValue = SirenixEditorFields.EnumDropdown(label, (Enum)fieldInfo.GetValue(classObj));
                    break;
            }
            return afterUpdateValue;
        }*/
        
        /// <summary>
        /// 绘制列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="label"></param>
        /// <param name="getNewValue"></param>
        /// <param name="itemIsClass">TItem的类型不是基础类型，是类或结构体</param>
        /// <typeparam name="TItem"></typeparam>
        /*public static void DrawList<TItem>(ref List<TItem> list, string label, Func<TItem> getNewValue, bool itemIsClass = false)
        {
            int removeIdx = -1;
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            SirenixEditorGUI.Title(label, "", TextAlignment.Left, true);
            SirenixEditorGUI.EndBoxHeader();
            if (list == null)
            {
                list = new List<TItem>();
            }
            for (int idx = 0; idx < list.Count; ++idx)
            {
                EditorGUILayout.BeginHorizontal();
                if (SirenixEditorGUI.Button("删除", ButtonSizes.Gigantic))
                {
                    removeIdx = idx;
                }
                SirenixEditorGUI.BeginListItem();
                if (itemIsClass)
                {
                    foreach (var fieldInfo in (list[idx]).GetType().GetFields())
                    {
                        var attr = GetFiledLabelAndType(fieldInfo);
                        var obj = DrawLabelAndUpdateValueByAttr(list[idx], fieldInfo, attr.Item1, attr.Item2);
                        fieldInfo.SetValue(list[idx],obj);
                    }
                }
                else
                {
                    var afterValue = DrawLabelByType(typeof(TItem), label, list[idx]);
                    list[idx] = (TItem)afterValue;
                }
                SirenixEditorGUI.EndListItem();
                EditorGUILayout.EndHorizontal();
            }

            if (removeIdx >= 0)
            {
                list.RemoveAt(removeIdx);
                removeIdx = -1;
            }

            if (SirenixEditorGUI.Button("添加", ButtonSizes.Medium))
            {
                if (getNewValue != null)
                {
                    list.Add(getNewValue());
                }
            }
            SirenixEditorGUI.EndBox();
        }*/

        /// <summary>
        /// 绘制字典,未完成
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="label"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public static void DrawDict<TKey, TValue>(ref Dictionary<TKey, TValue> dict, string label)
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            SirenixEditorGUI.Title(label, "", TextAlignment.Left, true);
            SirenixEditorGUI.EndBoxHeader();
            SirenixEditorGUI.BeginVerticalPropertyLayout(new GUIContent("label"));
            foreach (var pair in dict)
            {
                SirenixEditorGUI.BeginIndentedHorizontal();
                var afterKey = DrawLabelByType(typeof(TKey), label, pair.Key);
                var afterValue = DrawLabelByType(typeof(TKey), label, pair.Value);
                dict[pair.Key] = (TValue)afterValue;
                SirenixEditorGUI.EndIndentedHorizontal();
            }

            SirenixEditorGUI.EndVerticalPropertyLayout();
            if (GUILayout.Button("添加配置"))
            {
            }

            SirenixEditorGUI.EndBox();
        }
    }
}