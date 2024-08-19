using Battle.Def;
using UnityEditor.IMGUI.Controls;

namespace Editor.AbilityEditor
{
    public class AbilityLogicTreeDrawer : TreeView
    {
        public AbilityLogicTreeDrawer(TreeViewState state, AbilityData data, int headId) : base(state)
        {
            
            
        }

        protected override TreeViewItem BuildRoot()
        {
            throw new System.NotImplementedException();
        }
    }
}