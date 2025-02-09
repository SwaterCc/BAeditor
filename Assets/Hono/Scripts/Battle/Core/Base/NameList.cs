using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core.Base
{
    public class NameList<T>
    {
        // 使用字典存储 id 和对应的计数
        private readonly Dictionary<T, int> _idCountMap;

        public NameList(int capacity = 8)
        {
            _idCountMap = new Dictionary<T, int>(capacity);
        }
        
        // 添加 id，如果存在则增加计数，不存在则初始化为1
        public void Add(T id)
        {
            if (_idCountMap.ContainsKey(id))
            {
                _idCountMap[id]++;
            }
            else
            {
                _idCountMap[id] = 1;
            }
        }

        // 删除 id，如果存在则减少计数，计数为0时移除该id
        public void Remove(T id)
        {
            if (_idCountMap.ContainsKey(id))
            {
                _idCountMap[id]--;
                if (_idCountMap[id] <= 0)
                {
                    _idCountMap.Remove(id);
                }
            }
        }

        // 查询某个 id 是否存在，次数为0说明不存在
        public bool Contains(T id)
        {
            return _idCountMap.ContainsKey(id) && _idCountMap[id] > 0;
        }

        // 获取某个 id 的计数，如果不存在返回0
        public int GetCount(T id)
        {
            if (_idCountMap.TryGetValue(id, out var count))
            {
                return count;
            }
            return 0;
        }

        public void Clear()
        {
            _idCountMap.Clear();
        }
    }
}