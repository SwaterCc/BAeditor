using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class ParameterField
    {
        private readonly GenericMenu _menu;
        private AParams _params;
        private string _paramName;
        private Type _parseType;
        private Type _originType;

        public ParameterField(AParams aParams, string paramName, Type type)
        {
            _menu = new GenericMenu();
            _params = aParams;
            _paramName = paramName;
            _originType = _parseType = type;
        }

        private void drawTypeParse()
        {
            if (GUILayout.Button("▼", GUILayout.Width(22))) { }
        }

        private void drawParamSwitch()
        {
            if (GUILayout.Button("▼", GUILayout.Width(22)))
            {
                _menu.AddItem(new GUIContent("调用函数"), false,
                              () => { _params.paramType = EParamType.Function; });
                _menu.AddItem(new GUIContent("直接输入"), false,
                              () => { _params.paramType = EParamType.Simple; });
                _menu.AddItem(new GUIContent("自定义变量"), false,
                              () => { _params.paramType = EParamType.Variable; });
                _menu.AddItem(new GUIContent("属性"), false,
                              () => { _params.paramType = EParamType.Attr; });
                _menu.ShowAsContext();
            }
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(_paramName), GUILayout.Width(100));

            //绘制类型转换按钮
            drawTypeParse();

            //绘制参数来源切换按钮
            drawParamSwitch();

            switch (_params.paramType)
            {
                case EParamType.Simple:
                    baseDraw();
                    break;
                case EParamType.Function:
                    functionDraw();
                    break;
                case EParamType.Variable:
                    variableDraw();
                    break;
                case EParamType.Attr:
                    attrDraw();
                    break;
            }

            EditorGUIUtility.labelWidth = old;
            EditorGUILayout.EndHorizontal();
        }


        private void functionDraw()
        {
            string text = "";
            if (string.IsNullOrEmpty(_params.funcName))
            {
                text = "未选择函数";
            }
            else
            {
                text = "调用函数" + _params.funcName;
            }

            var rect = GUILayoutUtility.GetLastRect();
            if (SirenixEditorGUI.Button(text, ButtonSizes.Medium))
            {
                var funcWindow = FuncWindow.Open(_params, _parseType.GetParameterValueType(),
                                                 (parameter) => _params.CopyTo(parameter));
                funcWindow.position = new Rect(rect.position, new Vector2(680, 500));
            }
        }

        private void baseDraw()
        {
            try
            {
                //变量名加按钮
                switch (_parseType.GetParameterValueType())
                {
                    case EParamValueType.Int:
                        _params.Value ??= new RefInt();
                        ((RefInt)_params.Value).Value = SirenixEditorFields.IntField((RefInt)_params.Value);
                        break;
                    case EParamValueType.Float:
                        _params.Value ??= new RefFloat();
                        ((RefFloat)_params.Value).Value =
                            SirenixEditorFields.FloatField((RefFloat)_params.Value);
                        break;
                    case EParamValueType.Bool:
                        _params.Value ??= new RefBool();
                        string select = ((RefBool)_params.Value).ToString();
                        select =
                            SirenixEditorFields.Dropdown(new GUIContent(""), select, new[] { "true", "false" });
                        ((RefBool)_params.Value).Value = bool.Parse(select);
                        break;
                    case EParamValueType.Vector3:
                        _params.Value ??= new RefVector3();
                        ((RefVector3)_params.Value).FromVector3(
                            SirenixEditorFields.Vector3Field((RefVector3)_params.Value));
                        break;
                    case EParamValueType.Object:
                        if (_params.Value == null)
                        {
                            EditorGUILayout.LabelField("←----请选择类型！");
                        }

                        break;
                    default:
                        EditorGUILayout.LabelField($"还未实现{_parseType}");
                        break;
                }
            }
            catch (InvalidCastException e)
            {
                Debug.LogError(e);
                _params.Value = null;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private void attrDraw()
        {
            if (SirenixEditorGUI.Button("使用属性" + _params.attrType, ButtonSizes.Medium)) { }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("使用变量" + _params.variableName, ButtonSizes.Medium)) { }
        }

        /*private void drawDropDown()
        {
            SirenixEditorGUI.BeginVerticalList();

            // 绘制搜索栏
            SirenixEditorGUI.BeginListItem();
            _searchString = EditorGUILayout.TextField("搜索:", _searchString);
            SirenixEditorGUI.EndListItem();
            // 创建一个滚动视图以显示下拉列表项
            _dropDownPos = EditorGUILayout.BeginScrollView(_dropDownPos, GUILayout.Height(150));

            // 过滤列表项并显示
            foreach (var item in _dropDownList.Where(i => i.ToLower().Contains(_searchString.ToLower())))
            {
                if (SirenixEditorGUI.Button(item, ButtonSizes.Medium))
                {
                    if (_aParameter.paramType == EParamType.Attr)
                    {
                        _aParameter.attrType = Enum.Parse<EAttrType>(item);
                    }

                    if (_aParameter.paramType == EParamType.Variable)
                    {
                        _aParameter.variableName = item;
                    }

                    _showDropDown = false; // 选择后关闭下拉框
                }
            }

            EditorGUILayout.EndScrollView();
            SirenixEditorGUI.EndVerticalList();
        }*/

        /*private void handleClickOutside()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && _showDropDown)
            {
                if (!_dropDownRect.Contains(currentEvent.mousePosition))
                {
                    _showDropDown = false;
                }
            }
        }*/
    }
}