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
        public int rootKey;

        public Dictionary<int, TreeNode> allNodes = new();
        public Dictionary<int, EditorBattleVariable> allVariables = new();

        public class TreeNode
        {
            /// <summary>
            /// Node节点在这棵树中唯一HashId
            /// </summary>
            public int nodeId;

            /// <summary>
            /// 父节点Id
            /// </summary>
            public int parentKey;

            /// <summary>
            /// 在树中的深度
            /// </summary>
            public int depth;

            /// <summary>
            /// 子节点ID
            /// </summary>
            public List<int> childKeys = new();

            /// <summary>
            /// 节点类型
            /// </summary>
            public ENodeType eNodeType;

            /// <summary>
            /// 节点数据
            /// </summary>
            public object nodeData; //抽象一个数据类，后期用反射实现多态
        }

        public static TreeNode GetNode(BattleAbilitySerializableTree treeData, ENodeType eNodeType)
        {
            var newEventNode = new TreeNode();
            var hashId = BattleAbilitEditorHelper.GenerateTimeBasedHashId32();
            int maxCount = 100;
            int tryCount = 0;
            while (treeData.allNodes.ContainsKey(hashId) && ++tryCount < maxCount)
            {
                hashId = BattleAbilitEditorHelper.GenerateTimeBasedHashId32();
            }

            if (tryCount == maxCount)
            {
                //报个警告
            }

            newEventNode.nodeId = hashId;
            newEventNode.eNodeType = eNodeType;
            treeData.allNodes.Add(hashId, newEventNode);
            return newEventNode;
        }

        public class EditorBattleVariable
        {
            public int id;
            public string name;
            public string valueType;
            public string value;
            public ELocalValueRange enableRange;
        }

        public static EditorBattleVariable GetVariable(BattleAbilitySerializableTree treeData)
        {
            var newVariable = new EditorBattleVariable();
            var hashId = BattleAbilitEditorHelper.GenerateTimeBasedHashId32();
            int maxCount = 100;
            int tryCount = 0;
            while (treeData.allVariables.ContainsKey(hashId) && ++tryCount < maxCount)
            {
                hashId = BattleAbilitEditorHelper.GenerateTimeBasedHashId32();
            }

            if (tryCount == maxCount)
            {
                //报个警告
            }

            newVariable.id = hashId;
            treeData.allVariables.Add(hashId, newVariable);
            return newVariable;
        }
    }


    public class LogicTreeNodeDataBase
    {
    }
}