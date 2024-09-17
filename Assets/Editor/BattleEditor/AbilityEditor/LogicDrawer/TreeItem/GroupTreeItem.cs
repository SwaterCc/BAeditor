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
            _menu.AddItem(new GUIContent("�����ڵ�/���Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("�����ڵ�/���If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("�����ڵ�/Set����"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("�����ڵ�/SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));

            if (!checkHasParent(EAbilityNodeType.ERepeat))
            {
                _menu.AddItem(new GUIContent("�����ڵ�/����Repeat�ڵ�"), false,
                    AddChild, (EAbilityNodeType.ERepeat));
            }

            if (!checkHasParent(EAbilityNodeType.ETimer))
            {
                _menu.AddItem(new GUIContent("�����ڵ�/����Timer�ڵ�"), false,
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
            return "Group�ڵ���ӽڵ� ����Group�����ú����һ֡��ʼִ��";
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
            SirenixEditorGUI.BeginBox("����Group");
            _nodeData.GroupId = SirenixEditorFields.IntField("�׶�Id", _nodeData.GroupId);
            var boolStr =
                SirenixEditorFields.Dropdown(new GUIContent("�Ƿ�ΪĬ�Ͻ׶�:"), _nodeData.IsDefaultStart.ToString(),
                    new[] { "true", "false" });
            _nodeData.IsDefaultStart = bool.Parse(boolStr);

            if (SirenixEditorGUI.Button("��   ��", ButtonSizes.Large))
            {
                Save();
            }

            SirenixEditorGUI.EndBox();
        }
    }
}