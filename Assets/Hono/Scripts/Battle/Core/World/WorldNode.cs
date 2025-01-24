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
            child.onRemove();
            child.reset();
            child._parent = null;
            _children.Remove(child);

            if (child._children is not { Count: > 0 })
                return;
            foreach (var childChild in child._children)
            {
                child.RemoveChild(childChild);
            }
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
            
            foreach (var child in _children)
            {
                child.Tick(dt);
            }

            AfterTickCallBack?.Invoke(this, dt);
        }

        protected abstract void onTick(float dt);
        
        /// <summary>
        /// 被删除时调用
        /// </summary>
        protected abstract void onRemove();
    }
}