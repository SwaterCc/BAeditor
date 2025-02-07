#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
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
    public class EventManager : Singleton<EventManager>, IWorldSystemWhenTickCalled, IWorldSystemWhenExitCalled
    {
        /// <summary>
        /// actor绑定注册列表
        /// </summary>
        private readonly Dictionary<int, UnitEventListenerCollection> _unitEventListeners = new();
        private readonly EventListenerCollection _worldEventListeners = new(20);

        public void OnWorldTick(float dt)
        {
            _worldEventListeners.Tick(dt);
        }

        public void OnWorldExit()
        {
            _worldEventListeners.Clear();
            foreach (var listeners in _unitEventListeners.Values)
            {
                listeners.Clear();
            }

            _unitEventListeners.Clear();
        }

        /// <summary>
        /// 添加事件容器
        /// </summary>
        /// <param name="collection"></param>
        public void AddListenerCollection(UnitEventListenerCollection collection)
        {
            _unitEventListeners.TryAdd(collection.Unit.Uid, collection);
        }

        /// <summary>
        /// 删除事件容器
        /// </summary>
        /// <param name="collection"></param>
        public void RemoveListenerCollection(UnitEventListenerCollection collection)
        {
            _unitEventListeners.Remove(collection.Unit.Uid);
        }


        /// <summary>
        /// 注册全局事件监听
        /// </summary>
        /// <param name="listener"></param>
        /// <exception cref="Exception"></exception>
        public void RegisterWorldListener(WorldEventListener listener)
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

        public void UnregisterWorldListener(WorldEventListener listener)
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
        /// 发送全局事件，会触发所有WorldListener和ActorListener中GlobalListener被设置为true的监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="unitUid">发送者的Uid</param>
        /// <param name="board">事件信息</param>
        public void FireWorldEvent(EEventType eventType, int unitUid = -1, VariableBoard board = null)
        {
            if (unitUid > 0 && board != null)
            {
                board.Set("FireEventUnitUid", unitUid);
            }

            FireWorldEvent(eventType, board);
        }
        
        /// <summary>
        /// 发送全局事件，会触发所有WorldListener和ActorListener中GlobalListener被设置为true的监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="board">事件信息</param>
        public void FireWorldEvent(EEventType eventType, VariableBoard board = null)
        {
            //Actor事件，仅通知给对应的Actor
            foreach (var unitListener in _unitEventListeners.Values)
            {
                unitListener.FireWorldEvent(eventType, board);
            }

            //然后通知世界监听者
            _worldEventListeners.FireEvent(eventType, board);
        }
    }
}