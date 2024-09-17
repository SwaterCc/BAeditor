using System;
using System.Collections.Generic;
using System.Linq;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class VariableTreeItem : AbilityLogicTreeItem
    {
        private new VarSetterNodeData _nodeData;

        public VariableTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (VarSetterNodeData)base._nodeData;
        }

        public override void DrawItem(Rect lineRect)
        {
            base.DrawItem(lineRect);
            if (!string.IsNullOrEmpty(_nodeData.Name))
            {
                AbilityView.VariableCollector.Add(AbilityFunctionHelper.GetVariableType(_nodeData.typeString),_nodeData.Name);
            }
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("创建节点/添加Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("创建节点/添加If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("创建节点/Set变量"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("创建节点/SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));
            if (checkHasParent(EAbilityNodeType.EEvent))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                    AddChild, (EAbilityNodeType.EEvent));
            }

            if (checkHasParent(EAbilityNodeType.ERepeat))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                    AddChild, (EAbilityNodeType.ERepeat));
            }

            if (checkHasParent(EAbilityNodeType.EGroup))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Group节点"), false,
                    AddChild, (EAbilityNodeType.EGroup));
            }

            if (checkHasParent(EAbilityNodeType.ETimer))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                    AddChild, (EAbilityNodeType.ETimer));
            }
        }

        protected override Color getButtonColor()
        {
            return new Color(0.6f, 0.3f, 0.95f);
        }

        protected override string getButtonText()
        {
            return "调用并Set变量";
        }

        protected override string getButtonTips()
        {
            return "调用并Set变量";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<VarNodeDataWindow, VarSetterNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => _tree.TreeData.NodeDict[nodeData.NodeId] = nodeData);
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
                    Save();
                else
                {
                    Debug.LogError("无法识别类型，无法保存");
                }
            }

            SirenixEditorGUI.EndBox();
        }
    }
}