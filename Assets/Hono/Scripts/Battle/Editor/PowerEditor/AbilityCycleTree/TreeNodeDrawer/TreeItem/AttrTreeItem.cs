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
    public class AttrTreeItem : ATreeItem<AttrNodeData>
    {
        public AttrTreeItem(AbilityCycleTree tree, ATreeEditorNode data) : base(tree, data)
        {
          ButtonBackGroundColor =Color.magenta;
        }

        
        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildLimit)
            {
                return ERightMenuState.NoShow;
            }

            return ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            string attrName = "";
            if (Enum.GetName(typeof(EAttrType), Data.attrType) == null)
            {
                attrName = "未设置";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), Data.attrType);
            }

            return "设置属性 (属性Id:" + attrName + ") = " + Data.Value;
        }
        

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<AttrSettingWindow>(this);
        }
    }

    public class AttrSettingWindow : ANodeSettingWindow<AttrNodeData>
    {
        private ParameterField _value;
        private EAttrType _curSelect;
        
        private Vector2 _dropDownPos;
        private string _searchString;
        private bool _showDropDown;

        protected override void Init()
        {
            _searchString = "";
            _value = new ParameterField(TempData.Value, "属性值：", typeof(int));
            _dropDownPos = Vector2.zero;
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置属性");
            string attrName ="";
            if (Enum.GetName(typeof(EAttrType), TempData.attrType) == null)
            {
                attrName = "未设置";
            }
            else
            {
                attrName = Enum.GetName(typeof(EAttrType), TempData.attrType);
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

            TempData.IsPersistent = EditorGUILayout.Toggle("是否为常驻属性（不勾选则会在ability删除时撤销本次修改）", TempData.IsPersistent);
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
                    _showDropDown = false; // 选择后关闭下拉框
                }
            }

            EditorGUILayout.EndScrollView();
            SirenixEditorGUI.EndVerticalList();
        }
    }
}