using System.Collections.Generic;
using BattleAbility.Editor;

namespace BattleAbility
{
    /// <summary>
    /// 逻辑树节点基类，定义作为树节点的一些基本数据，子类则实现Do函数完成节点功能
    /// </summary>
    public abstract class TreeNodeBase
    {
        protected int _treeNodeId;
        protected ENodeType _curNodeType;
        protected BattleAbilityBlock _block;

        /// <summary>
        /// 父节点
        /// </summary>
        protected TreeNodeBase _parent;

        /// <summary>
        /// 记录子节点，有序的，所以使用List
        /// </summary>
        protected readonly List<TreeNodeBase> _children = new();

        /// <summary>
        /// 树节点数据
        /// </summary>
        public BattleAbilitySerializableTree.TreeNode TreeNode { get; }

        protected NodeDataBase _nodeData;

        public bool TryGetParent(out TreeNodeBase parent) => (parent = _parent) == null;
        public bool IsRoot() => _parent == null;

        protected TreeNodeBase(BattleAbilityBlock block, BattleAbilitySerializableTree.TreeNode treeNode)
        {
            TreeNode = treeNode;
            _block = block;
            _nodeData = TreeNode.nodeData;
        }

        public void AddChild(TreeNodeBase node)
        {
            _children.Add(node);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract void RunLogic();

        public virtual bool HasNext()
        {
            return _children.Count > 0;
        }

        /// <summary>
        /// 获取下一个运行节点
        /// </summary>
        /// <returns></returns>
        public virtual TreeNodeBase GetNext()
        {
            return _children.Count > 0 ? _children[0] : null;
        }

    }
}