using System;
using Battle;
using Battle.Def;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : AbilityLogicTreeItem
    {
        public RepeatTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public RepeatTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return Color.grey;
        }

        protected override string getButtonText()
        {
            var rData = NodeData.RepeatNodeData;
            string a = rData.RepeatOperationType == ERepeatOperationType.OnlyRepeat
                ? $"循环次数{rData.MaxRepeatCount}"
                : $"for step = {rData.StartValue} ; count = {rData.StepCount} ; step + {rData.StepValue}";

            return a;
        }

        protected override void OnBtnClicked()
        {
            RepeatNodeDataWindow.Open(NodeData);
        }
    }

    public class RepeatNodeDataWindow : BaseNodeOdinWindow<RepeatNodeDataWindow>, IWindowInit
    {
        [BoxGroup("循环节点配置", true, true)] [EnumToggleButtons]
        public ERepeatOperationType RepeatOperationType;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.OnlyRepeat)]
        public int MaxRepeatCount;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.NumberLoop)]
        public float StartValue;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.NumberLoop)]
        public float StepValue;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.NumberLoop)]
        public int StepCount;

        protected override void onInit()
        {
            RepeatOperationType = NodeData.RepeatNodeData.RepeatOperationType;
            MaxRepeatCount = NodeData.RepeatNodeData.MaxRepeatCount;
            StartValue = NodeData.RepeatNodeData.StartValue;
            StepValue = NodeData.RepeatNodeData.StepValue;
            StepCount = NodeData.RepeatNodeData.StepCount;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            NodeData.RepeatNodeData.RepeatOperationType = RepeatOperationType;
            NodeData.RepeatNodeData.MaxRepeatCount = MaxRepeatCount;
            NodeData.RepeatNodeData.StartValue = StartValue;
            NodeData.RepeatNodeData.StepValue = StepValue;
            NodeData.RepeatNodeData.StepCount = StepCount;
        }
    }
}