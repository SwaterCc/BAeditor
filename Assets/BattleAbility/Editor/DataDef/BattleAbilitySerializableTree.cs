using System.Collections.Generic;

namespace BattleAbility.Editor
{
    public class BattleAbilitySerializableTree
    {
        /// <summary>
        /// 树的id
        /// </summary>
        public int treeId;
        /// <summary>
        /// root节点在allNodes中的下标
        /// </summary>
        public int rootKey = -1;
        public Dictionary<int, TreeNode> allNodes = new();
        public Dictionary<int, EditorBattleTempValue> allLocalValue = new();
        
        public class TreeNode
        {
            public int nodeId;
            public int parentId;
            public List<int> childIds = new();
            public ENodeType eNodeType;
            public object nodeData;//抽象一个数据类，后期用反射实现多态
        }
    }
    
    public class EditorBattleTempValue
    {
        public int id;
        public string name;
        public string valueType;
        public string value;
        public ELocalValueRange enableRange;
    }

    public class LogicTreeNodeDataBase
    {
        
    }
}