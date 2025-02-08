using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{
    public class IdAllocator
    {
        private readonly Stack<int> _availableIds = new(100); // 存储可回收的 ID
        private int _nextId = 1;                              // 下一个可用的 ID

        /// <summary>
        /// 分配一个新的 ID
        /// </summary>
        public int Allocate()
        {
            lock (_availableIds)
            {
                if (_availableIds.Count > 0)
                {
                    return _availableIds.Pop(); // 优先使用回收的 ID
                }

                return _nextId++; // 如果没有可回收的 ID，则生成新的 ID
            }
        }

        /// <summary>
        /// 回收指定的 ID
        /// </summary>
        public void Recycle(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid UID");

            lock (_availableIds)
            {
                _availableIds.Push(id); // 将 ID 回收到池中
            }
        }

        /// <summary>
        /// 获取当前已分配的最大 ID
        /// </summary>
        private int GetMaxAllocatedId()
        {
            lock (_availableIds)
            {
                return _nextId - 1; // 当前最大 ID 是 _nextId - 1
            }
        }

        /// <summary>
        /// 清理所有资源
        /// </summary>
        public void Clear()
        {
            lock (_availableIds)
            {
                _availableIds.Clear();
                _nextId = 1; // 重置分配器
            }
        }
    }
}