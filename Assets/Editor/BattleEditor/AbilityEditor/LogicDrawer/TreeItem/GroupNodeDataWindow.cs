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

        [Button("保存")]
        public void SaveData()
        {
            NodeData.GroupNodeData.GroupId = GroupId;
            NodeData.GroupNodeData.Desc = Desc;
            Close();
        }

        protected override void onInit()
        {
            GroupId = NodeData.GroupNodeData.GroupId;
            Desc = NodeData.GroupNodeData.Desc;
        }
    }
}