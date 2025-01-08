using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AParamsField
    {
        /// <summary>
        /// 所属树节点
        /// </summary>
        private readonly ATreeItem _treeItem;
        /// <summary>
        /// 参数队列
        /// </summary>
        private readonly AParams _params;
        /// <summary>
        /// label
        /// </summary>
        private readonly string _label;
        /// <summary>
        /// 转换后的类型
        /// </summary>
        private Type _castType;
        /// <summary>
        /// 初始类型
        /// </summary>
        private Type _originType;
        /// <summary>
        /// 参数来源菜单
        /// </summary>
        private readonly GenericMenu _paramTypeMenu;

        /// <summary>
        /// AParam类型的参数绘制
        /// </summary>
        /// <param name="treeItem">数据所属的节点</param>
        /// <param name="aParams">aParams对象，修改会直接应用到这里</param>
        /// <param name="label">aParams的label</param>
        /// <param name="type">aParams最终转化的参数类型</param>
        /// <exception cref="Exception"></exception>
        public AParamsField(ATreeItem treeItem, AParams aParams, string label, Type type)
        {
            _treeItem = treeItem;
            _params = aParams;
            _label = label;

            _originType = _castType = ARef.ParseValueTypeToARefType(type);
            if (_params.paramType == EParamType.Simple)
            {
                _params.Value ??= AParamsFieldConfig.GetDefaultValue(_castType);
            }
            _paramTypeMenu = new GenericMenu();
        }

        private void drawParamCast()
        {
            string getCastLabel(string cast)
            {
                return _castType.Name + "→" + cast;
            }

            var castMenu = new GenericMenu();

            var curType = _castType.ToString().Split(".")[^1];

            if (AParamsFieldConfig.AllowCastConfig.TryGetValue(_castType, out var castList))
            {
                if (GUILayout.Button(curType, GUILayout.Width(80)))
                {
                    foreach (var type in castList)
                    {
                        castMenu.AddMenuItem(getCastLabel(type.Name), setCastType, type);
                    }

                    castMenu.ShowAsContext();
                }
            }

            void setCastType(object castType)
            {
                _castType = (Type)castType;
            }
        }

        private void drawParamTypeSwitch()
        {
            if (GUILayout.Button("▼", GUILayout.Width(22)))
            {
                _paramTypeMenu.AddItem(new GUIContent("直接输入"), false,
                                       () =>
                                       {
                                           _params.paramType = EParamType.Simple;
                                           _params.Value = AParamsFieldConfig.GetDefaultValue(_castType);
                                       });
                _paramTypeMenu.AddItem(new GUIContent("调用函数"), false,
                                       () => { _params.paramType = EParamType.Function; });
                _paramTypeMenu.AddItem(new GUIContent("黑板变量"), false,
                                       () => { _params.paramType = EParamType.Variable; });
                _paramTypeMenu.AddItem(new GUIContent("属性"), false,
                                       () => { _params.paramType = EParamType.Attr; });
                //来自运行时
                _paramTypeMenu.ShowAsContext();
            }
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(_label, _label), GUILayout.Width(100));

            //绘制类型转换按钮
            drawParamCast();

            //绘制参数来源切换按钮
            drawParamTypeSwitch();

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

        private void baseDraw()
        {
            if (_castType.BaseType == typeof(ARef))
            {
                //继承自ARef，自行补充
                switch (_params.Value)
                {
                    case RefInt value:
                        value.Value = SirenixEditorFields.IntField(value);
                        break;
                    case RefFloat value:
                        value.Value = SirenixEditorFields.FloatField(value);
                        break;
                    case RefBoolean value:
                        value.Value = PowerEditorUIHelper.BoolDropField("", value);
                        break;
                    case RefVector3 value:
                        value.FromVector3(SirenixEditorFields.Vector3Field(value));
                        break;
                }
            }
            else if (_castType == typeof(string))
            {
                _params.Value = SirenixEditorFields.TextField((string)_params.Value);
            }
            else if (_castType.IsEnum)
            {
                _params.Value = SirenixEditorFields.EnumDropdown((Enum)_params.Value);
            }
            else if (_castType.IsSerializable && _castType != typeof(object))
            {
                if ((SirenixEditorGUI.Button("编辑 " + _castType.Name, ButtonSizes.Medium)))
                {
                    //可序列化类型
                    if (AParamsFieldConfig.SerializeWindow.TryGetValue(_castType, out var window))
                    {
                        window.Show();
                    }
                    else
                    {
                        SerializableOdinWindow.Open(_params.Value);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("该类型不支持直接输入");
            }
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
                FuncWindow.Open(_treeItem, _params, new List<Type>() { ARef.ParseARefTypeToValueType(_castType) });
            }
        }

        private void attrDraw()
        {
            if (SirenixEditorGUI.Button("使用属性 : " + _params.attrType, ButtonSizes.Medium))
            {
                var dropdown = new AttrDropdown(_params);
                dropdown.Show(GUILayoutUtility.GetRect(300, 300, 100, 400));
            }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("黑板变量 : " + _params.variableName, ButtonSizes.Medium))
            {
                var dropdown =
                    new VariableDropView(_treeItem, onVariableSelect, ARef.ParseARefTypeToValueType(_castType));
                dropdown.Show(GUILayoutUtility.GetRect(300, 300, 100, 400));
            }

            void onVariableSelect(string key, Type type)
            {
                _params.variableName = key;
            }
        }
    }
}