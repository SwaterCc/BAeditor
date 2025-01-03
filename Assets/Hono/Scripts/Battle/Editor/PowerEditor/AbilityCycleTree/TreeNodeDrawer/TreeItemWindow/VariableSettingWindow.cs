using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class VariableSettingWindow : ANodeSettingWindow<VariableNodeData>
    {
        private string[] _baseTypeShow = { "int", "float", "bool", "string" };
        private string[] _baseTypeValue =
            { typeof(int).ToString(), typeof(float).ToString(), typeof(bool).ToString(), typeof(string).ToString() };

        private string _beforeType;
        private Type _curSelectVarType;
        private string _modifyBtnText;
        private int _operation;

        protected override void Init()
        {
            //创建变量 基础类型 int float bool string
            //修改变量 下拉框选择已有变量  -> 行为自增，自减，加值，乘值，取反，重设
            //int float bool 可能来自属性？可能性很低，暂时不做
            //变量存储变量类型？一般不会有
            _modifyBtnText = "选择变量";
            _beforeType = TempData.valueType;
            _curSelectVarType = Type.GetType(TempData.valueType);
            _operation = (int)TempData.operationType;
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("变量配置");
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (SirenixEditorGUI.ToolbarTab(!TempData.isModify, "Create"))
            {
                TempData.isModify = false;
            }

            if (SirenixEditorGUI.ToolbarTab(TempData.isModify, "Modify"))
            {
                TempData.isModify = true;
            }

            SirenixEditorGUI.EndHorizontalToolbar();

            if (!TempData.isModify)
            {
                drawCreateView();
            }
            else
            {
                drawModifyView();
            }

            SirenixEditorGUI.EndBox();
        }

        /// <summary>
        /// 创建变量
        /// </summary>
        private void drawCreateView()
        {
            TempData.key = SirenixEditorFields.TextField("变量Name:", TempData.key);

            if (AEditorVariableBoard.HasVariable(TempData.key))
            {
                PowerEditorUIHelper.DrawColorLabel("变量名重复！！", Color.red);
                DisableCloseButton = true;
            }
            else
            {
                DisableCloseButton = false;
            }

            TempData.valueType =
                SirenixEditorFields.Dropdown("选择变量类型", TempData.valueType, _baseTypeValue, _baseTypeShow);
            var type = Type.GetType(TempData.valueType);

            if (_beforeType != TempData.valueType)
            {
                TempData.value = type == typeof(string) ? "" : Activator.CreateInstance(type).ToString();
                _beforeType = TempData.valueType;
            }

            object value;
            if (type == typeof(int))
            {
                value = SirenixEditorFields.IntField("Value: ", int.Parse(TempData.value));
            }
            else if (type == typeof(float))
            {
                value = SirenixEditorFields.FloatField("Value: ", float.Parse(TempData.value));
            }
            else if (type == typeof(bool))
            {
                value = PowerEditorUIHelper.BoolDropField("Value: ", bool.Parse(TempData.value));
            }
            else
            {
                value = SirenixEditorFields.TextField("Value: ", TempData.value);
            }

            TempData.value = value.ToString();
        }

        /// <summary>
        /// 修改变量
        /// </summary>
        private void drawModifyView()
        {
            GUILayout.Space(30);
            if (SirenixEditorGUI.Button(_modifyBtnText, ButtonSizes.Medium))
            {
                //变量下拉框
                VariableDropView dropView = new(TreeItem, OnVariableSelect, null, true);
                dropView.Show(GUILayoutUtility.GetLastRect());
            }

            GUILayout.Space(30);

            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            object value;
            if (_curSelectVarType == typeof(int))
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Add,
                                                     (int)EVariableOperationType.Sub
                                                 },
                                                 new[] { "重设", "+", "*" });

                value = SirenixEditorFields.IntField("Value: ", int.Parse(TempData.value));
            }
            else if (_curSelectVarType == typeof(float))
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Add,
                                                     (int)EVariableOperationType.Sub
                                                 },
                                                 new[] { "重设", "+", "*" });
                value = SirenixEditorFields.FloatField("Value: ", float.Parse(TempData.value));
            }
            else if (_curSelectVarType == typeof(bool))
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Reverse,
                                                 },
                                                 new[] { "重设", "取反" });
                value = PowerEditorUIHelper.BoolDropField("Value: ", bool.Parse(TempData.value));
            }
            else
            {
                value = SirenixEditorFields.TextField("Value: ", TempData.value);
            }

            TempData.value = value.ToString();
            TempData.operationType = (EVariableOperationType)_operation;
            EditorGUIUtility.labelWidth = old;
            EditorGUILayout.EndHorizontal();
        }

        private void OnVariableSelect(string key, Type type)
        {
            _modifyBtnText = key + $"({type})";
            TempData.key = key;
            _curSelectVarType = type;
            TempData.valueType = type.ToString();
        }

        protected override void SaveDataToEditorNode()
        {
            base.SaveDataToEditorNode();
            if (!TempData.isModify)
            {
                AEditorVariableBoard.CycleVariableRefresh(TreeItem.Node.Root);
            }
        }
    }
}