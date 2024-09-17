using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class AttrSetterTreeItem : AbilityLogicTreeItem
    {
        private new AttrSetterNodeData _nodeData;

        public AttrSetterTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _nodeData = (AttrSetterNodeData)base._nodeData;
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

            if (checkHasParent(EAbilityNodeType.ERepeat))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Repeat节点"), false,
                    AddChild, (EAbilityNodeType.ERepeat));
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
            return Color.magenta;
        }

        protected override string getButtonText()
        {
            return "设置属性";
        }

        protected override string getButtonTips()
        {
            return "设置属性";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<AttrSetterWindow, AttrSetterNodeData>.GetSettingWindow(_tree.TreeData,
                _nodeData,
                (nodeData) => _nodeData = nodeData);
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class AttrSetterWindow : BaseNodeWindow<AttrSetterWindow, AttrSetterNodeData>,
        IAbilityNodeWindow<AttrSetterNodeData>
    {
        protected override void onInit() { }
    }
}