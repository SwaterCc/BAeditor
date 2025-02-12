using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Hono.Scripts.Battle.Core
{
    public class WorldNodeRoot :WorldNode
    {
        protected override void OnRemove()
        {
            throw new NotImplementedException();
        }

        protected override void onTick(float dt)
        {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// 世界节点
    /// 世界节点存在逻辑上的父子关系管理
    /// 即子节点的生命周期依附于父节点，当父节点清除时，子节点会一起清除
    /// 清除顺序是子节点先执行清理然后轮到父节点
    /// </summary>
    public abstract class WorldNode
    {
        public delegate bool AddChildCondition(WorldNode arg);
        
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
        private List<WorldNode> _tickRemoveChildren;
        /// <summary>
        /// 待添加的子节点
        /// </summary>
        private List<(WorldNode, AddChildCondition)> _loadingActorCache = new();
        /// <summary>
        /// 第一次Tick
        /// </summary>
        private bool _isFirstTick = true;

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

        /// <summary>
        /// 设置Root节点
        /// </summary>
        /// <param name="root"></param>
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
        /// 当条件判定成功时添加子节点
        /// </summary>
        /// <param name="child"></param>
        /// <param name="condition"></param>
        public void AddChildWhenSuccess(WorldNode child, AddChildCondition condition)
        {
            if (child == null)
            {
                return;
            }

            _loadingActorCache.Add((child,condition));
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

            //将该节点加入待删除列表
            _tickRemoveChildren.Add(child);
        }

        /// <summary>
        /// 从父代移除自己
        /// </summary>
        /// <param name="disableCallback">禁用删除时回调</param>
        public void RemoveSelfFromParent(bool disableCallback = false)
        {
            //将该节点加入待删除列表
            _parent?._tickRemoveChildren.Add(this);
        }


        /// <summary>
        /// 从父代移除自己
        /// </summary>
        /// <param name="disableCallback">禁用删除时回调</param>
        private void removeSelfFromParent(bool disableCallback = false)
        {
            //如果有子节点先从最子层节点开始释放
            if (_children is { Count: > 0 })
            {
                foreach (var child in _children)
                {
                    child.removeSelfFromParent();
                }
            }

            //缓存节点清理
            foreach (var node in _loadingActorCache)
            {
                node.Item1.OnRemove();
                node.Item1.clear();
            }

            _loadingActorCache.Clear();

            //先调用删除回调
            if (!disableCallback)
            {
                RemoveCallBack?.Invoke(this);
            }

            //调用子类实现的OnRemove周期方法
            OnRemove();
            //父节点中移除自己
            _parent._children.RemoveSwapBack(this);
            //清空节点
            clear();
        }

        /// <summary>
        /// 被删除时调用
        /// </summary>
        protected abstract void OnRemove();

        private void clear()
        {
            _root = null;
            _parent = null;
            _isFirstTick = true;
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

            if (_isFirstTick)
            {
                FirstTickCallback?.Invoke(this);
                _isFirstTick = false;
            }

            onTick(dt);

            //检测缓存中可加入的子节点
            for (var index = 0; index < _loadingActorCache.Count; index++)
            {
                var tuple = _loadingActorCache[index];
                if (tuple.Item2.Invoke(tuple.Item1))
                {
                    _loadingActorCache.RemoveSwapBack(tuple);
                    index--;
                    AddChild(tuple.Item1);
                }
            }

            if (_children is not { Count: > 0 })
                return;

            int i = 0;
            while (i < _children.Count)
            {
                WorldNode child = _children[i++];
                child.Tick(dt);
            }

            AfterTickCallBack?.Invoke(this, dt);

            if (_tickRemoveChildren is not { Count: > 0 })
                return;

            foreach (var removeChild in _tickRemoveChildren)
            {
                removeChild.removeSelfFromParent();
            }
        }

        protected abstract void onTick(float dt);
    }
}