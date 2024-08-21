using Sirenix.OdinInspector;

namespace Editor.AbilityEditor.TreeItem
{
    public class TimerNodeDataWindow : BaseNodeOdinWindow<TimerNodeDataWindow>, IWindowInit
    {
        [BoxGroup("计时器配置")] [LabelText("第一次调用间隔")] public float FirstInterval;

        [BoxGroup("计时器配置")] [LabelText("之后每次调用间隔")]
        public float Interval;

        [BoxGroup("计时器配置")] [LabelText("回调次数")] public float MaxCount;

        protected override void onInit()
        {
            FirstInterval = NodeData.TimerNodeData.FirstInterval;
            Interval = NodeData.TimerNodeData.Interval;
            MaxCount = NodeData.TimerNodeData.MaxCount;
        }

        protected override void OnDestroy()
        {
            NodeData.TimerNodeData.FirstInterval = FirstInterval;
            NodeData.TimerNodeData.Interval = Interval;
            NodeData.TimerNodeData.MaxCount = MaxCount;
            base.OnDestroy();
        }
    }
}