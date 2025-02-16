using System.Collections.Generic;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle
{
    public class MessageCacheQueue : IGPoolObject
    {
        /// <summary>
        /// 消息最大存留时长
        /// </summary>
        private const float MessageCacheClearTime = 3f;

        /// <summary>
        /// 清理间隔累计
        /// </summary>
        private float _cacheClearInterval;

        /// <summary>
        /// 缓存队列
        /// </summary>
        private readonly Queue<VariableBoard> _catchQueue = new(10);

        /// <summary>
        /// 入队缓存 VariableBoard引用计数会+1
        /// </summary>
        public void Push(VariableBoard board)
        {
            board.RefCount.AddReference();
            _catchQueue.Enqueue(board);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _catchQueue.Count == 0;
        }
        
        /// <summary>
        /// 出队缓存 VariableBoard引用计数会-1
        /// </summary>
        public VariableBoard Pop()
        {
            if (_catchQueue.TryDequeue(out var board))
            {
                GPool<VariableBoard>.Pool.Recycle(board);
            }

            return board;
        }

        public void Tick(float dt)
        {
            if(_catchQueue.Count ==0)
                return;
            _cacheClearInterval += dt;
            if (_cacheClearInterval > MessageCacheClearTime)
            {
                _cacheClearInterval = 0;
                _catchQueue.Clear();
            }
        }
        
        public void OnRecycle()
        {
            foreach (var board in _catchQueue)
            {
                GPool<VariableBoard>.Pool.Recycle(board);
            }
            _catchQueue.Clear();
        }
    }
}