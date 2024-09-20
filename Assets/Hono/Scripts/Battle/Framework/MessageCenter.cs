using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class MessageListener
    {
        public string MsgKey { get; }

        private readonly Action<object, object, object, object, object> _callback;

        public MessageListener(string msgKey, Action<object, object, object, object, object> callback)
        {
            MsgKey = msgKey;
            _callback = callback;
        }

        public void Invoke(object p1, object p2, object p3, object p4, object p5)
        {
            _callback.Invoke(p1, p2, p3, p4, p5);
        }
    }

    /// <summary>
    /// msg要能存储，要能当帧执行
    /// </summary>
    public class MessageCollection
    {
        private readonly Dictionary<string, List<MessageListener>> _msgHandlers;

        public int Uid { get; }

        public MessageCollection(Actor actor)
        {
            Uid = actor.Uid;
            _msgHandlers = new Dictionary<string, List<MessageListener>>();
        }

        public void Init()
        {
            MessageCenter.Instance.Register(Uid, this);
        }

        /// <summary>
        /// 直接调用msg，该消息将不会进入缓存
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="p5"></param>
        /// <returns></returns>
        public bool SendMsg(string msg, object p1, object p2, object p3, object p4, object p5)
        {
            if (_msgHandlers.TryGetValue(msg, out var listeners))
            {
                if (listeners.Count > 0)
                {
                    foreach (var listener in listeners)
                    {
                        listener.Invoke(p1, p2, p3, p4, p5);
                    }

                    return true;
                }
            }

            return false;
        }

        public bool SendMsgCheck(string msgKey)
        {
            if (!_msgHandlers.TryGetValue(msgKey, out var listeners)) return false;
            return listeners.Count > 0;
        }
        
        public void AddListener(MessageListener listener)
        {
            if (!_msgHandlers.TryGetValue(listener.MsgKey, out var handlers))
            {
                handlers = new List<MessageListener>();
                _msgHandlers.Add(listener.MsgKey, handlers);
            }

            handlers.Add(listener);
        }

        public void RemoveListener(MessageListener listener)
        {
            if (_msgHandlers.TryGetValue(listener.MsgKey, out var handlers))
            {
                handlers.Remove(listener);
            }
        }

        public void UnInit()
        {
            MessageCenter.Instance.Unregister(Uid);
        }
    }

    public class MsgCache
    {
        public string MsgKey;
        public object P1;
        public object P2;
        public object P3;
        public object P4;
        public object P5;
    }

    public class MessageCenter : Singleton<MessageCenter>
    {
        private readonly Dictionary<int, MessageCollection> _collections;
        private readonly Dictionary<int, List<MsgCache>> _msgCaches;

        private readonly float _msgCacheClearTime = 3f;
        private float _clearTimeDuration;

        public MessageCenter()
        {
            _collections = new Dictionary<int, MessageCollection>();
            _msgCaches = new Dictionary<int, List<MsgCache>>();
            _clearTimeDuration = 0;
        }

        public void Register(int uid, MessageCollection collection)
        {
            if (!_collections.TryAdd(uid, collection))
            {
                Debug.LogError($"重复添加MessageCollection Uid:{uid}");
            }
        }

        public void Unregister(int uid)
        {
            if (_collections.ContainsKey(uid))
            {
                _collections.Remove(uid);
            }
        }

        public void AddMsg(int uid, MsgCache msgCache)
        {
            if (!_msgCaches.TryGetValue(uid, out var caches))
            {
                caches = new List<MsgCache>();
                _msgCaches.Add(uid, caches);
            }

            caches.Add(msgCache);
        }
        
        
        public void Tick(float dt)
        {
            _clearTimeDuration += dt;

            foreach (var pair in _msgCaches)
            {
                if (_collections.TryGetValue(pair.Key, out var collection))
                {
                    List<MsgCache> removes = new List<MsgCache>();
                    foreach (var msgCache in pair.Value)
                    {
                        if (collection.SendMsg(msgCache.MsgKey, msgCache.P1, msgCache.P2, msgCache.P3, msgCache.P4,
                                msgCache.P5))
                        {
                            removes.Add(msgCache);
                        }
                    }

                    foreach (var removeItem in removes)
                    {
                        pair.Value.Remove(removeItem);
                    }
                }
            }
            
            if (_clearTimeDuration > _msgCacheClearTime)
            {
                _clearTimeDuration = 0;
                _msgCaches.Clear();
            }
        }
    }
}