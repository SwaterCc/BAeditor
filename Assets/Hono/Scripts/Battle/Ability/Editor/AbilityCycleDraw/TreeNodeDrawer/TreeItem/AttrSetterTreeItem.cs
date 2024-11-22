using System;
using System.Collections.Generic;
using System.Linq;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class AttrSetterTreeItem : AbilityLogicTreeItem
    {
        private new AttrNodeData _nodeData;

        public AttrSetterTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (AttrNodeData)base._nodeData;
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("删除"), false,
                Remove);
        }

        protected override Color getButtonColor()
        {
            return Color.magenta;
        }

        protected override string getButtonText()
        {
            string attrName = "";
            if (Enum.GetName(typeof(EAttrType), _nodeData.attrType) == null)
            {
                attrName = "未设置";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), _nodeData.attrType);
            }

            return "设置属性 (属性Id:" + attrName + ") = " + _nodeData.Value;
        }

        protected override string getButtonTips()
        {
            return "仅设置该Ability归属的Actor的属性";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            AbilityViewDrawer.NodeBtnClick(_nodeData);
            SettingWindow = BaseNodeWindow<AttrSetterWindow, AttrNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => { _tree.TreeData.NodeDict[nodeData.NodeId] = nodeData;
                    _nodeData = nodeData;
                });
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class AttrSetterWindow : BaseNodeWindow<AttrSetterWindow, AttrNodeData>,
        IAbilityNodeWindow<AttrNodeData>
    {
        private ParameterField _value;
        private EAttrType _curSelect;
        private Type _customType;
        private bool _customCastSuccess;
        private Vector2 _dropDownPos;
        private string _searchString;
        private bool _showDropDown;

        protected override void onInit()
        {
            _searchString = "";
            _customType = typeof(int);

            if (_customType != null)
            {
                _value = new ParameterField(_nodeData.Value, "属性值：", _customType);
            }

            _dropDownPos = Vector2.zero;
            _curSelect = _nodeData.attrType;
        }
        
        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("设置属性");

            string attrName ="";
            if (Enum.GetName(typeof(EAttrType), _nodeData.attrType) == null)
            {
                attrName = "未设置";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), _nodeData.attrType);
            }
            
            EditorGUILayout.LabelField("当前属性：" + attrName);
            
            if (!_showDropDown)
            {
                if (SirenixEditorGUI.Button("选择属性", ButtonSizes.Medium))
                {
                    _showDropDown = true;
                }
            }

            if (_showDropDown)
            {
                drawDropDown();
            }

            _value?.Draw();

            _nodeData.IsTempAttr = EditorGUILayout.Toggle("是否跟随Ability结束时删除", _nodeData.IsTempAttr);

            SirenixEditorGUI.EndBox();

            SirenixEditorGUI.BeginBox();
            
            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Large))
            {
                Save();
            }

            SirenixEditorGUI.EndBox();
        }
        
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
            foreach (var item in Enum.GetNames(typeof(EAttrType))
                         .Where(i => i.ToLower().Contains(_searchString.ToLower())))
            {
                if (SirenixEditorGUI.Button(item, ButtonSizes.Medium))
                {
                    _curSelect = Enum.Parse<EAttrType>(item);

                    if (_curSelect != _nodeData.attrType)
                    {
                        _nodeData.attrType = _curSelect;
                       
                        if (_customType != null)
                        {
                            _value = new ParameterField(_nodeData.Value, "属性值：", _customType);
                        }
                    }
                    
                    _showDropDown = false; // 选择后关闭下拉框
                }
            }

            EditorGUILayout.EndScrollView();
            SirenixEditorGUI.EndVerticalList();
        }
    }
}