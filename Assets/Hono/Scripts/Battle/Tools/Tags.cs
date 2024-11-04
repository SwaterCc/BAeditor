#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Tools
{
	/// <summary>
	///     标签检索接口
	/// </summary>
	public interface IEnableTagSearch { }

	/// <summary>
	///     TAG 标识系统
	///     目前一个最多支持256个tag
	/// </summary>
	public class Tags
    {
        private readonly HashSet<int> _tags = new(256);
        private readonly List<int> _tagsList = new(256);
        private IEnableTagSearch _bind;

        public Tags() { }

        public Tags(int[] tags)
        {
            foreach (var tag in tags)
            {
                Add(tag);
            }
        }

        public Tags(IEnableTagSearch bind)
        {
            _bind = bind;
        }

        public void Add(int tag)
        {
            _tags.Add(tag);
            _tagsList.Add(tag);
        }

        public bool HasTag(int tag)
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
    }
}