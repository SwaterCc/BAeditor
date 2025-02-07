#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Base
{
    /// <summary>
    /// TAG 标识系统
    /// </summary>
    public class TagCollection
    {
        private readonly Dictionary<int,int> _tags = new(256);

        public bool HasTag(int tag)
        {
            return _tags.GetValueOrDefault(tag, 0) > 0;
        }

        public void Add(int tag)
        {
            _tags[tag] = _tags.GetValueOrDefault(tag, 0) + 1;
        }
        
        public void Remove(int tag)
        {
            if (!_tags.ContainsKey(tag))
            {
                return;
            }

            _tags[tag] -= 1;
            _tags[tag] = Mathf.Min(0, _tags[tag]);
        }

        /// <summary>
        /// 产生快照
        /// </summary>
        /// <param name="tags"></param>
        public void GetSnapshot(ref HashSet<int> tags)
        {
            foreach (var pair in _tags)
            {
                if (pair.Value > 0)
                {
                    tags.Add(pair.Key);
                }
            }   
        }
        
        public void Clear()
        {
            _tags.Clear();
        }
    }
}