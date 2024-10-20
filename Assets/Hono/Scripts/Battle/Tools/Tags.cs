using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    /// <summary>
    /// 标签检索接口
    /// </summary>
    public interface IEnableTagSearch { }

    /// <summary>
    /// TAG 标识系统
    /// 目前一个最多支持256个tag
    /// </summary>
    public class Tags
    {
        private readonly HashSet<int> _tags = new(256);

        private IEnableTagSearch _bind;

        public Tags() { }

        public Tags(int[] tag)
        {
            foreach (var i in tag)
            {
                _tags.Add(i);
            }
        }

        public Tags(IEnableTagSearch bind)
        {
            _bind = bind;
        }

        public void Add(int tag)
        {
            _tags.Add(tag);
        }

        public bool HasTag(int tag)
        {
            return _tags.Contains(tag);
        }

        public void Remove(int tag)
        {
            _tags.Remove(tag);
        }

        public void Clear()
        {
            _tags.Clear();
        }
        
        public List<int> GetAllTag()
        {
            var res = new List<int>();
            foreach (var tag in _tags)
            {
                res.Add(tag);
            }

            return res;
        }
    }
}