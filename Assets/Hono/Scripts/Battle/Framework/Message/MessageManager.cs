#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 消息管理
    /// 对于消息的定义 消息的发送者和接收者是一对一的关系，消息会在一定时间内缓存消息，是有序的
    /// 消息key值在Actor域内唯一
    /// </summary>
    public class MessageManager : Singleton<MessageManager>, IBattleFrameworkEnterExit
    {
        private readonly Dictionary<int, MessageCollection> _collections = new(128);
        private readonly Dictionary<int, Dictionary<string, MessageCacheQueue>> _messageCaches = new(128);
        private const float MsgCacheClearTime = 3f;
        private float _clearTimeDuration = 0;
        public void OnEnterBattle() { }

        public void OnExitBattle()
        {
            _collections.Clear();
            _messageCaches.Clear();
        }

        public void AddMsgCollection(MessageCollection collection)
        {
            if (_collections.TryAdd(collection.Actor.Uid, collection))
            {
                Debug.LogError($"重复添加MessageCollection Uid:{collection.Actor.Uid}");
            }
        }

        public void RemoveMsgCollection(MessageCollection collection)
        {
            _collections.Remove(collection.Actor.Uid);
        }

        public void AddMsg(int uid, string msgKey, VariableBoard board)
        {
            if (!_collections.TryGetValue(uid, out var collection))
            {
                //collection不存在，缓存消息
                cacheMsg(uid, msgKey, board);
                return;
            }

            if (!collection.ContainsKey(msgKey))
            {
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
                cacheQueue = APool<MessageCacheQueue>.Pool.Rent();
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
            APool<MessageCacheQueue>.Pool.Recycle(cacheQueue);
            _messageCaches[uid].Remove(msgKey);
        }
    }
}