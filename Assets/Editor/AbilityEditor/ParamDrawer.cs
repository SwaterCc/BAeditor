using System;
using System.Collections.Generic;
using System.Reflection;
using Battle;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class ParameterNode
    {
        public Parameter Self;

        public ParameterNode Parent;

        public int InFuncIdx = -1;

        public List<ParameterNode> FuncParameter = new();

        private MethodInfo _method;

        public void Create(Parameter parameter)
        {
            Self = parameter;
            if (!parameter.IsFunc) return;
            if (_method == null)
            {
                _method = FuncWindow.MethodCache[Self.FuncName][0];
            }

            FuncParameter.Clear();
            foreach (var parameterInfo in _method.GetParameters())
            {
                var paramNode = new ParameterNode();
                paramNode.InFuncIdx = FuncParameter.Count;
                paramNode.Create(new Parameter() { ParamType = parameterInfo.ParameterType.ToString() });
                FuncParameter.Add(paramNode);
                paramNode.Parent = this;
            }
        }

        public void Parse(Parameter[] source, int idx = 0)
        {
            if (source == null || source.Length == 0)
            {
                Self = new Parameter()
                {
                    IsValueType = true,
                };
            }
            else
            {
                Self = source[idx];
            }
            
           
            if (Self.IsFunc)
            {
                _method = FuncWindow.MethodCache[Self.FuncName][0];

                foreach (var _ in _method.GetParameters())
                {
                    ++idx;
                    var paramNode = new ParameterNode();
                    paramNode.InFuncIdx = FuncParameter.Count;
                    paramNode.Parse(source, ++idx);
                    FuncParameter.Add(paramNode);
                    paramNode.Parent = this;
                }
            }
        }

        public void ChangeToValueType()
        {
            if (Self.IsValueType) return;
            Self.IsFunc = false;
            Self.FuncName = "";
            Self.IsValueType = true;
            FuncParameter.Clear();
        }

        public void ChangeToFunc(string funcName, Parameter[] parameters)
        {
            if (Self.IsFunc) return;
            Self.IsValueType = false;
            Self.FuncName = funcName;
            Self.IsFunc = true;
            foreach (var parameter in parameters)
            {
                var paramNode = new ParameterNode();
                paramNode.InFuncIdx = FuncParameter.Count;
                paramNode.Create(parameter);
                FuncParameter.Add(paramNode);
                paramNode.Parent = this;
            }
        }

        public Parameter[] ToArray()
        {
            var list = new List<Parameter>();

            list.Add(Self);
            foreach (var parameterNode in FuncParameter)
            {
                list.AddRange(parameterNode.ToArray());
            }

            return list.ToArray();
        }
    }

    public static class ParameterNodeDraw
    {
        public static void Draw(this ParameterNode node)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("▼", GUILayout.Width(8)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("调用函数"), !node.Self.IsFunc, FuncWindow.Open, node);
                menu.AddItem(new GUIContent("使用基础类型"), !node.Self.IsValueType, node.ChangeToValueType);
                menu.ShowAsContext();
            }

            if (node.Self.IsFunc)
            {
                if (node.Parent != null)
                    EditorGUILayout.LabelField($"参数{node.InFuncIdx}");
                if (GUILayout.Button(node.Self.FuncName))
                {
                    //打开函数界面
                    FuncWindow.Open(node);
                }
            }

            if (node.Self.IsValueType)
            {
                var type = Type.GetType(node.Self.ParamType);
                node.Self.Value = AbilityEditorHelper.DrawLabelByType(type, $"参数{node.InFuncIdx}", node.Self.Value);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}