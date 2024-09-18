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
            _menu.AddItem(new GUIContent("���Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("���If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("Set����"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));
            _menu.AddItem(new GUIContent("����Event�ڵ�"), false,
                AddChild, (EAbilityNodeType.EEvent));
            _menu.AddItem(new GUIContent("����Repeat�ڵ�"), false,
                AddChild, (EAbilityNodeType.ERepeat));
            _menu.AddItem(new GUIContent("����Group�ڵ�"), false,
                AddChild, (EAbilityNodeType.EGroup));
            _menu.AddItem(new GUIContent("����Timer�ڵ�"), false,
                AddChild, (EAbilityNodeType.ETimer));
        }

        protected override Color getButtonColor()
        {
            return Color.black;
        }

        protected override string getButtonText()
        {
            string desc = "";
            switch (_cycleNode.AllowEditCycleNodeData)
            {
                case EAbilityAllowEditCycle.OnInit:
                    desc = "Init(��ʼ���׶�)";
                    break;
                case EAbilityAllowEditCycle.OnReady:
                    break;
                case EAbilityAllowEditCycle.OnPreExecuteCheck:
                    desc = "PreExecuteCheck(����ǰ���)";
                    break;
                case EAbilityAllowEditCycle.OnPreExecute:
                    desc = "PreExecute(Ԥ�����׶�)";
                    break;
                case EAbilityAllowEditCycle.OnExecuting:
                    desc = "Executing(ִ�н׶�)";
                    break;
                case EAbilityAllowEditCycle.OnEndExecute:
                    desc = "EndExecute(ִ�н����׶�)";
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
            switch (_cycleNode.AllowEditCycleNodeData)
            {
                case EAbilityAllowEditCycle.OnInit:
                    desc = "Ability���������һִ֡�и����ڣ�ִ�к�����Ready״̬";
                    break;
                case EAbilityAllowEditCycle.OnReady:
                    break;
                case EAbilityAllowEditCycle.OnPreExecuteCheck:
                    desc = "���յ�ִ��ָ��󣬻��Ready״̬����Ƿ�����ִ����������������Ԥ�����׶�";
                    break;
                case EAbilityAllowEditCycle.OnPreExecute:
                    desc = "����ǰ�Ľ׶Σ���һ�׶ν���ִ�н׶�";
                    break;
                case EAbilityAllowEditCycle.OnExecuting:
                    desc = "ִ�н׶�";
                    break;
                case EAbilityAllowEditCycle.OnEndExecute:
                    desc = "ִ�н׶ν���������ý׶Σ���ʱ���ݻ�δ����������һЩ�Զ����߼���֮�����������";
                    break;
            }
            return desc;
        }
    }
}