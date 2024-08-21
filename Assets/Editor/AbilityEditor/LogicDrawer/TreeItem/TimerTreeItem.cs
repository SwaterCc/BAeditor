using Battle.Def;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class TimerTreeItem : AbilityLogicTreeItem
    {
        public TimerTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public TimerTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.2f, 1.0f, 0.3f);
        }

        protected override string getButtonText()
        {
            return
                $"Timer First:{NodeData.TimerNodeData.FirstInterval}s,Interval:{NodeData.TimerNodeData.Interval}s,Count:{NodeData.TimerNodeData.MaxCount}";
        }

        protected override void OnBtnClicked()
        {
            TimerNodeDataWindow.Open(NodeData);
        }
    }
}