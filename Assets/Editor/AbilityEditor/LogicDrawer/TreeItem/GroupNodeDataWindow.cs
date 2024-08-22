using Sirenix.OdinInspector;

namespace Editor.AbilityEditor.TreeItem
{
    public class GroupNodeDataWindow : BaseNodeOdinWindow<GroupNodeDataWindow>, IWindowInit
    {
        [BoxGroup("Group配置")]
        [InfoBox("阶段ID不可以重复！")]
        [LabelText("组ID")]
        public int GroupId;
        
        [BoxGroup("Group配置")]
        [LabelText("阶段描述")]
        public string Desc;
        
        [BoxGroup("Group配置")]
        [LabelText("是否为默认开启阶段")]
        public bool IsDefaultStart;
        
        protected override void onInit()
        {
            StageId = NodeData.groupNodeData.GroupId;
            Desc = NodeData.groupNodeData.Desc;
            IsDefaultStart = NodeData.groupNodeData.IsDefaultStart;
        }

        protected override void OnDestroy()
        {
            NodeData.groupNodeData.GroupId = StageId;
            NodeData.groupNodeData.Desc = Desc;
            NodeData.groupNodeData.IsDefaultStart = IsDefaultStart;
            base.OnDestroy();
        }
    }
}