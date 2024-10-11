using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class MsgCache
    {
        public string MsgKey;
        public object P1;
        public object P2;
        public object P3;
        public object P4;
        public object P5;
    }

    public class MessageCenter : Singleton<MessageCenter>, IBattleFrameworkEnterExit, IBattleFrameworkTick
    {
        private readonly Dictionary<int, MessageCollection> _collections = new(128);
        private readonly Dictionary<int, List<MsgCache>> _msgCaches = new(128);

        private const float MsgCacheClearTime = 3f;
        private float _clearTimeDuration = 0;

        public void OnEnterBattle()
        {
            _msgCaches.Clear();
            _collections.Clear();
        }

        public void OnExitBattle()
        {
            _msgCaches.Clear();
            _collections.Clear();
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

            if (_clearTimeDuration > MsgCacheClearTime)
            {
                _clearTimeDuration = 0;
                _msgCaches.Clear();
            }
        }
    }
}