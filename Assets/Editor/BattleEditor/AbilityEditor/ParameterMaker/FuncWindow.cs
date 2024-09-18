using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class FuncWindow : EditorWindow
    {
        public static FuncWindow Open(Parameter parameter, EParameterValueType valueType, Action<Parameter> onSave)
        {
            var window = CreateInstance<FuncWindow>();
            window.Init(parameter, valueType, onSave);
            window.Show();
            return window;
        }
        
        private Parameter _function;
        private EParameterValueType _valueType;
        private Action<Parameter> _onSave;
        private FunctionView _funcTree;
        private List<ParameterField> _parameterFields;

        public void Init(Parameter parameter, EParameterValueType valueType, Action<Parameter> onSave)
        {
            _function = new Parameter(parameter);
            _valueType = valueType;
            _onSave = onSave;
            _parameterFields = new List<ParameterField>();

            if (!string.IsNullOrEmpty(_function.FuncName))
            {
                var funcInfo = AbilityFunctionHelper.GetFuncInfo(_function.FuncName);
                for (var index = 0; index < _function.FuncParams.Count; index++)
                {
                    var funcParam = _function.FuncParams[index];
                    var paramInfo = funcInfo.ParamInfos[index];
                    _parameterFields.Add(new ParameterField(funcParam, paramInfo.ParamName, paramInfo.ParamType));
                }
            }
            
            _funcTree = new FunctionView(new TreeViewState(), this, _function.FuncName, AbilityFunctionHelper.GetFuncInfosByType(_valueType));
        }
        
        public void OnDoubleClick(string funcName)
        {
            var funcInfo = AbilityFunctionHelper.GetFuncInfo(funcName);

            _function.FuncName = funcName;
            _function.FuncParams ??= new List<Parameter>();
            _function.FuncParams.Clear();
            _function.ParameterType = EParameterType.Function;
            _parameterFields.Clear();
            foreach (var paramInfo in funcInfo.ParamInfos)
            {
                var funcParam = new Parameter
                {
                    ParameterType = EParameterType.Simple
                };
                _function.FuncParams.Add(funcParam);
                _parameterFields.Add(new ParameterField(funcParam, paramInfo.ParamName, paramInfo.ParamType));
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            //SirenixEditorGUI.Title(FromString, "", TextAlignment.Center, true);
            EditorGUILayout.BeginHorizontal();
            //函数列表界面
            GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(280));
            var rect = GUIHelper.GetCurrentLayoutRect();
            _funcTree.OnGUI(new Rect(rect.x, rect.y, 300, 280));
            //函数预览界面
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginVerticalList();
            
            if (AbilityFunctionHelper.TryGetFuncInfo(_funcTree.CurSelect, out var funcInfo) && funcInfo.ParamCount > 0)
            {
                foreach (var param in funcInfo.ParamInfos)
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.LabelField("参数名：" + param.ParamName);
                    EditorGUILayout.LabelField("参数类型：" + param.ParamType);
                    SirenixEditorGUI.EndListItem();
                }

                if (funcInfo.ReturnType != typeof(void))
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.LabelField("返回类型：" + funcInfo.ReturnType);
                    SirenixEditorGUI.EndListItem();
                }
                else
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.LabelField("无返回值");
                    SirenixEditorGUI.EndListItem();
                }
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30);
            //配置界面

            if (string.IsNullOrEmpty(_function.FuncName))
            {
                SirenixEditorGUI.BeginBox();
                EditorGUILayout.LabelField("未选择函数！");
                SirenixEditorGUI.EndBox();
            }
            else
            {
                SirenixEditorGUI.BeginBox($"当前函数:{_function.FuncName}", true);
                if (_parameterFields.Count == 0)
                {
                    EditorGUILayout.LabelField("无参函数");
                }
                else
                {
                    foreach (var parameterField in _parameterFields)
                    {
                        parameterField.Draw();
                    }
                }

                if (SirenixEditorGUI.Button("确认修改",ButtonSizes.Gigantic))
                {
                    _onSave.Invoke(_function);
                    Close();
                }
                SirenixEditorGUI.EndBox();
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}