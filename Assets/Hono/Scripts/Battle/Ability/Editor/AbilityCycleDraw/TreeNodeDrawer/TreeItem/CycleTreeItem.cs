using System;
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class CycleTreeItem : AbilityLogicTreeItem
    {
        private CycleNodeData _cycleNode;
        public CycleTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            base.depth = 0;
            _cycleNode = (CycleNodeData)_nodeData;
            _nodeData.Depth = 0;
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("添加Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("添加If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("Set变量"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));
            _menu.AddItem(new GUIContent("创建Event节点"), false,
                AddChild, (EAbilityNodeType.EEvent));
            _menu.AddItem(new GUIContent("创建Repeat节点"), false,
                AddChild, (EAbilityNodeType.ERepeat));
            _menu.AddItem(new GUIContent("创建Group节点"), false,
                AddChild, (EAbilityNodeType.EGroup));
            _menu.AddItem(new GUIContent("创建Timer节点"), false,
                AddChild, (EAbilityNodeType.ETimer));
        }

        protected override Color getButtonColor()
        {
            return Color.black;
        }

        protected override string getButtonText()
        {
            string desc = "";
            switch (_cycleNode.cycleNodeData)
            {
                case EAbilityCycle.Init:
                    desc = "Init(初始化阶段)";
                    break;
                case EAbilityCycle.PreExecute:
                    desc = "PreExecute(预启动阶段)";
                    break;
                case EAbilityCycle.Executing:
                    desc = "Executing(执行阶段)";
                    break;
                case EAbilityCycle.EndExecute:
                    desc = "EndExecute(执行结束阶段)";
                    break;
            }
            
            return desc;
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
            string desc = "";
            switch (_cycleNode.cycleNodeData)
            {
                case EAbilityCycle.Init:
                    desc = "Ability被授予后下一帧执行该周期，执行后会进入Ready状态";
                    break;
                case EAbilityCycle.PreExecute:
                    desc = "启动前的阶段，下一阶段进入执行阶段";
                    break;
                case EAbilityCycle.Executing:
                    desc = "执行阶段";
                    break;
                case EAbilityCycle.EndExecute:
                    desc = "执行阶段结束后会进入该阶段，此时数据还未清理，可以做一些自定义逻辑，之后会重置数据";
                    break;
            }
            return desc;
        }
    }
}