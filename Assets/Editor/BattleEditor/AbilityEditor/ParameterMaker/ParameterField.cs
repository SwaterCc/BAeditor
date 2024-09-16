using System;
using System.Collections.Generic;
using System.Linq;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.RefValue;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public enum EParameterValueType
    {
        Any,
        Int,
        Float,
        Bool,
        String,
        IntList,
        FloatList,
        Custom,
    }


    public class ParameterFieldSetting
    {
        public ParameterField.AllowShowType DefaultShowType;
        public int AllowShowType;
        public EParameterValueType ValueType;
        public string ParamName;
    }


    public class ParameterField
    {
        [Flags]
        public enum AllowShowType
        {
            BaseValue = 0,
            Function = 1 << 0,
            Variable = 1 << 1,
            Attr = 1 << 2,
            All = ~0,
        }

        private readonly ParameterFieldSetting _setting;
        private readonly GenericMenu _menu;
        private AllowShowType _currentShowType;
        private Parameter _parameter;
        private string _searchString;
        private Vector2 _dropDownPos;
        private bool _showDropDown;
        private Rect _dropDownRect;
        private List<string> _dropDownList;
        private readonly GUIStyle _searchFieldStyle;
        private readonly GUIStyle _searchFieldBackgroundStyle;

        public ParameterField(ParameterFieldSetting setting, Parameter parameterMaker)
        {
            _setting = setting;
            _menu = new GenericMenu();
            _currentShowType = setting.DefaultShowType;
            _parameter = new Parameter(parameterMaker);
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
            if ((_setting.AllowShowType & (int)AllowShowType.Function) > 0)
            {
                _menu.AddItem(new GUIContent("调用函数"), false, () => _currentShowType = AllowShowType.Function);
            }

            if ((_setting.AllowShowType & (int)AllowShowType.BaseValue) > 0)
            {
                _menu.AddItem(new GUIContent("使用基础类型"), false, () => _currentShowType = AllowShowType.BaseValue);
            }

            if ((_setting.AllowShowType & (int)AllowShowType.Variable) > 0)
            {
                _menu.AddItem(new GUIContent("自定义变量"), false, () => _currentShowType = AllowShowType.Variable);
            }

            if ((_setting.AllowShowType & (int)AllowShowType.Attr) > 0)
            {
                _menu.AddItem(new GUIContent("属性"), false, () => _currentShowType = AllowShowType.Attr);
            }

            _menu.ShowAsContext();
        }


        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;
            if (GUILayout.Button("▼", GUILayout.Width(22)))
            {
                showMenu();
            }

            EditorGUILayout.LabelField(new GUIContent(_setting.ParamName), GUILayout.Width(60));

            switch (_currentShowType)
            {
                case AllowShowType.BaseValue:
                    baseDraw();
                    break;
                case AllowShowType.Function:
                    functionDraw();
                    break;
                case AllowShowType.Variable:
                    variableDraw();
                    break;
                case AllowShowType.Attr:
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
            if (SirenixEditorGUI.Button("调用函数" + _parameter.FuncName, ButtonSizes.Medium))
            {
                //打开函数窗口
            }
        }

        private void baseDraw()
        {
            //变量名加按钮
            switch (_setting.ValueType)
            {
                case EParameterValueType.Int:
                    _parameter.Value = SirenixEditorFields.IntField((RefInt)_parameter.Value);
                    break;
                case EParameterValueType.Float:
                    _parameter.Value = SirenixEditorFields.FloatField((RefFloat)_parameter.Value);
                    break;
                case EParameterValueType.Bool:
                    string select = ((RefBool)_parameter.Value).ToString();
                    _parameter.Value =
                        SirenixEditorFields.Dropdown(new GUIContent(""), select, new[] { "true", "false" });
                    _parameter.Value = bool.Parse(select);
                    break;
                case EParameterValueType.String:
                    _parameter.Value = SirenixEditorFields.TextField((string)_parameter.Value);
                    break;
                case EParameterValueType.Custom:
                    var type = _parameter.Value.GetType();
                    if (SirenixEditorGUI.Button(type.Name, ButtonSizes.Medium))
                    {
                        //打开序列化窗口
                    }

                    break;
                default:
                    EditorGUILayout.LabelField($"还未实现{_setting.ValueType}");
                    break;
            }
        }

        private void attrDraw()
        {
            if (SirenixEditorGUI.Button("使用属性" + _parameter.AttrType, ButtonSizes.Medium))
            {
                _dropDownList.Clear();
                //获取属性列表
            }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("使用变量" + _parameter.AttrType, ButtonSizes.Medium))
            {
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