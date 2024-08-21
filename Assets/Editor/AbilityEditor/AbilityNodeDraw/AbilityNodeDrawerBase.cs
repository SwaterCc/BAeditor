using Battle;

namespace Editor.AbilityEditor.AbilityNodeDraw
{
    public class AbilityNodeDrawerBase
    {
        public AbilityNodeData NodeData;

        protected AbilityData _data;

        public AbilityNodeDrawerBase(AbilityData data)
        {
            _data = data;
        }
    }
}