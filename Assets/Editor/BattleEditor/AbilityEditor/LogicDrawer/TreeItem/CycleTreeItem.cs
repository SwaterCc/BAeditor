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
            menu.AddItem(new GUIContent("创建节点/添加Action节点"), false,
                AddNode, EAbilityNodeType.EAction);
            menu.AddItem(new GUIContent("创建节点/添加分支节点"), false,
                AddNode,  EAbilityNodeType.EBranchControl);
            menu.AddItem(new GUIContent("创建节点/创建变量控制节点"), false,
                AddNode, EAbilityNodeType.EVariableControl);
            menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                AddNode, EAbilityNodeType.EEvent);
            menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                AddNode, EAbilityNodeType.ERepeat);
            menu.AddItem(new GUIContent("创建节点/创建Stage节点"), false,
                AddNode, EAbilityNodeType.EGroup);
            menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
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
            return "周期根节点，仅做展示";
        }

        protected override void OnBtnClicked() { }
    }
}