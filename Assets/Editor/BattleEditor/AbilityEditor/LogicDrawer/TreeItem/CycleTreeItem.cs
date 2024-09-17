using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : AbilityLogicTreeItem
    {
        public CycleTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            base.depth = 0;
            _nodeData.Depth = 0;
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
            _menu.AddItem(new GUIContent("�����ڵ�/����Event�ڵ�"), false,
                AddChild, (EAbilityNodeType.EEvent));
            _menu.AddItem(new GUIContent("�����ڵ�/����Repeat�ڵ�"), false,
                AddChild, (EAbilityNodeType.ERepeat));
            _menu.AddItem(new GUIContent("�����ڵ�/����Group�ڵ�"), false,
                AddChild, (EAbilityNodeType.EGroup));
            _menu.AddItem(new GUIContent("�����ڵ�/����Timer�ڵ�"), false,
                AddChild, (EAbilityNodeType.ETimer));
        }

        protected override Color getButtonColor()
        {
            return Color.black;
        }

        protected override string getButtonText()
        {
            return _nodeData.ToString();
        }

        protected override float getButtonWidth()
        {
            return 200;
        }

        protected override GUIStyle getButtonTextStyle()
        {
            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            return buttonStyle;
        }

        protected override void OnBtnClicked(Rect btnRect) { }

        protected override string getButtonTips()
        {
            return "���ڸ��ڵ㣬����չʾ";
        }
    }
}