
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatNodeDataWindow : BaseNodeOdinWindow<RepeatNodeDataWindow>, IWindowInit
    {
        [BoxGroup("循环节点配置", true, true)] [EnumToggleButtons]
        [LabelText("循环节点类型")]
        public ERepeatOperationType RepeatOperationType;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.OnlyRepeat)]
        [LabelText("最大循环次数")]public int MaxRepeatCount;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.NumberLoop)]
        [LabelText("起始值")]public float StartValue;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.NumberLoop)]
        [LabelText("循环增长值")]public float StepValue;

        [BoxGroup("循环节点配置", true, true)] [ShowIf("RepeatOperationType", ERepeatOperationType.NumberLoop)]
        [LabelText("循环次数")]public int StepCount;

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