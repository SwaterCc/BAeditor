using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public static class AbilityEditorHelper
    {
        /// <summary>
        /// 根据变量类型来创建对应类型的Filed
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="label"></param>
        /// <param name="updateValue"></param>
        /// <returns></returns>
        public static object DrawLabelByType(Type elementType, string label, object updateValue)
        {
            if (elementType == null)
                return null;
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
            else if (elementType == typeof(bool))
            {
                afterValue = EditorGUILayout.Toggle(label, (bool)updateValue);
            }
            else if (elementType.IsEnum)
            {
                afterValue = EditorGUILayout.EnumPopup(label, (Enum)updateValue);
            }

            return afterValue;
        }
        
        public static string GetTypeAllName(Type type)
        {
            string typeName = type.ToString();
            string assemblyName = type.Assembly.ToString();
            return typeName + ", " + assemblyName;
        }
        
        
        public static void DrawIntList(List<int> list, string label,float labelWidth)
        {
            int removeIdx = -1;
            SirenixEditorGUI.BeginBox();
            if (list == null)
            {
                list = new List<int>();
                list.Add(0);
            }

            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            for (int idx = 0; idx < list.Count; ++idx)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-",GUILayout.Width(22)))
                {
                    removeIdx = idx;
                }
                list[idx] = SirenixEditorFields.IntField(list[idx],GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();
            }

            if (removeIdx >= 0)
            {
                list.RemoveAt(removeIdx);
            }

            if (GUILayout.Button("+", GUILayout.Width(22)))
            {
                list.Add(0);
            }
            SirenixEditorGUI.EndBox();
        }
    }
}