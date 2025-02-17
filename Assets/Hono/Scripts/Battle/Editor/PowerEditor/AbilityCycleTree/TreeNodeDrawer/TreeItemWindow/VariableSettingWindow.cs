using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class VariableSettingWindow : ANodeSettingWindow<VariableNodeData>
    {
        private string _beforeType;

        private string _modifyBtnText;
        private int _operation;
        private AParamsField _varField;
        private Type _curSelectVarType;

        protected override void Init()
        {
            _operation = (int)TempData.operationType;

            _varField = new AParamsField(TreeItem, TempData.value, "自定义变量值:",  typeof(object));
            _curSelectVarType = typeof(object);
            if (!string.IsNullOrEmpty(TempData.value.paramCastType))
            {
                var varType = Type.GetType(TempData.value.paramCastType);
                if (varType != null)
                {
                    _curSelectVarType = varType;
                }
            }

            _modifyBtnText = string.IsNullOrEmpty(TempData.key) && _curSelectVarType != null ? "选择变量" : TempData.key + $"({_curSelectVarType.Name})";
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

            _varField.Draw();
        }

        /// <summary>
        /// 修改变量
        /// </summary>
        private void drawModifyView()
        {
            GUILayout.Space(5);
            if (SirenixEditorGUI.Button(_modifyBtnText, ButtonSizes.Large))
            {
                //变量下拉框
                VariableDropView dropView = new(TreeItem, OnVariableSelect, null, true);
                dropView.Show(GUILayoutUtility.GetLastRect());
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            var old = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;

            if (_curSelectVarType == typeof(int) || _curSelectVarType == typeof(RefInt))
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Add,
                                                     (int)EVariableOperationType.Sub
                                                 },
                                                 new[] { "重设", "+", "*" }, GUILayout.Width(60));
            }
            else if (_curSelectVarType == typeof(float) || _curSelectVarType == typeof(RefFloat))
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Add,
                                                     (int)EVariableOperationType.Sub
                                                 },
                                                 new[] { "重设", "+", "*" }, GUILayout.Width(60));
            }
            else if (_curSelectVarType == typeof(bool) || _curSelectVarType == typeof(RefBoolean))
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Reverse,
                                                 },
                                                 new[] { "重设", "取反" }, GUILayout.Width(60));
            }
            else
            {
                _operation =
                    SirenixEditorFields.Dropdown(_operation,
                                                 new[]
                                                 {
                                                     (int)EVariableOperationType.Reset,
                                                     (int)EVariableOperationType.Reverse,
                                                 },
                                                 new[] { "重设" }, GUILayout.Width(60));
            }

            _varField.CastValueType(ARef.ParseValueTypeToARefType(_curSelectVarType));
            _varField.Draw();
            TempData.operationType = (EVariableOperationType)_operation;
            EditorGUIUtility.labelWidth = old;
            EditorGUILayout.EndHorizontal();
        }

        private void OnVariableSelect(string key, Type type)
        {
            _modifyBtnText = key + $"({type.Name})";
            TempData.key = key;
            _curSelectVarType = type;
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