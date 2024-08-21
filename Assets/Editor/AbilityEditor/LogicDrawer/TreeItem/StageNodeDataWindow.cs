using Sirenix.OdinInspector;

namespace Editor.AbilityEditor.TreeItem
{
    public class StageNodeDataWindow : BaseNodeOdinWindow<StageNodeDataWindow>, IWindowInit
    {
        [BoxGroup("阶段配置")]
        [InfoBox("阶段ID不可以重复！")]
        [LabelText("阶段ID")]
        public int StageId;
        
        [BoxGroup("阶段配置")]
        [LabelText("阶段描述")]
        public string Desc;
        
        [BoxGroup("阶段配置")]
        [LabelText("是否为默认开启阶段")]
        public bool IsDefaultStart;
        
        protected override void onInit()
        {
            StageId = NodeData.StageNodeData.StageId;
            Desc = NodeData.StageNodeData.Desc;
            IsDefaultStart = NodeData.StageNodeData.IsDefaultStart;
        }

        protected override void OnDestroy()
        {
            NodeData.StageNodeData.StageId = StageId;
            NodeData.StageNodeData.Desc = Desc;
            NodeData.StageNodeData.IsDefaultStart = IsDefaultStart;
            base.OnDestroy();
        }
    }
}