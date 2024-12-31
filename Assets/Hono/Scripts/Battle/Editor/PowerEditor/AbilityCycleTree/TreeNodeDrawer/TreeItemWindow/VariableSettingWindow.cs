using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class VariableSettingWindow : ANodeSettingWindow<VariableNodeData>
    {
        private string[] _baseTypeShow = { "int", "float", "bool", "string" };
        private string[] _baseTypeValue =
            { typeof(int).ToString(), typeof(float).ToString(), typeof(bool).ToString(), typeof(string).ToString() };

        private string _beforeType;
        
        protected override void Init()
        {
            //创建变量 基础类型 int float bool string
            //修改变量 下拉框选择已有变量  -> 行为自增，自减，加值，乘值，取反，重设
            //int float bool 可能来自属性？可能性很低，暂时不做
            //变量存储变量类型？一般不会有
            _beforeType = TempData.valueType;
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
            
            if (AbilityView.VariableBoard.HasVariable(TempData.key))
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
            //通过操作筛选变量
            TempData.operationType = (EVariableOperationType)SirenixEditorFields.EnumDropdown("变量操作类型", TempData.operationType);
            
            var buttonText = TempData.key;
            Type varType = TempData.GetType();
            if (string.IsNullOrEmpty(TempData.key))
            {
                buttonText = "选择变量";
            }

            if (SirenixEditorGUI.Button(buttonText, ButtonSizes.Medium))
            {
                //变量下拉框
                
            }

            switch (TempData.operationType)
            {
                case EVariableOperationType.Add:
                    break;
                case EVariableOperationType.Sub:
                    break;
                case EVariableOperationType.Reverse:
                    break;
            }
        }

        protected override void SaveDataToEditorNode()
        {
            base.SaveDataToEditorNode();
            if (!TempData.isModify)
            {
                AbilityView.VariableBoard.RefreshAllVariable();
            }
        }
    }
}