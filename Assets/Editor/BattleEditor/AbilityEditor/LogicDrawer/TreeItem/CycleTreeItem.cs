using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : AbilityLogicTreeItem
    {
        public CycleTreeItem(AbilityNodeData nodeData) : base(nodeData)
        {
            base.depth = 0;
            NodeData.Depth = 0;
        }

        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            return NodeData.allowEditCycleNodeData.ToString();
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

        protected override string getItemEffectInfo()
        {
            return "���ڸ��ڵ㣬����չʾ";
        }

        protected override void OnBtnClicked() { }
    }
}