using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupTreeItem : AbilityLogicTreeItem
    {
        private new GroupNodeData _nodeData;

        public GroupTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (GroupNodeData)base._nodeData;
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

            if (!checkHasParent(EAbilityNodeType.ERepeat))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                    AddChild, (EAbilityNodeType.ERepeat));
            }

            if (!checkHasParent(EAbilityNodeType.ETimer))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                    AddChild, (EAbilityNodeType.ETimer));
            }
        }

        protected override Color getButtonColor()
        {
            return new Color(0, 0.5f, 1.5f);
        }

        protected override string getButtonText()
        {
            return $"GroupID <{_nodeData.GroupId}> " + _nodeData.Desc;
        }

        protected override string getButtonTips()
        {
            return "Group节点的子节点 是在Group被调用后的下一帧开始执行";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<GroupNodeDataWindow, GroupNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => _nodeData = nodeData);
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class GroupNodeDataWindow : BaseNodeWindow<GroupNodeDataWindow, GroupNodeData>,
        IAbilityNodeWindow<GroupNodeData>
    {
        protected override void onInit() { }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("设置Group");
            _nodeData.GroupId = SirenixEditorFields.IntField("阶段Id", _nodeData.GroupId);
            var boolStr =
                SirenixEditorFields.Dropdown(new GUIContent("是否为默认阶段:"), _nodeData.IsDefaultStart.ToString(),
                    new[] { "true", "false" });
            _nodeData.IsDefaultStart = bool.Parse(boolStr);

            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Large))
            {
                Save();
            }

            SirenixEditorGUI.EndBox();
        }
    }
}