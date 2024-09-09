using System;
using System.Collections.Generic;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor {
	public static class AbilityEditorHelper {
		public static string SearchText = "";

		/// <summary>
		/// 根据变量类型来创建对应类型的Filed
		/// </summary>
		/// <param name="elementType"></param>
		/// <param name="label"></param>
		/// <param name="updateValue"></param>
		/// <returns></returns>
		public static object DrawLabelByType(Type elementType, string label, object updateValue) {
			if (elementType == null)
				return null;
			object afterValue = null;
			if (elementType == typeof(int)) {
				afterValue = EditorGUILayout.IntField(label, (int)updateValue);
			}
			else if (elementType == typeof(long)) {
				afterValue = EditorGUILayout.LongField(label, (long)updateValue);
			}
			else if (elementType == typeof(float)) {
				afterValue = EditorGUILayout.FloatField(label, (float)updateValue);
			}
			else if (elementType == typeof(string)) {
				afterValue = EditorGUILayout.TextField(label, (string)updateValue);
			}
			else if (elementType == typeof(bool)) {
				afterValue = EditorGUILayout.Toggle(label, (bool)updateValue);
			}
			else if (elementType.IsEnum) {
				if ((Enum.GetNames((elementType)).Length <= 6)) {
					afterValue = EditorGUILayout.EnumPopup(label, (Enum)updateValue);
				}
				else {
					EditorGUILayout.BeginHorizontal();
					var origin = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 38;
					SearchText = SirenixEditorFields.TextField("过滤器:", SearchText, GUILayout.Width(86));
					EditorGUIUtility.labelWidth = origin;
					var names = Enum.GetNames(elementType);
					var enumValues = Enum.GetValues(elementType);

					var nameList = new List<string>();
					var valueList = new List<int>();
					for (int index = 0; index < names.Length; index++) {
						string name = names[index];
						var value = (int)enumValues.GetValue(index);

						var lname = name.ToLower();
						if (lname.Contains(SearchText.ToLower())) {
							nameList.Add(name);
							valueList.Add(value);
						}

						if (value == (int)updateValue) {
							nameList.Add(name);
							valueList.Add(value);
						}
					}
					afterValue = EditorGUILayout.IntPopup(label, (int)updateValue, nameList.ToArray(),
						valueList.ToArray());
					
					EditorGUILayout.EndHorizontal();
				}
			}
			else if (elementType.IsSerializable) {
				if (SirenixEditorGUI.Button("编辑序列化数据", ButtonSizes.Medium)) {
					SerializableOdinWindow.Open(ref updateValue, elementType);
				}

				return updateValue;
			}

			return afterValue;
		}

		public static string GetTypeAllName(Type type) {
			string typeName = type.ToString();
			string assemblyName = type.Assembly.ToString();
			return typeName + ", " + assemblyName;
		}


		public static void DrawIntList(List<int> list, string label, float labelWidth) {
			int removeIdx = -1;
			SirenixEditorGUI.BeginBox();
			if (list == null) {
				list = new List<int>();
				list.Add(0);
			}

			EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
			for (int idx = 0; idx < list.Count; ++idx) {
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(22))) {
					removeIdx = idx;
				}

				list[idx] = SirenixEditorFields.IntField(list[idx], GUILayout.Width(30));
				EditorGUILayout.EndHorizontal();
			}

			if (removeIdx >= 0) {
				list.RemoveAt(removeIdx);
			}

			if (GUILayout.Button("+", GUILayout.Width(22))) {
				list.Add(0);
			}

			SirenixEditorGUI.EndBox();
		}
	}
}