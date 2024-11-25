#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Base
{
    public class VarCollection
    {
        private readonly Dictionary<string, object> _collection;

        private VarCollection _parent;
        private readonly List<VarCollection> _children;

        public VarCollection(int capacity)
        {
            _collection = new Dictionary<string, object>(capacity);
            _children = new List<VarCollection>(20);
        }

        public void SetParent(VarCollection parent)
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

        public void AddChild(VarCollection child)
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

        public void RemoveChild(VarCollection child)
        {
            _children.RemoveSwapBack(child);
        }

        public object Get(string name)
        {
            return _collection.GetValueOrDefault(name);
        }

        public T Get<T>(in string key) where T : class
        {
            throw new NotImplementedException();
        }

        public void Set<T>(in string key, in T value) where T : class
        {
            throw new NotImplementedException();
        }

        public T Change<T>(in string key, in T changeValue) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete(string name)
        {
            if (_collection.ContainsKey(name))
            {
                _collection.Remove(name);
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}