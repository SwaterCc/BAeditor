using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class VarSetterTreeItem : AbilityLogicTreeItem
    {
        private new VarSetterNodeData _nodeData;

        public VarSetterTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (VarSetterNodeData)base._nodeData;
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("删除"), false,
                Remove);
        }

        protected override Color getButtonColor()
        {
            return new Color(0.6f, 0.3f, 0.95f);
        }

        protected override string getButtonText()
        {
            var parentData = _tree.TreeData.NodeDict[_nodeData.ParentId];
            string name = string.IsNullOrEmpty(_nodeData.Name) ? "未设置" : _nodeData.Name;
            if (parentData is ActionNodeData parentActionData)
            {
              
                if(AbilityFunctionHelper.TryGetFuncInfo(parentActionData.Function.FuncName, out var funcInfo))
                {
                    if (funcInfo.ReturnType == typeof(void))
                    {
                        return "获取返回值失败函数没有返回值!!";
                    }
                    
                    return "获取返回值 (name：" + name + ") 返回值类型：" + funcInfo.ReturnType;
                }
                return "获取函数失败";
            }

            return "设置变量 " + name + " = " + _nodeData.Value;
        }

        protected override string getButtonTips()
        {
            return "Set变量";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            AbilityViewDrawer.NodeBtnClick(_nodeData);
            SettingWindow = BaseNodeWindow<VarNodeDataWindow, VarSetterNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => { _tree.TreeData.NodeDict[nodeData.NodeId] = nodeData;
                    _nodeData = nodeData;
                });
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class VarNodeDataWindow : BaseNodeWindow<VarNodeDataWindow, VarSetterNodeData>,
        IAbilityNodeWindow<VarSetterNodeData>
    {
        private ParameterField _value;

        private List<string> _dropList = new List<string>()
        {
            "int",
            "float",
            "bool",
            "string",
            "intList",
            "floatList",
            "custom"
        };

        private string _curSelect;
        private string _customTypeStr;
        private bool _customCastSuccess;

        protected override void onInit()
        {
            _value = new ParameterField(_nodeData.Value, "变量值：",
                AbilityFunctionHelper.GetVariableType(_nodeData.typeString));
            _curSelect = _nodeData.typeString;
            if (_curSelect == "custom")
            {
                _customTypeStr = _curSelect;
                _customCastSuccess = Type.GetType(_customTypeStr) != null;
            }
            else
            {
                _customCastSuccess = true;
            }
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox();

            _nodeData.Name = SirenixEditorFields.TextField("变量名：", _nodeData.Name);
            _curSelect = SirenixEditorFields.Dropdown(new GUIContent("变量类型"), _curSelect, _dropList);
            if (_curSelect != _nodeData.typeString)
            {
                if (_curSelect != "custom")
                {
                    _value = new ParameterField(_nodeData.Value, "变量值：",
                        AbilityFunctionHelper.GetVariableType(_nodeData.typeString));
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    _customTypeStr = SirenixEditorFields.TextField("类型字符串", _customTypeStr);
                    if (SirenixEditorGUI.Button("转换检测", ButtonSizes.Medium))
                    {
                        var customType = Type.GetType(_customTypeStr);
                        _customCastSuccess = customType != null;
                        if (_customCastSuccess)
                        {
                            _value = new ParameterField(_nodeData.Value, "变量值：", customType);
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (_customCastSuccess)
            {
                _value.Draw();
            }
            else
            {
                EditorGUILayout.LabelField("类型转换失败！");
            }

            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Large))
            {
                if (_customCastSuccess)
                {
                    AbilityViewDrawer.VarCollector.RefreshAllVariable();
                    Save();
                }
                else
                {
                    Debug.LogError("无法识别类型，无法保存");
                }
            }

            SirenixEditorGUI.EndBox();
        }
    }
}