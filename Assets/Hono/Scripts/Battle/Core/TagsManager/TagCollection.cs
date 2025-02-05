#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Base
{
    /// <summary>
    ///     TAG 标识系统
    ///     目前一个最多支持256个tag
    /// </summary>
    public class TagCollection
    {
        private readonly HashSet<int> _tags = new(256);

        private readonly List<int> _tagsList = new(256);

        //关系映射
        private TagCollection _parent;
        private readonly List<TagCollection> _children = new(10);

        public void SetParent(TagCollection parent)
        {
            if (parent == null)
            {
                if (_parent == null)
                    return;
                _parent.RemoveChild(this);
            }

            _parent = parent;

            if (_parent != null && _parent._children.Contains(this))
            {
                _parent.AddChild(this);
            }
        }

        public void AddChild(TagCollection child)
        {
            if (_children.Contains(child))
            {
                Debug.LogError("子节点已存在！");
                return;
            }

            if (child._parent != this)
            {
                child.SetParent(this);
            }

            _children.Add(child);
        }

        public void RemoveChild(TagCollection child)
        {
            _children.RemoveSwapBack(child);
        }

        public void Add(int tag)
        {
            _tags.Add(tag);
            _tagsList.Add(tag);
        }

        public bool HasTag(int tag, ETagSearchRange searchRange)
        {
            return _tags.Contains(tag);
        }

        public void Remove(int tag)
        {
            _tags.Remove(tag);
            _tagsList.Remove(tag);
        }

        public List<int> GetAllTag()
        {
            return _tagsList;
        }

        public void Clear() { }
    }
}