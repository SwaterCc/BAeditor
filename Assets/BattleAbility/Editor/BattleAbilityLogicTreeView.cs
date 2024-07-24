using Sirenix.OdinInspector;

namespace BattleAbility.Editor
{
    public class BattleAbilityLogicTreeView
    {
        private BattleAbilitySerializableTree _serializableTree;
        
        //定义编辑器树结构
        [InfoBox("这是一颗TREE")]
        //
        
        public BattleAbilityLogicTreeView(BattleAbilitySerializableTree serializableTree)
        {
            _serializableTree = serializableTree;

            MakeTree();
        }

        public void MakeTree()
        {
            
        }
    }
}