using System;
using System.Collections.Generic;
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
        /// AParam类型的参数绘制,如果传入的类型是object则说明支持全类型
        /// </summary>
        /// <param name="treeItem">数据所属的节点</param>
        /// <param name="aParams">aParams对象，修改会直接应用到这里</param>
        /// <param name="label">aParams的label</param>
        /// <param name="type">aParams最终转化的参数类型</param>
        public AParamsField(ATreeItem treeItem, AParams aParams, string label, Type type)
        {
            _treeItem = treeItem;
            _params = aParams;
            _label = label;

            _originType = ARef.ParseValueTypeToARefType(type);

            _castType = string.IsNullOrEmpty(_params.paramCastType) ? _originType : _params.GetParamType();

            _paramTypeMenu = new GenericMenu();
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

        private void drawParamCast()
        {
            var castMenu = new GenericMenu();

            string curTypeName = AParamsFieldSetting.GetTypeName(_castType);

            if (GUILayout.Button(curTypeName, GUILayout.Width(80)))
            {
                if (AParamsFieldSetting.AllowCastConfig.TryGetValue(_castType, out var castList))
                {
                    foreach (var type in castList)
                    {
                        string castTypeName = AParamsFieldSetting.GetTypeName(type);
                        castMenu.AddMenuItem(curTypeName + "→" + castTypeName, changeValueType, type);
                    }
                }

                if (_castType != _originType)
                {
                    castMenu.AddMenuItem("重 置", (data) =>
                                         {
                                             _params.Value = null;
                                             changeValueType(data);
                                         },
                                         _originType);
                }

                castMenu.ShowAsContext();
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
                                           _params.Value = AParamsFieldSetting.GetDefaultValue(_castType);
                                       });
                _paramTypeMenu.AddItem(new GUIContent("调用函数"), false,
                                       () =>
                                       {
                                           _params.paramType = EParamType.Function;
                                           _params.Value = null;
                                       });
                _paramTypeMenu.AddItem(new GUIContent("黑板变量"), false,
                                       () =>
                                       {
                                           _params.paramType = EParamType.Variable;
                                           _params.Value = null;
                                       });
                _paramTypeMenu.AddItem(new GUIContent("属性"), false,
                                       () =>
                                       {
                                           _params.paramType = EParamType.Attr;
                                           _params.Value = null;
                                       });
                //来自运行时
                _paramTypeMenu.ShowAsContext();
            }
        }

        private void changeValueType(object castType)
        {
            CastValueType((Type)castType);
        }

        /// <summary>
        /// 改变Field的值类型
        /// </summary>
        /// <param name="castType"></param>
        public void CastValueType(Type castType)
        {
            if (castType == null)
                return;
            if (_castType == castType)
                return;
            _castType = castType;
            _params.paramCastType = _castType.AssemblyQualifiedName;
        }


        private void baseDraw()
        {
            _params.Value ??= AParamsFieldSetting.GetDefaultValue(_castType);
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
                    if (AParamsFieldSetting.SerializeWindow.TryGetValue(_castType, out var window))
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