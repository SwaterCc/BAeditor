using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using SearchField = UnityEditor.IMGUI.Controls.SearchField;

namespace Editor.AbilityEditor
{
    public partial class FuncWindow : EditorWindow
    {
        public static void Open(ATreeItem treeItem,
            AParams aParameter,
            List<Type> filters = null,
            bool reelection = false)
        {
            var window = CreateInstance<FuncWindow>();
            window.Init(treeItem, aParameter, filters, reelection);
            window.Show();
        }

        private ATreeItem _treeItem;
        private AParams _function;
        private FunctionView _funcListView;
        private List<AParamsField> _parameterFields;
        private SearchField _searchField;
        private string _curTab;
        private AParamFiledFilter _filters;

        private float _windowWidth;
        private float _windowHeight;

        private void Init(ATreeItem treeItem, AParams aParameter, List<Type> filters = null, bool reelection = false)
        {
            _treeItem = treeItem;
            _function = aParameter;
            _searchField = new SearchField();
            _parameterFields = new List<AParamsField>();
            _curTab = "All";
            _filters = new AParamFiledFilter();

            if (filters != null)
            {
                _filters.FilterItems.AddRange(filters);
            }

            _filters.Reelection = reelection;
            _funcListView = new FunctionView(this, _curTab);
            ChangeSelectFunction(aParameter.funcName);
        }

        private void ChangeSelectFunction(string funcName)
        {
            if (string.IsNullOrEmpty(funcName))
                return;

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

                var field = new AParamsField(_treeItem, funcParam, paramInfo.ParamName, paramInfo.ParamType);

                _function.funcParams.Add(funcParam);
                _parameterFields.Add(field);
            }
        }

        protected void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            _funcListView.searchString = _searchField.OnGUI(_funcListView.searchString);

            SirenixEditorGUI.BeginHorizontalToolbar();
            foreach (var tab in AbilityFuncInfoCache.FuncGroupDict.Keys)
            {
                if (SirenixEditorGUI.ToolbarTab(_curTab == tab, tab))
                {
                    _curTab = tab;
                    _funcListView.ChangeFunctionGroup(_curTab);
                }
            }

            SirenixEditorGUI.EndHorizontalToolbar();

            //函数列表界面
            var rect = GUILayoutUtility.GetRect(300, 500, 60, 200);
            _funcListView.OnGUI(rect);

            GUILayout.Space(30);
            //配置界面
            SirenixEditorGUI.BeginBox();
            if (string.IsNullOrEmpty(_function.funcName))
            {
                EditorGUILayout.LabelField("未选择函数！");
            }
            else
            {
                var funcInfo = AbilityFuncInfoCache.GetFuncInfo(_function.funcName);

                SirenixEditorGUI.BeginBox($"当前正在配置 {_function.funcName}", true);
                SirenixEditorGUI.BeginVerticalList();
                for (var index = 0; index < _parameterFields.Count; index++)
                {
                    SirenixEditorGUI.BeginListItem();
                    if (!string.IsNullOrEmpty(funcInfo.ParamInfos[index].ParamDesc))
                    {
                        var fontColor = GUI.contentColor;
                        GUI.contentColor = Color.yellow;
                        //绘制参数描述
                        EditorGUILayout.LabelField("参数描述：" + funcInfo.ParamInfos[index].ParamDesc,
                                                   new GUIStyle(EditorStyles.label)
                                                   {
                                                       fontStyle = FontStyle.BoldAndItalic,
                                                       richText = true,
                                                       alignment = TextAnchor.LowerLeft,
                                                   });
                        GUI.contentColor = fontColor;
                    }

                    AParamsField parameterField = _parameterFields[index];
                    parameterField.Draw();
                    SirenixEditorGUI.EndListItem();
                }

                SirenixEditorGUI.EndVerticalList();

                if (SirenixEditorGUI.Button("确认修改", ButtonSizes.Gigantic))
                {
                    Close();
                }

                SirenixEditorGUI.EndBox();
            }

            SirenixEditorGUI.EndBox();
            EditorGUILayout.EndVertical();
        }
    }
}