using System.Collections.Generic;

namespace BattleAbility
{
    /// <summary>
    /// 能力逻辑树节点,抽象层面的
    /// 
    /// 配置数据以树形结构来构造
    /// 1.root必须存在一个Timing节点
    /// 2.判定节点不能作为叶节点，如果存在空判定则自动补全一个Action:NoThing
    ///
    /// 这里读取数据，构成树后缓存
    /// </summary>
    public class BattleAbilityLogicTreeNode
    {
        private int _treeNodeId;
        private ENodeType _curNodeType;
        
        /// <summary>
        /// 父节点
        /// </summary>
        private BattleAbilityLogicTreeNode _parent;
        
        /// <summary>
        /// 记录子节点，有序的，所以使用List
        /// </summary>
        private readonly List<BattleAbilityLogicTreeNode> _children = new();
        
        public int GetId() => _treeNodeId;
        public List<BattleAbilityLogicTreeNode> GetChildren() => _children;

        public bool TryGetParent(out BattleAbilityLogicTreeNode parent) => (parent = _parent) == null;
        public bool IsRoot() => _parent == null;
        
        //解析数据构成树
        public void Setup()
        {
            
        }
    }
}