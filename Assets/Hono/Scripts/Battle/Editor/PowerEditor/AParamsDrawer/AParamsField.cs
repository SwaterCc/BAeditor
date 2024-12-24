using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AParamsField
    {
        public abstract void Draw();
    }

    public class AParamsField<T> : AParamsField where T : class, new()
    {
        private readonly GenericMenu _paramTypeMenu;
        private readonly GenericMenu _castMenu;
        private AParams _params;
        /// <summary>
        /// label
        /// </summary>
        private string _label;
        /// <summary>
        /// 转换类型
        /// </summary>
        private Type _castType;
        /// <summary>
        /// 初始类型
        /// </summary>
        private Type _originType;

        public AParamsField(AParams aParams, string label)
        {
            _originType = _castType = typeof(T);

            if (!_originType.IsSerializable)
            {
                throw new Exception("类型必须为可序列化对象");
            }

            _params = aParams;
            _label = label;

            _paramTypeMenu = new GenericMenu();
            _castMenu = new GenericMenu();

            if (aParams.paramType == EParamType.Simple && aParams.Value == null)
            {
                aParams.Value = new T();
            }
        }

        private void drawParamCast()
        {
            string getCastLabel(string cast)
            {
                return cast + "→" + _originType.Name;
            }

            if (GUILayout.Button(nameof(_originType), GUILayout.Width(60)))
            {
                _castMenu.AddMenuItem(getCastLabel("object"), setCastType, typeof(object));
                if (AParamsFieldConfig.AllowCastConfig.TryGetValue(_originType, out var castList))
                {
                    foreach (var type in castList)
                    {
                        _castMenu.AddMenuItem(getCastLabel(type.Name), setCastType, type);
                    }
                }

                _castMenu.AddMenuItem("（空）",               null,        true);
                _castMenu.AddMenuItem(nameof(_originType), setCastType, _originType);
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
                _paramTypeMenu.AddItem(new GUIContent("调用函数"), false,
                                       () => { _params.paramType = EParamType.Function; });
                _paramTypeMenu.AddItem(new GUIContent("直接输入"), false,
                                       () => { _params.paramType = EParamType.Simple; });
                _paramTypeMenu.AddItem(new GUIContent("自定义变量"), false,
                                       () => { _params.paramType = EParamType.Variable; });
                _paramTypeMenu.AddItem(new GUIContent("属性"), false,
                                       () => { _params.paramType = EParamType.Attr; });
                _paramTypeMenu.ShowAsContext();
            }
        }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(_label), GUILayout.Width(100));

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
            if (_originType.BaseType == typeof(ARef))
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
            else if (_originType.IsSerializable)
            {
                //可序列化类型
                if (AParamsFieldConfig.SerializeWindow.TryGetValue(_originType, out var window))
                {
                    window.ShowModal();
                }
                else
                {
                    SerializableOdinWindow.Open(_params.Value);
                }
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
                var funcWindow = FuncWindow.Open(_params, _castType.GetParameterValueType(),
                                                 (parameter) => _params.CopyTo(parameter));
                funcWindow.position = new Rect(rect.position, new Vector2(680, 500));
            }
        }

        private void attrDraw()
        {
            var dropdown = new AttrDropdown(new AdvancedDropdownState());
            dropdown.Show(GUILayoutUtility.GetRect(300, 300, 100, 600));
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("使用变量" + _params.variableName, ButtonSizes.Medium)) { }
        }
    }
}