using Hono.Scripts.Battle;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
     public static class ParameterNodeDraw
    {
        public static void Draw(this ParameterMaker maker,string formStr = "")
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;
            if (GUILayout.Button("▼", GUILayout.Width(22)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("调用函数"), false, FuncWindow.OpenVariableToFunc, (maker,formStr));
                menu.AddItem(new GUIContent("使用基础类型/int"), false, maker.ChangeToValueType,
                    AbilityEditorHelper.GetTypeAllName(typeof(int)));
                menu.AddItem(new GUIContent("使用基础类型/float"), false, maker.ChangeToValueType,
                    AbilityEditorHelper.GetTypeAllName(typeof(float)));
                menu.AddItem(new GUIContent("使用基础类型/bool"), false, maker.ChangeToValueType,
                    AbilityEditorHelper.GetTypeAllName(typeof(bool)));
                menu.AddItem(new GUIContent("使用基础类型/string"), false, maker.ChangeToValueType,
                    AbilityEditorHelper.GetTypeAllName(typeof(string)));
                menu.ShowAsContext();
            }
            EditorGUIUtility.labelWidth = 86;
            if (maker.Self.IsFunc)
            {
                string label = string.IsNullOrEmpty(maker.Self.ParamName) ? "调用：" : maker.Self.ParamName;
                EditorGUILayout.LabelField(label, GUILayout.Width(30));
                if (GUILayout.Button(maker.ToString()))
                {
                    //打开函数界面
                    var window = FuncWindow.Open(maker, EFuncCacheFlag.Variable);
                    window.FromString += formStr;
                }
            }

            if (maker.Self.IsValueType)
            {
                if (maker.Self.ParamType == AbilityEditorHelper.GetTypeAllName(typeof(object)))
                {
                    EditorGUILayout.LabelField("←请选择参数类型");
                }
                else
                {
                    var type = Type.GetType(maker.Self.ParamType);

                    string label = string.IsNullOrEmpty(maker.Self.ParamName) ? "参数" : maker.Self.ParamName;

                    maker.Self.Value = (AutoValue)AbilityEditorHelper.DrawLabelByType(type, label, maker.Self.Value);
                }
            }

            EditorGUIUtility.labelWidth = old;
            EditorGUILayout.EndHorizontal();
        }
    }
}