
using Hono.Scripts.Battle;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupTreeItem : AbilityLogicTreeItem
    {
        public GroupTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public GroupTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree,nodeData) { }
        protected override Color getButtonColor()
        {
            return new Color(0, 0.5f, 1.5f);
        }

        protected override string getButtonText()
        {
            return $"GroupID <{_nodeData.GroupNodeData.GroupId}> " + _nodeData.GroupNodeData.Desc;
        }

        protected override string getButtonTips()
        {
            return "Group�ڵ���ӽڵ� ����Group�����ú����һ֡��ʼִ��";
        }

        public override GenericMenu GetGenericMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("�����ڵ�/���Action�ڵ�"), false,
                AddNode, EAbilityNodeType.EAction);
            menu.AddItem(new GUIContent("�����ڵ�/��ӷ�֧�ڵ�"), false,
                AddNode,  EAbilityNodeType.EBranchControl);
            menu.AddItem(new GUIContent("�����ڵ�/�����������ƽڵ�"), false,
                AddNode, EAbilityNodeType.EVariableControl);
            menu.AddItem(new GUIContent("�����ڵ�/����Event�ڵ�"), false,
                AddNode, EAbilityNodeType.EEvent);
            menu.AddItem(new GUIContent("�����ڵ�/����Repeat�ڵ�"), false,
                AddNode, EAbilityNodeType.ERepeat);
            menu.AddItem(new GUIContent("�����ڵ�/����Timer�ڵ�"), false,
                AddNode, EAbilityNodeType.ETimer);
            return menu;
        }

        protected override void OnBtnClicked()
        { 
            SettingWindow = GroupNodeDataWindow.GetWindow(_nodeData);
           SettingWindow.Show();
           SettingWindow.Focus();
        }
    }
}