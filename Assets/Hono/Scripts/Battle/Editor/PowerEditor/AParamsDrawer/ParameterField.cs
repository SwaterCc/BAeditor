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
        private AParams _aParameter;
        private string _searchString;
        private Vector2 _dropDownPos;
        private bool _showDropDown;
        private Rect _dropDownRect;
        private string _paramName;
        private Type _type;
        private List<string> _dropDownList;
        private Type _originType;

        public ParameterField(AParams aParameter, string paramName, Type type)
        {
            _menu = new GenericMenu();
            _aParameter = aParameter;
            _paramName = paramName;
            _type = type;
            _originType = type;
            _dropDownPos = Vector2.zero;
            _dropDownList = new List<string>();
            _searchString = "";
            
            if (_originType == typeof(object) && _aParameter.Value != null) {
	            _type = _aParameter.Value.GetType();
            }
        }

        private void showMenu()
        {
            _menu.AddItem(new GUIContent("调用函数"), false, () => { _aParameter.paramType = EParamType.Function; });

            _menu.AddItem(new GUIContent("直接输入"), false, () => { _aParameter.paramType = EParamType.Simple; });

            _menu.AddItem(new GUIContent("自定义变量"), false,
                () => { _aParameter.paramType = EParamType.Variable; });

            _menu.AddItem(new GUIContent("属性"), false, () => { _aParameter.paramType = EParamType.Attr; });

            if (_originType == typeof(object)) {
	            _menu.AddItem(new GUIContent("Object/int"), false, () => {
		            _type = typeof(RefInt);
		            _aParameter.Value = new RefInt();
	            });
	            _menu.AddItem(new GUIContent("Object/float"), false, () => {
		            _type = typeof(RefFloat);
		            _aParameter.Value = new RefFloat();
	            });
	            _menu.AddItem(new GUIContent("Object/bool"), false, () => {
		            _type = typeof(bool);
		            _aParameter.Value = new RefBool();
	            });
	            _menu.AddItem(new GUIContent("Object/重置"), false, () => {
		            _type = typeof(object);
		            _aParameter.Value = null;
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
            
            switch (_aParameter.paramType)
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
            if (string.IsNullOrEmpty(_aParameter.funcName))
            {
                text = "未选择函数";
            }
            else
            {
                text = "调用函数" + _aParameter.funcName;
            }

            if (SirenixEditorGUI.Button(text, ButtonSizes.Medium))
            {
                _dropDownRect = GUIHelper.GetCurrentLayoutRect();
                var funcWindow = FuncWindow.Open(_aParameter, _type.GetParameterValueType(),
                    (parameter) => _aParameter.CopyTo(parameter));
                funcWindow.position = new Rect(_dropDownRect.position, new Vector2(680, 500));
            }
        }

        private void baseDraw()
        {
	        try {
		          //变量名加按钮
            switch (_type.GetParameterValueType())
            {
                case EParamValueType.Int:
                case EParamValueType.Enum:
                    _aParameter.Value ??= new RefInt();
                    ((RefInt)_aParameter.Value).Value = SirenixEditorFields.IntField((RefInt)_aParameter.Value);
                    break;
                case EParamValueType.Float:
                    _aParameter.Value ??= new RefFloat();
                    ((RefFloat)_aParameter.Value).Value = SirenixEditorFields.FloatField((RefFloat)_aParameter.Value);
                    break;
                case EParamValueType.Bool:
                    _aParameter.Value ??= new RefBool();
                    string select = ((RefBool)_aParameter.Value).ToString();
                    select =
                        SirenixEditorFields.Dropdown(new GUIContent(""), select, new[] { "true", "false" });
                    ((RefBool)_aParameter.Value).Value = bool.Parse(select);
                    break;
                case EParamValueType.Vector3:
	                _aParameter.Value ??= new RefVector3();
	                ((RefVector3)_aParameter.Value).FromVector3(SirenixEditorFields.Vector3Field((RefVector3)_aParameter.Value));
	                break;
                case EParamValueType.Object:
	                if (_aParameter.Value == null) {
		                EditorGUILayout.LabelField("←----请选择类型！");
	                }
	                break;
                default:
                    EditorGUILayout.LabelField($"还未实现{_type}");
                    break;
            }
	        }
	        catch (InvalidCastException e) {
		        Debug.LogError(e);
		        _aParameter.Value = null;
	        }
	        catch (Exception e) {
		        Debug.LogError(e);
		        throw;
	        }
        }

        private void attrDraw()
        {
            if (SirenixEditorGUI.Button("使用属性" + _aParameter.attrType, ButtonSizes.Medium))
            {
                _dropDownRect = GUILayoutUtility.GetLastRect();
                _dropDownList.Clear();
                //获取属性列表
                foreach (var attrName in Enum.GetNames(typeof(EAttrType)))
                {
                    if (_type == typeof(int) ||_type == typeof(float))
                    {
                        _dropDownList.Add(attrName);
                    }
                }

                _showDropDown = true;
            }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("使用变量" + _aParameter.variableName, ButtonSizes.Medium))
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