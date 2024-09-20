using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private Type _originType;

        public ParameterField(Parameter parameter, string paramName, Type type)
        {
            _menu = new GenericMenu();
            _parameter = parameter;
            _paramName = paramName;
            _type = type;
            _originType = type;
            _dropDownPos = Vector2.zero;
            _dropDownList = new List<string>();
            _searchString = "";
            
            if (_originType == typeof(object) && _parameter.Value != null) {
	            _type = _parameter.Value.GetType();
            }
        }

        private void showMenu()
        {
            _menu.AddItem(new GUIContent("调用函数"), false, () => { _parameter.ParameterType = EParameterType.Function; });

            _menu.AddItem(new GUIContent("直接输入"), false, () => { _parameter.ParameterType = EParameterType.Simple; });

            _menu.AddItem(new GUIContent("自定义变量"), false,
                () => { _parameter.ParameterType = EParameterType.Variable; });

            _menu.AddItem(new GUIContent("属性"), false, () => { _parameter.ParameterType = EParameterType.Attr; });

            if (_originType == typeof(object)) {
	            _menu.AddItem(new GUIContent("Object/int"), false, () => {
		            _type = typeof(int);
		            _parameter.Value = new int();
	            });
	            _menu.AddItem(new GUIContent("Object/float"), false, () => {
		            _type = typeof(float);
		            _parameter.Value = new float();
	            });
	            _menu.AddItem(new GUIContent("Object/bool"), false, () => {
		            _type = typeof(bool);
		            _parameter.Value = new bool();
	            });
	            _menu.AddItem(new GUIContent("Object/string"), false, () => {
		            _type = typeof(string);
		            _parameter.Value = "";
	            });
	            _menu.AddItem(new GUIContent("Object/重置"), false, () => {
		            _type = typeof(object);
		            _parameter.Value = null;
	            });
            }

            _menu.ShowAsContext();
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(_paramName), GUILayout.Width(100));

            if (GUILayout.Button("▼", GUILayout.Width(22)))
            {
                showMenu();
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
                var funcWindow = FuncWindow.Open(_parameter, _type.GetParameterValueType(),
                    (parameter) => _parameter.CopyTo(parameter));
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
                case EParameterValueType.Object:
	                if (_parameter.Value == null) {
		                EditorGUILayout.LabelField("←----请选择类型！");
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
                foreach (var attrName in Enum.GetNames(typeof(ELogicAttr)))
                {
                    var type = Enum.Parse<ELogicAttr>(attrName).GetValueType();
                    if (_type == type)
                    {
                        _dropDownList.Add(attrName);
                    }
                }

                _showDropDown = true;
            }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("使用变量" + _parameter.VairableName, ButtonSizes.Medium))
            {
                _dropDownList.Clear();
                //获取变量列表
                _dropDownList = AbilityViewDrawer.VarCollector.GetVariables(_type);
                if (AbilityViewDrawer.BeforeClick != null)
                {
                    EventNodeData eventNode = null;
                    int parentId = AbilityViewDrawer.BeforeClick.ParentId;
                    while (parentId > 0)
                    {
                        var parentNode = AbilityViewDrawer.AbilityData.NodeDict[parentId];
                        parentId = parentNode.ParentId;
                        if (parentNode.NodeType != EAbilityNodeType.EEvent) continue;
                        eventNode = (EventNodeData)parentNode;
                        break;
                    }

                    if (eventNode != null)
                    {
                        if (eventNode.IsEvent)
                        {
                            if (AbilityFunctionHelper.EventCheckerDict.TryGetValue(eventNode.EventType,
                                    out var editorInfo))
                            {
                                foreach (var fieldInfo in editorInfo.EventInfoType.GetFields(BindingFlags.Public |
                                             BindingFlags.Instance))
                                {
                                    if (fieldInfo.FieldType == _type)
                                        _dropDownList.Add("EventInfo:" + fieldInfo.Name);
                                }
                            }
                        }
                        else
                        {
	                        _dropDownList.Add("Msg:P1");
	                        _dropDownList.Add("Msg:P2");
	                        _dropDownList.Add("Msg:P3");
	                        _dropDownList.Add("Msg:P4");
	                        _dropDownList.Add("Msg:P5");
                        }
                    }
                }

                _showDropDown = true;
            }
        }

        private void processEventNodeChildren() { }

        private void drawDropDown()
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
                    if (_parameter.ParameterType == EParameterType.Attr)
                    {
                        _parameter.AttrType = Enum.Parse<ELogicAttr>(item);
                    }

                    if (_parameter.ParameterType == EParameterType.Variable)
                    {
                        _parameter.VairableName = item;
                    }

                    _showDropDown = false; // 选择后关闭下拉框
                }
            }

            EditorGUILayout.EndScrollView();
            SirenixEditorGUI.EndVerticalList();
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