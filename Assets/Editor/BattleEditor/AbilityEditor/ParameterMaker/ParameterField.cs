using System;
using System.Collections.Generic;
using System.Linq;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.RefValue;
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
        private Parameter _parameter;
        private string _searchString;
        private Vector2 _dropDownPos;
        private bool _showDropDown;
        private Rect _dropDownRect;
        private string _paramName;
        private Type _type;
        private List<string> _dropDownList;
        private readonly GUIStyle _searchFieldStyle;
        private readonly GUIStyle _searchFieldBackgroundStyle;

        public ParameterField(Parameter parameter, string paramName, Type type)
        {
            _menu = new GenericMenu();
            _parameter = parameter;
            _paramName = paramName;
            _type = type;
            _dropDownPos = Vector2.zero;
            _dropDownList = new List<string>();

            _searchFieldStyle = new GUIStyle(EditorStyles.textField)
            {
                padding = new RectOffset(20, 5, 3, 3) // 设置内边距，确保文本和图标之间有空间
            };

            _searchFieldBackgroundStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(5, 5, 5, 5), // 内边距
                margin = new RectOffset(0, 0, 0, 0) // 外边距
            };
        }
        
        private void showMenu()
        {
            _menu.AddItem(new GUIContent("调用函数"), false, () => { _parameter.ParameterType = EParameterType.Function; });

            _menu.AddItem(new GUIContent("直接输入"), false, () => { _parameter.ParameterType = EParameterType.Simple; });

            _menu.AddItem(new GUIContent("自定义变量"), false,
                () => { _parameter.ParameterType = EParameterType.Variable; });

            _menu.AddItem(new GUIContent("属性"), false, () => { _parameter.ParameterType = EParameterType.Attr; });

            _menu.ShowAsContext();
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(_paramName), GUILayout.Width(100));
            
            if (_type.GetParameterValueType() != EParameterValueType.Custom)
            {
                if (GUILayout.Button("▼", GUILayout.Width(22)))
                {
                    showMenu();
                }
            }
            else
            {
                EditorGUILayout.LabelField("",GUILayout.Width(22));
            }
            
            switch (_parameter.ParameterType)
            {
                case EParameterType.Simple:
                    baseDraw();
                    break;
                case EParameterType.Function:
                    functionDraw();
                    break;
                case EParameterType.Variable:
                    variableDraw();
                    break;
                case EParameterType.Attr:
                    attrDraw();
                    break;
            }

            if (_showDropDown)
            {
                drawDropDown();
            }

            handleClickOutside();

            EditorGUIUtility.labelWidth = old;
            EditorGUILayout.EndHorizontal();
        }


        private void functionDraw()
        {
            string text = "";
            if (string.IsNullOrEmpty(_parameter.FuncName))
            {
                text = "未选择函数";
            }
            else
            {
                text = "调用函数" + _parameter.FuncName;
            }

            if (SirenixEditorGUI.Button(text, ButtonSizes.Medium))
            {
                _dropDownRect = GUIHelper.GetCurrentLayoutRect();
                var funcWindow = FuncWindow.Open(_parameter, _type.GetParameterValueType(), (parameter) => _parameter = parameter);
                funcWindow.position = new Rect(_dropDownRect.position, new Vector2(680, 500));
            }
        }

        private void baseDraw()
        {
            //变量名加按钮
            switch (_type.GetParameterValueType())
            {
                case EParameterValueType.Int:
                    _parameter.Value ??= new int();
                    _parameter.Value = SirenixEditorFields.IntField((int)_parameter.Value);
                    break;
                case EParameterValueType.Float:
                    _parameter.Value ??= new float();
                    _parameter.Value = SirenixEditorFields.FloatField((float)_parameter.Value);
                    break;
                case EParameterValueType.Bool:
                    _parameter.Value ??= new bool();
                    string select = ((bool)_parameter.Value).ToString();
                    select =
                        SirenixEditorFields.Dropdown(new GUIContent(""), select, new[] { "true", "false" });
                    _parameter.Value = bool.Parse(select);
                    break;
                case EParameterValueType.String:
                    _parameter.Value ??= "";
                    _parameter.Value = SirenixEditorFields.TextField((string)_parameter.Value);
                    break;
                case EParameterValueType.Enum:
                    _parameter.Value ??= _type.InstantiateDefault(true);
                    _parameter.Value = SirenixEditorFields.EnumDropdown((Enum)_parameter.Value);
                    break;
                case EParameterValueType.Custom:
                    _parameter.Value ??= _type.InstantiateDefault(true);
                    if (SirenixEditorGUI.Button("编辑：" + _type.Name, ButtonSizes.Medium))
                    {
                        SerializableOdinWindow.Open(_parameter.Value, _type, (data) => _parameter.Value = data);
                    }
                    break;
                default:
                    EditorGUILayout.LabelField($"还未实现{_type}");
                    break;
            }
        }

        private void attrDraw()
        {
            if (SirenixEditorGUI.Button("使用属性" + _parameter.AttrType, ButtonSizes.Medium))
            {
                _dropDownRect = GUILayoutUtility.GetLastRect();
                _dropDownList.Clear();
                //获取属性列表
            }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("使用变量" + _parameter.AttrType, ButtonSizes.Medium))
            {
                _dropDownRect = GUILayoutUtility.GetLastRect();
                _dropDownList.Clear();
                //获取变量列表
            }
        }

        private void drawDropDown()
        {
            GUILayout.BeginArea(new Rect(_dropDownRect.x, _dropDownRect.y, 200, 200), _searchFieldBackgroundStyle);

            // 绘制搜索栏
            _searchString = EditorGUILayout.TextField("搜索:", _searchString, _searchFieldStyle, GUILayout.Height(20));

            // 创建一个滚动视图以显示下拉列表项
            _dropDownPos = EditorGUILayout.BeginScrollView(_dropDownPos, GUILayout.Height(150));

            // 过滤列表项并显示
            foreach (var item in _dropDownList.Where(i => i.ToLower().Contains(_searchString.ToLower())))
            {
                if (GUILayout.Button(item))
                {
                    Debug.Log($"Selected: {item}");
                    _showDropDown = false; // 选择后关闭下拉框
                }
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void handleClickOutside()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && _showDropDown)
            {
                if (!_dropDownRect.Contains(currentEvent.mousePosition))
                {
                    _showDropDown = false;
                }
            }
        }
    }
}