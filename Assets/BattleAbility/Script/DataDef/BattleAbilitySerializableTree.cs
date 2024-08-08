using System;
using System.Collections.Generic;
using Battle.Def;

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
            public NodeDataBase nodeData;
        }

        public static TreeNode GetNode(BattleAbilitySerializableTree treeData, ENodeType eNodeType)
        {
            var newEventNode = new TreeNode();
            var hashId = CommonUtility.GenerateTimeBasedHashId32();
            int maxCount = 100;
            int tryCount = 0;
            while (treeData.allNodes.ContainsKey(hashId) && ++tryCount < maxCount)
            {
                hashId = CommonUtility.GenerateTimeBasedHashId32();
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
    }

    public abstract class NodeDataBase
    {
    }

    /// <summary>
    /// 节点基础的参数类型
    /// </summary>
    public class TreeNodeParam
    {
        public ETreeNodeParamType ValueType;
        public Object Value;
    }


    /// <summary>
    /// 遍历节点
    /// </summary>
    public class ForEachTreeNodeData : NodeDataBase
    {
        public TreeNodeParam Source;
    }

    /// <summary>
    /// 变量操作涉及创建和修改 
    /// </summary>
    public class VariableTreeNodeData : NodeDataBase
    {
        /// <summary>
        /// 变量的操作类型
        /// </summary>
        public EVariableOperationType OperationType;

        /// <summary>
        /// 变量名
        /// </summary>
        public string Name;

        /// <summary>
        /// 值类型
        /// </summary>
        public ETreeNodeParamType ValueType;

        /// <summary>
        /// 值范围
        /// </summary>
        public ELocalValueRange Range;

        /// <summary>
        /// 值的具体值
        /// </summary>
        public Object Value;
    }


    /// <summary>
    /// 条件节点数据
    /// </summary>
    public class ConditionTreeNodeData : NodeDataBase
    {
        /// <summary>
        /// 该条件是什么类型
        /// </summary>
        public EConditionType ConditionType;

        /// <summary>
        /// 关联的条件id
        /// </summary>
        public int NextNodeId;

        /// <summary>
        /// 判定表达式，目前只支持3个，后续考虑接脚本语言，不然我就得自己写一套语义分析了
        /// </summary>
        public ConditionItem[] ConditionContext = new ConditionItem[3];

        /// <summary>
        /// 0，1两个式子之间的链接符号
        /// </summary>
        public int LinkFlag1;

        /// <summary>
        /// 1，2两个式子之间的链接符号
        /// </summary>
        public int LinkFlag2;
    }

    /// <summary>
    /// 单个条件表达式
    /// </summary>
    public class ConditionItem
    {
        /// <summary>
        /// 符号左边
        /// </summary>
        public TreeNodeParam LeftValue;

        /// <summary>
        /// 比较符号
        /// </summary>
        public ECompareType CompareType;

        /// <summary>
        /// 符号右边
        /// </summary>
        public TreeNodeParam RightValue;
    }

    /// <summary>
    /// Action节点数据
    /// </summary>
    public class ActionTreeNodeData : NodeDataBase
    {
        /// <summary>
        /// Action函数的Id，自动生成
        /// </summary>
        public int ActionFuncId;

        /// <summary>
        /// 函数重载的类型，默认是0
        /// </summary>
        public int ActionFuncStyle;

        /// <summary>
        /// 参数列表，有顺序
        /// </summary>
        public List<TreeNodeParam> Params;
    }

    /// <summary>
    /// 事件节点数据
    /// </summary>
    public class EventTreeNodeData : NodeDataBase
    {
        /// <summary>
        /// 注册的事件枚举
        /// </summary>
        public EBattleEventType EventType;

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<TreeNodeParam> Params;
    }
}