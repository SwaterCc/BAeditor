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
        private readonly List<bool> _tags = new(256);

        private IEnableTagSearch _bind;

        public Tags() {
	        for (int i = 0; i < _tags.Capacity; i++) {
		        _tags.Add(false);
	        }
        }

        public Tags(int[] tag)
        {
	        for (int i = 0; i < _tags.Capacity; i++) {
		        _tags.Add(false);
	        }
            foreach (var i in tag)
            {
                _tags[i] = true;
            }
        }
        
        public Tags(IEnableTagSearch bind)
        {
            _bind = bind;
        }

        public void Add(int tag)
        {
            if (tag is <= 255 and >= 0)
            {
                _tags[tag] = true;
            }
        }

        public bool HasTag(int tag)
        {
            return tag is <= 255 and >= 0 && _tags[tag];
        }

        public void Show() {
	        foreach (var tag in _tags) {
		        Debug.Log($"{(bool)tag}");
	        }
        }
        
        public void Remove(int tag)
        {
            if (tag is <= 255 and >= 0)
            {
                _tags[tag] = false;
            }
        }

        public List<int> GetAllTag() {
	        var res = new List<int>();
	        for (int index = 0; index < _tags.Count; index++) {
		        bool tag = _tags[index];
		        if (tag) {
			        res.Add(index);
		        }
	        }

	        return res;
        }
    }
}