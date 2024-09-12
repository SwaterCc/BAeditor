using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class ParameterMaker
    {
        public Parameter Self;

        public ParameterMaker Parent;

        public List<ParameterMaker> FuncParams = new();

        public Action<Parameter[]> OnSave;

        public static void Init(ParameterMaker maker, Parameter[] source)
        {
            if (source == null || source.Length == 0)
            {
                maker.FuncParams.Clear();
                maker.CreateFuncParam(FuncWindow.FlagMethodCache[EFuncCacheFlag.Variable][0].FuncName);
            }
            else
            {
                int start = 0;
                maker.FuncParams.Clear();
                maker.Parse(source, ref start);
            }
        }

        public override string ToString()
        {
            if (Self.IsValueType)
            {
                var valueStrs = Self.Value.ToString().Split(".");
                var valueStr = valueStrs[^1];
                return $"参数:{Self.ParamName}:{valueStr}";
            }

            if (Self.IsFunc)
            {
                string func = $"函数:{Self.FuncName}(";
                for (var index = 0; index < FuncParams.Count; index++)
                {
                    var parameter = FuncParams[index];
                    if (index != FuncParams.Count - 1)
                        func = func + parameter + ",";
                    else
                        func += parameter;
                }

                func += ")";
                return func;
            }

            return "????";
        }

        public void CreateFuncParam(string funcName)
        {
            var paramName =Self.ParamName;
            Self = new Parameter();
            Self.IsFunc = true;
            Self.FuncName = funcName;
            Self.ParamName = paramName;
            if (!FuncWindow.MethodCache.TryGetValue(Self.FuncName, out var methodInfo))
            {
                Debug.LogError($"{Self.FuncName} 该函数未定义！");
                return;
            }

            Self.ParamCount = methodInfo.GetParameters().Length;
            FuncParams.Clear();
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                var paramNode = new ParameterMaker();
                var fullName = AbilityEditorHelper.GetTypeAllName(parameterInfo.ParameterType);
                paramNode.CreateValueParam(fullName, parameterInfo.Name);
                FuncParams.Add(paramNode);
                paramNode.Parent = this;
            }
        }

        public void CreateValueParam(string paramValueTypeStr, string paramName = "")
        {
            Self = new Parameter();
            Self.ParamType = paramValueTypeStr;
            Self.IsValueType = true;
            Self.ParamName = paramName;
            var valueType = Type.GetType(paramValueTypeStr);
            if (valueType == null)
            {
                Debug.LogError("获取type失败");
                return;
            }

            Self.Value = (AutoValue)valueType.InstantiateDefault(true);
        }

        public void Parse(Parameter[] source, ref int start)
        {
            if (source == null || source.Length == 0)
            {
                Debug.LogError("源数据是空的");
                return;
            }

            Self = source[start];

            if (Self.IsFunc)
            {
                for (int i = 0; i < Self.ParamCount; ++i)
                {
                    var paramNode = new ParameterMaker();
                    ++start;
                    paramNode.Parse(source, ref start);
                    FuncParams.Add(paramNode);
                    paramNode.Parent = this;
                }
            }
        }

        public void ChangeToValueType(object valueTypeStr)
        {
            ChangeToValueType((string)valueTypeStr);
        }

        public void ChangeToValueType(string valueTypeStr)
        {
            if (Self.IsValueType && valueTypeStr == Self.ParamType) return;
            CreateValueParam(valueTypeStr, Self.ParamName);
            FuncParams.Clear();
        }

        public void ChangeToFunc(string funcName)
        {
            if (Self.IsFunc && Self.FuncName != funcName) return;
            CreateFuncParam(funcName);
        }

        public void ChangeToDefaultFunc(EFuncCacheFlag flag)
        {
            if (FuncWindow.FlagMethodCache[flag].Count > 0)
            {
                var info = FuncWindow.FlagMethodCache[flag][0];
                if (Self.IsFunc && Self.FuncName != info.FuncName) return;
                CreateFuncParam(info.FuncName);
            }
            else
            {
                Debug.LogError("该类型函数没有反射");
            }
        }

        public Parameter[] ToArray()
        {
            var list = new List<Parameter>();

            list.Add(Self);
            foreach (var parameterNode in FuncParams)
            {
                list.AddRange(parameterNode.ToArray());
            }

            return list.ToArray();
        }

        public void Save()
        {
            OnSave?.Invoke(ToArray());
        }
    }

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