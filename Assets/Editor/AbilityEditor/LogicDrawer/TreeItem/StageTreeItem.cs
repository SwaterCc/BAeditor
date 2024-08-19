using Battle.Def;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class StageTreeItem : AbilityLogicTreeItem
    {
        public StageTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public StageTreeItem(AbilityNodeData nodeData) : base(nodeData) { }
        protected override Color getButtonColor()
        {
            return new Color(0, 0.5f, 1f);
        }

        protected override string getButtonText()
        {
            return $"ID <{NodeData.StageNodeData.StageId}> :" + NodeData.StageNodeData.Desc;
        }

        protected override void OnBtnClicked()
        {
            throw new System.NotImplementedException();
        }
    }

    public class StageNodeDataWindow : BaseNodeOdinWindow<StageNodeDataWindow>, IWindowInit
    {
        [BoxGroup("阶段配置")]
        [InfoBox("阶段ID不可以重复！")]
        [Title("阶段ID")]
        public int StageId;
        
        [BoxGroup("阶段配置")]
        [Title("阶段描述")]
        public string Desc;
        
        [BoxGroup("阶段配置")]
        [Title("是否为默认开启阶段")]
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