using System.Collections.Generic;

namespace Script.Battle
{
    /// <summary>
    /// 定义了节点通用函数
    /// </summary>
    public interface IBattleAbilityTree
    {
        public void Clear();
        /// <summary>
        /// 遍历树
        /// </summary>
        public void Deep();
        /// <summary>
        /// 返回子节点数量
        /// </summary>
        public void ChildCount();
        /// <summary>
        /// 插入子节点到指定位置
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="node"></param>
        public void InsertChild(int idx, in BattleAbilityLogicTreeNode node);
        /// <summary>
        /// 添加子节点到列表尾部
        /// </summary>
        /// <param name="node"></param>
        public void AddChildBack(in BattleAbilityLogicTreeNode node);
        /// <summary>
        /// 删除列表尾部子节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(in BattleAbilityLogicTreeNode node);
        /// <summary>
        /// 通过子节点Id删除指定子节点
        /// </summary>
        /// <param name="childId"></param>
        public void RemoveChild(int childId);
    }


    /// <summary>
    /// 能力逻辑树节点,抽象层面的
    /// 
    /// 配置数据以树形结构来构造
    /// 1.root必须存在一个Timing节点
    /// 2.判定节点不能作为叶节点，如果存在空判定则自动补全一个Action:NoThing
    ///
    /// 这里读取数据，构成树后缓存
    /// </summary>
    public class BattleAbilityLogicTreeNode : IBattleAbilityTree
    {
        /// <summary>
        /// 节点类型
        /// 与逻辑类型不同，这里的类型会根据操作动态变化，主要用于编辑器识别和做一些安全检测
        /// </summary>
        public enum ETreeNodeType
        {
            OnlyContainer,
            RootNode,
            ConditionNode,
            BehaveNode,
        }
        
        private int _treeNodeId;
        private ETreeNodeType _curNodeType;
        /// <summary>
        /// 存储自身持有的所有逻辑节点
        /// </summary>
        private readonly List<BattleAbilityLogicNode> _ownerLogicNodes = new();
        /// <summary>
        /// 记录子节点，有序的，所以使用List
        /// </summary>
        private readonly List<BattleAbilityLogicTreeNode> _children = new();

        /// <summary>
        /// 父节点
        /// </summary>
        private BattleAbilityLogicTreeNode _parent;
        
        public int GetId() => _treeNodeId;
        public List<BattleAbilityLogicTreeNode> GetChildren() => _children;
        public List<BattleAbilityLogicNode> GetLogicNodes() => _ownerLogicNodes;

        public bool TryGetParent(out BattleAbilityLogicTreeNode parent) => (parent = _parent) == null;
        public bool IsRoot() => _parent == null;
        
        //解析数据构成树
        public void Setup()
        {
            
        }
        
        public void AddLogicNodeBack(in BattleAbilityLogicNode logicNode)
        {
            
        }
        
        public void RemoveLogicNodeBack()
        {
            
        }

        #region -------------------------------接口函数---------------------------------
        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void Deep()
        {
            throw new System.NotImplementedException();
        }

        public void ChildCount()
        {
            throw new System.NotImplementedException();
        }

        public void InsertChild(int idx, in BattleAbilityLogicTreeNode node)
        {
            throw new System.NotImplementedException();
        }

        public void AddChildBack(in BattleAbilityLogicTreeNode node)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveChild(in BattleAbilityLogicTreeNode node)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveChild(int childId)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}