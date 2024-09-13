
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
            return "Group节点的子节点 是在Group被调用后的下一帧开始执行";
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
            menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
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