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
        public static FuncWindow Open<T>(AParams aParameter)
        {
            var window = CreateInstance<FuncWindow>();
            window.Init(aParameter);
            window.ShowModal();
            return window;
        }
        
        private AParams _function;
        private EParamValueType _valueType;
        private Action<AParams> _onSave;
        private FunctionView _funcListView;
        private List<AParamsField> _parameterFields;
        
        private float _windowWidth;
        private float _windowHeight;

        public void Init(AParams aParameter)
        {
            _function = aParameter;
            _parameterFields = new List<AParamsField>();
            _funcListView = new FunctionView(this);
        }
        
        public void OnDoubleClick(string funcName)
        {
            var funcInfo = AbilityFuncInfoCache.GetFuncInfo(funcName);

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
                // 获取泛型类的类型
                Type genericClassType = typeof(AParamsField<>);
                // 为泛型类指定具体类型参数，例如 typeof(int)
                Type constructedType = genericClassType.MakeGenericType(paramInfo.ParamType);
                object instance = Activator.CreateInstance(constructedType, funcParam, paramInfo.ParamName);
                _function.funcParams.Add(funcParam);
                _parameterFields.Add((AParamsField)instance);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            //SirenixEditorGUI.Title(FromString, "", TextAlignment.Center, true);
            SirenixEditorGUI.BeginBox();
            //函数列表界面
            var rect = GUILayoutUtility.GetRect(200, 200, 300, 500);
            _funcListView.OnGUI(rect);
           
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