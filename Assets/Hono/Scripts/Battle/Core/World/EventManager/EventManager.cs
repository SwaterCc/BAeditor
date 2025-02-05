#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 战斗逻辑的事件管理
    /// <para>
    /// 对于事件的定义
    /// 事件是一种即使的通知，触发者和接收者是一对多的关系
    /// </para>
    /// </summary>
    public class EventManager : World.WorldSingleton<EventManager>, IWorldSystem
    {
        /// <summary>
        /// actor绑定注册列表
        /// </summary>
        private readonly Dictionary<int, UnitEventListenerCollection> _actorEventListeners = new();
        private readonly EventListenerCollection _worldEventListeners = new(20);

        public void EnterWorld() { }

        public void Tick(float dt)
        {
            _worldEventListeners.Tick(dt);
        }

        public void ExitWorld()
        {
            _worldEventListeners.Clear();
            foreach (var listeners in _actorEventListeners.Values)
            {
                listeners.Clear();
            }
            _actorEventListeners.Clear();
        }


        /// <summary>
        /// 添加事件容器
        /// </summary>
        /// <param name="collection"></param>
        public void AddListenerCollection(UnitEventListenerCollection collection)
        {
            _actorEventListeners.TryAdd(collection.Unit.Uid, collection);
        }

        /// <summary>
        /// 删除事件容器
        /// </summary>
        /// <param name="collection"></param>
        public void RemoveListenerCollection(UnitEventListenerCollection collection)
        {
            _actorEventListeners.Remove(collection.Unit.Uid);
        }


        /// <summary>
        /// 注册全局事件监听
        /// </summary>
        /// <param name="listener"></param>
        /// <exception cref="Exception"></exception>
        public void RegisterGlobalListener(GlobalEventListener listener)
        {
#if UNITY_EDITOR
            if (listener == null)
            {
                throw new Exception("listener is null");
            }

            if (_worldEventListeners.Contains(listener))
            {
                Debug.LogWarning("重复注册相同的listener");
            }

            if (listener.EventType == EEventType.NoInit)
            {
                Debug.LogError("listener 未绑定Event！");
                return;
            }
#endif
            //加入对象监听列表
            _worldEventListeners.AddListener(listener);
        }

        public void UnregisterGlobalListener(GlobalEventListener listener)
        {
#if UNITY_EDITOR
            if (listener == null)
            {
                throw new Exception("listener is null");
            }

            if (listener.EventType == EEventType.NoInit)
            {
                Debug.LogError("listener 未绑定Event！");
                return;
            }
#endif
            _worldEventListeners.RemoveListener(listener);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="actorUid">如果小于0则该事件为全局事件，会通知所有全局监听，大于0则会通知对于Actor内的监听</param>
        /// <param name="board">事件信息</param>
        public void FireEvent(EEventType eventType,
            int actorUid = -1,
            VariableBoard board = null)
        {
            bool isGlobalEvent = actorUid > 0;

            if (isGlobalEvent)
            {
                //全局事件 通知actor Listener中监听全局事件的listener
                foreach (var collection in _actorEventListeners.Values)
                {
                    collection.FireEvent(eventType, board);
                }

                //然后通知世界监听者
                _worldEventListeners.FireEvent(eventType, board);
            }
            else
            {
                //Actor事件，仅通知给对应的Actor
                if (_actorEventListeners.TryGetValue(actorUid, out var collection))
                {
                    collection.FireEvent(eventType, board);
                }
            }
        }
    }
}