#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Message;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     msg要能存储，要能当帧执行
    /// </summary>
    public class MessageCollection
    {
        public Actor Actor { get; }

        private readonly Dictionary<string, MessageListener> _messageListeners;


        public MessageCollection(Actor actor, int capacity)
        {
            Actor = actor;
            _messageListeners = new Dictionary<string, MessageListener>(capacity);
        }

        public bool ContainsKey(string key)
        {
            return _messageListeners.ContainsKey(key);
        }

        public void AddListener(MessageListener listener)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(listener.MsgKey))
            {
                Debug.LogError("AddListener listener key值不可以为空");
                return;
            }

            if (listener.BindActorUid <= 0)
            {
                Debug.LogError("AddListener listener 未绑定Actor");
                return;
            }
#endif

            if (!_messageListeners.TryAdd(listener.MsgKey, listener))
            {
                Debug.LogError($"AddListener MessageListener key值 {listener.MsgKey} 重复");
                return;
            }

            //查找缓存是否有未接收的消息
            if (MessageManager.Instance.TryGetMsgCatchQueue(Actor.Uid, listener.MsgKey, out var cacheQueue))
            {
                while (cacheQueue.IsEmpty())
                {
                    listener.Invoke(cacheQueue.Pop());
                }

                MessageManager.Instance.RemoveMsgCatchQueue(Actor.Uid, listener.MsgKey);
            }
        }

        public void RemoveListener(MessageListener listener)
        {
            _messageListeners.Remove(listener.MsgKey);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage(string key, VariableBoard board)
        {
            if (_messageListeners.TryGetValue(key, out var listener))
            {
                listener.Invoke(board);
            }
        }

        public void Tick(float dt)
        {
            if (MessageManager.Instance.TryGetMsgCatchQueues(Actor.Uid, out var messageCacheQueues))
            {
                foreach (var msgQueue in messageCacheQueues.Values)
                {
                    msgQueue.Tick(dt);
                }
            }
        }

        public void Clear()
        {
            _messageListeners.Clear();
        }
    }
}