using Hono.Scripts.Battle;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : AbilityLogicTreeItem
    {
        public CycleTreeItem(AbilityLogicTree tree,AbilityNodeData nodeData) : base(tree,nodeData)
        {
            base.depth = 0;
            _nodeData.Depth = 0;
        }

        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            return ((EditorCycleNodeData)_nodeData).AllowEditCycleNodeData.ToString();
        }

        protected override float getButtonWidth()
        {
            return 200;
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
            menu.AddItem(new GUIContent("�����ڵ�/����Stage�ڵ�"), false,
                AddNode, EAbilityNodeType.EGroup);
            menu.AddItem(new GUIContent("�����ڵ�/����Timer�ڵ�"), false,
                AddNode, EAbilityNodeType.ETimer);
            return menu;
        }

        protected override GUIStyle getButtonTextStyle()
        {
            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            return buttonStyle;
        }

        protected override string getButtonTips()
        {
            return "���ڸ��ڵ㣬����չʾ";
        }

        protected override void OnBtnClicked() { }
    }
}