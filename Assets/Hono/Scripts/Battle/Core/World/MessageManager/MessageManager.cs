#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 消息管理
    /// 对于消息的定义 消息的发送者和接收者是一对一的关系，消息会在一定时间内缓存消息，是有序的
    /// 消息key值在Actor域内唯一
    /// </summary>
    public class MessageManager : Singleton<MessageManager>, IWorldSystemWhenExitCalled
    {
        private readonly Dictionary<int, MessageCollection> _collections = new(128);
        private readonly Dictionary<int, Dictionary<string, MessageCacheQueue>> _messageCaches = new(128);

        public void OnWorldExit()
        {
            _collections.Clear();
            _messageCaches.Clear();
        }

        public void AddMsgCollection(MessageCollection collection)
        {
            if (_collections.TryAdd(collection.Unit.Uid, collection))
            {
                Debug.LogError($"重复添加MessageCollection Uid:{collection.Unit.Uid}");
            }
        }

        public void RemoveMsgCollection(MessageCollection collection)
        {
            _collections.Remove(collection.Unit.Uid);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="uid">发给目标Actor的uid</param>
        /// <param name="msgKey">msg键值</param>
        /// <param name="board"></param>
        public void SendMessage(in int uid, string msgKey, VariableBoard board)
        {
            if (!_collections.TryGetValue(uid, out var collection))
            {
                //collection不存在，缓存消息
                cacheMsg(uid, msgKey, board);
                return;
            }

            if (!collection.ContainsKey(msgKey))
            {
                //key值不存在先缓存
                cacheMsg(uid, msgKey, board);
                return;
            }

            collection.SendMessage(msgKey, board);
        }

        private void cacheMsg(int uid, string msgKey, VariableBoard board)
        {
            if (!_messageCaches.TryGetValue(uid, out var queues))
            {
                queues = new Dictionary<string, MessageCacheQueue>();
                _messageCaches.Add(uid, queues);
            }

            if (!queues.TryGetValue(msgKey, out var cacheQueue))
            {
                cacheQueue = GPool<MessageCacheQueue>.Pool.Rent();
                queues.Add(msgKey, cacheQueue);
            }

            cacheQueue.Push(board);
        }

        public bool TryGetMsgCatchQueue(int uid, string msgKey, out MessageCacheQueue cacheQueue)
        {
            cacheQueue = null;

            if (!_messageCaches.TryGetValue(uid, out var queues))
            {
                return false;
            }

            return queues.TryGetValue(msgKey, out cacheQueue);
        }

        public bool TryGetMsgCatchQueues(int uid, out Dictionary<string, MessageCacheQueue> cacheQueues)
        {
            return _messageCaches.TryGetValue(uid, out cacheQueues);
        }

        public void RemoveMsgCatchQueue(int uid, string msgKey)
        {
            var cacheQueue = _messageCaches[uid][msgKey];

            //缓存队列回收
            GPool<MessageCacheQueue>.Pool.Recycle(cacheQueue);
            _messageCaches[uid].Remove(msgKey);
        }
    }
}