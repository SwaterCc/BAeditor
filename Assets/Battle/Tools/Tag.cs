using System.Collections;

namespace Battle.Tools
{
    /// <summary>
    /// 标签检索接口
    /// </summary>
    public interface IEnableTagSearch { }

    /// <summary>
    /// TAG 标识系统
    /// 目前一个最多支持256个tag
    /// </summary>
    public class Tag
    {
        private readonly BitArray _tags = new(256);

        private IEnableTagSearch _bind;

        public Tag() { }

        public Tag(int[] tag)
        {
            foreach (var i in tag)
            {
                _tags[i] = true;
            }
        }
        
        public Tag(IEnableTagSearch bind)
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


        public void Remove(int tag)
        {
            if (tag is <= 255 and >= 0)
            {
                _tags[tag] = false;
            }
        }
    }
}