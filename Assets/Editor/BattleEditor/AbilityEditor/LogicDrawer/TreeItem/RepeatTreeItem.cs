
using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : AbilityLogicTreeItem
    {
        private new RepeatNodeData _nodeData;

        public RepeatTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (RepeatNodeData)base._nodeData;
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
            return Color.green;
        }

        protected override string getButtonText()
        {
            return "循环";
        }

        protected override string getButtonTips()
        {
            return "循环节点，循环指定次数";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<RepeatNodeDataWindow, RepeatNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) =>  _tree.TreeData.NodeDict[nodeData.NodeId] = nodeData);
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class RepeatNodeDataWindow : BaseNodeWindow<RepeatNodeDataWindow,RepeatNodeData>, IAbilityNodeWindow<RepeatNodeData>
    {

        private ParameterField _maxCount;
        protected override void onInit()
        {
            _maxCount = new ParameterField(_nodeData.MaxRepeatCount, "循环次数", typeof(int));
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("设置循环次数");
            
            _maxCount.Draw();
            
            if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
            {
                Save();
            }
            SirenixEditorGUI.EndBox();
        }
    }
}