using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
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
        public static FuncWindow Open(AParams aParameter, EParamValueType valueType, Action<AParams> onSave)
        {
            var window = CreateInstance<FuncWindow>();
            window.Init(aParameter, valueType, onSave);
            window.Show();
            return window;
        }
        
        private AParams _function;
        private EParamValueType _valueType;
        private Action<AParams> _onSave;
        private FunctionView _funcTree;
        private List<AParamsField> _parameterFields;

        public void Init(AParams aParameter, EParamValueType valueType, Action<AParams> onSave)
        {
            _function = new AParams(aParameter);
            _valueType = valueType;
            _onSave = onSave;
            _parameterFields = new List<AParamsField>();

            if (!string.IsNullOrEmpty(_function.funcName))
            {
                var funcInfo = AbilityFunctionHelper.GetFuncInfo(_function.funcName);
                for (var index = 0; index < _function.funcParams.Count; index++)
                {
                    var funcParam = _function.funcParams[index];
                    var paramInfo = funcInfo.ParamInfos[index];
                    //反射参数创建模板
                    _parameterFields.Add(new AParamsField(funcParam, paramInfo.ParamName));
                }
            }
            
            _funcTree = new FunctionView(new TreeViewState(), this, _function.funcName, AbilityFunctionHelper.GetFuncInfosByType(_valueType));
        }
        
        public void OnDoubleClick(string funcName)
        {
            var funcInfo = AbilityFunctionHelper.GetFuncInfo(funcName);

            _function.funcName = funcName;
            _function.funcParams ??= new List<AParams>();
            _function.funcParams.Clear();
            _function.paramType = EParamType.Function;
            _parameterFields.Clear();
            foreach (var paramInfo in funcInfo.ParamInfos)
            {
                var funcParam = new AParams
                {
                    paramType = EParamType.Simple
                };
                _function.funcParams.Add(funcParam);
                _parameterFields.Add(new AParamsField(funcParam, paramInfo.ParamName, paramInfo.ParamType));
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

            if (string.IsNullOrEmpty(_function.funcName))
            {
                SirenixEditorGUI.BeginBox();
                EditorGUILayout.LabelField("未选择函数！");
                SirenixEditorGUI.EndBox();
            }
            else
            {
                SirenixEditorGUI.BeginBox($"当前函数:{_function.funcName}", true);
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