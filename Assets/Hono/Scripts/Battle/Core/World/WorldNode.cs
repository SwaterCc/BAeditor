using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 世界节点
    /// </summary>
    public abstract class WorldNode
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid { get; protected set; }

        /// <summary>
        /// 所处世界
        /// </summary>
        private WorldNodeRoot _root;
        /// <summary>
        /// 父节点
        /// </summary>
        private WorldNode _parent;
        /// <summary>
        /// 子节点
        /// </summary>
        private List<WorldNode> _children;
        /// <summary>
        /// 下一帧删除的Node
        /// </summary>
        private List<WorldNode> _nextTickRemoveChildren;
        /// <summary>
        /// 第一次Tick
        /// </summary>
        private bool isFirstTick = true;

        /// <summary>
        /// 运行第一帧回调
        /// </summary>
        public event Action<WorldNode> FirstTickCallback;

        /// <summary>
        /// 帧更新前回调
        /// </summary>
        public event Action<WorldNode, float> BeforeTickCallBack;

        /// <summary>
        /// 帧更新后回调
        /// </summary>
        public event Action<WorldNode, float> AfterTickCallBack;

        /// <summary>
        /// 删除前回调
        /// </summary>
        public event Action<WorldNode> RemoveCallBack;

        public void SetRoot(WorldNodeRoot root)
        {
            _root = root;
        }

        /// <summary>
        /// 设置父节点，如果传入null则会将层级设置为世界之下（顶层）
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(WorldNode parent)
        {
            if (_parent != null)
            {
                _parent._children.Remove(this);
                _parent = null;
            }

            if (parent == null)
            {
                _root.AddChild(this);
            }
            else
            {
                parent.AddChild(this);
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(WorldNode child)
        {
            if (child == null)
            {
                return;
            }

            if (child._parent != null)
            {
                child._parent._children.Remove(child);
            }
            else
            {
                _root._children.Remove(child);
            }

            child._parent = this;
            _children ??= new List<WorldNode>(10);
            _children.Add(child);
        }

        /// <summary>
        /// 删除子节点,会先递归删除该子节点的所有子节点，然后执行自身的删除
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(WorldNode child)
        {
            if (_children == null)
            {
                return;
            }

            if (!_children.Contains(child))
            {
                return;
            }

            RemoveCallBack?.Invoke(this);
            
            _nextTickRemoveChildren ??= new List<WorldNode>(10);
            _nextTickRemoveChildren.Add(child);
            
            if (child._children is not { Count: > 0 })
                return;
            
            foreach (var childChild in child._children)
            {
                child.RemoveChild(childChild);
            }
        }

        /// <summary>
        /// 从父代移除自己
        /// </summary>
        protected void RemoveSelfFromParent()
        {
            
        }
        
        private void reset()
        {
            _root = null;
            isFirstTick = true;
            FirstTickCallback = null;
            BeforeTickCallBack = null;
            AfterTickCallBack = null;
            RemoveCallBack = null;
        }

        /// <summary>
        /// 节点的tick，
        /// 当帧创建的node会在当前帧tick，
        /// 当帧被删除的node，会在当前帧所有node的tick执行完后被删除
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            BeforeTickCallBack?.Invoke(this, dt);

            if (isFirstTick)
            {
                FirstTickCallback?.Invoke(this);
                isFirstTick = false;
            }

            onTick(dt);

            if (_children is not { Count: > 0 })
                return;

            int i = 0;
            while (i < _children.Count)
            {
                WorldNode child = _children[i++];
                child.Tick(dt);
            }

            AfterTickCallBack?.Invoke(this, dt);
            
            if (_nextTickRemoveChildren is not { Count: > 0 })
                return;
            
            foreach (var removeChild in _nextTickRemoveChildren)
            {
                removeChild.onRemove();
                removeChild.reset();
                removeChild._parent = null;
                _children.Remove(removeChild);
            }
        }

        protected abstract void onTick(float dt);

        /// <summary>
        /// 被删除时调用
        /// </summary>
        protected abstract void onRemove();
    }
}