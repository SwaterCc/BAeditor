using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AParamsField
    {
        /// <summary>
        /// 参数队列
        /// </summary>
        protected readonly AParams Params;
        /// <summary>
        /// label
        /// </summary>
        protected readonly string Label;

        protected AParamsField(AParams aParams, string label)
        {
            Params = aParams;
            Label = label;
        }

        public abstract void Draw();
    }

    public class AParamsField<T> : AParamsField where T : class, new()
    {
        private readonly GenericMenu _paramTypeMenu;
        private readonly GenericMenu _castMenu;

        /// <summary>
        /// 转换后的类型
        /// </summary>
        private Type _castType;

        /// <summary>
        /// 初始类型
        /// </summary>
        private Type _originType;

        public AParamsField(AParams aParams, string label) : base(aParams, label)
        {
            _originType = _castType = typeof(T);

            if (!_originType.IsSerializable)
            {
                throw new Exception("类型必须为可序列化对象");
            }

            _paramTypeMenu = new GenericMenu();
            _castMenu = new GenericMenu();
        }

        private void drawParamCast()
        {
            string getCastLabel(string cast)
            {
                return _castType.Name + "→" + cast;
            }

            var curType = _castType.ToString().Split(".")[^1];

            if (AParamsFieldConfig.AllowCastConfig.TryGetValue(_castType, out var castList))
            {
                if (GUILayout.Button(curType, GUILayout.Width(80)))
                {
                    foreach (var type in castList)
                    {
                        _castMenu.AddMenuItem(getCastLabel(type.Name), setCastType, type);
                    }

                    _castMenu.ShowAsContext();
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
                                       () => { Params.paramType = EParamType.Simple; });
                _paramTypeMenu.AddItem(new GUIContent("调用函数"), false,
                                       () => { Params.paramType = EParamType.Function; });
                _paramTypeMenu.AddItem(new GUIContent("黑板变量"), false,
                                       () => { Params.paramType = EParamType.Variable; });
                _paramTypeMenu.AddItem(new GUIContent("属性"), false,
                                       () => { Params.paramType = EParamType.Attr; });
                _paramTypeMenu.ShowAsContext();
            }
        }

        public override void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;

            EditorGUILayout.LabelField(new GUIContent(Label, Label), GUILayout.Width(100));

            //绘制类型转换按钮
            drawParamCast();

            //绘制参数来源切换按钮
            drawParamTypeSwitch();

            switch (Params.paramType)
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
                if (Params.Value == null)
                {
                    Params.Value = new T();
                }

                //继承自ARef，自行补充
                switch (Params.Value)
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
            else if (_castType.IsSerializable)
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
                        SerializableOdinWindow.Open(Params.Value = new T());
                    }
                }
            }
        }

        private void functionDraw()
        {
            string text = "";
            if (string.IsNullOrEmpty(Params.funcName))
            {
                text = "未选择函数";
            }
            else
            {
                text = "调用函数" + Params.funcName;
            }

            var rect = GUILayoutUtility.GetLastRect();
            if (SirenixEditorGUI.Button(text, ButtonSizes.Medium))
            {
                FuncWindow.Open(Params, new List<Type>() { _castType });
            }
        }

        private void attrDraw()
        {
            if (SirenixEditorGUI.Button("使用属性 : " + Params.attrType, ButtonSizes.Medium))
            {
                var dropdown = new AttrDropdown(Params);
                dropdown.Show(GUILayoutUtility.GetRect(300, 300, 100, 400));
            }
        }

        private void variableDraw()
        {
            if (SirenixEditorGUI.Button("黑板变量 : " + Params.variableName, ButtonSizes.Medium)) { }
        }
    }
}