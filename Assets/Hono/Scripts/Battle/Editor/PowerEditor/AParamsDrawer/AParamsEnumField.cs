using System;
using Hono.Scripts.Battle.Base;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AParamsEnumField<T> : AParamsField where T : Enum
    {
        public AParamsEnumField(ATreeItem node, AParams aParams, string label) : base(node, aParams, label) { }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(Label), GUILayout.Width(100));
            GUILayout.Button("Enum", GUILayout.Width(80));
            Params.Value ??= 0;
            Params.Value = SirenixEditorFields.EnumDropdown((T)Params.Value);
            EditorGUIUtility.labelWidth = old;
            EditorGUILayout.EndHorizontal();
        }
    }
}