#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Event {
	/// <summary>
	/// 战斗逻辑的事件管理
	/// <para>
	/// 对于事件的定义
	/// 事件是一种即使的通知，触发者和接收者是一对多的关系
	/// </para>
	/// </summary>
	public class EventManager : Singleton<EventManager>, IWorldSystemWhenTickCalled, IWorldSystemWhenExitCalled {
		/// <summary>
		/// actor绑定注册列表
		/// </summary>
		private readonly Dictionary<int, EventListenerCollection> _evtListenerCollections = new();
		private readonly Dictionary<EEventType, List<EventListener>> _worldEventListeners = new(20);

		public void OnWorldTick(float dt) {
			foreach (var eventListners in _worldEventListeners.Values) {
				foreach (var worldListner in eventListners) {
					worldListner.Tick(dt);
				}
			}
		}

		public void OnWorldExit() {
			_worldEventListeners.Clear();
			foreach (var listeners in _evtListenerCollections.Values) {
				listeners.Clear();
			}

			_evtListenerCollections.Clear();
		}

		/// <summary>
		/// 添加事件容器
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="collection"></param>
		public void AddCollection(int uid, EventListenerCollection collection) {
			_evtListenerCollections.TryAdd(uid, collection);
		}

		/// <summary>
		/// 删除事件容器
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="collection"></param>
		public void RemoveListenerCollection(int uid, EventListenerCollection collection) {
			_evtListenerCollections.Remove(uid);
		}

		/// <summary>
		/// 注册全局事件监听
		/// </summary>
		/// <param name="listener"></param>
		/// <exception cref="Exception"></exception>
		public void AddWorldListener(EventListener listener) {
#if UNITY_EDITOR
			if (listener == null) {
				throw new Exception("listener is null");
			}

			if (listener.EventType == EEventType.NoInit) {
				Debug.LogError("listener 未绑定Event！");
				return;
			}
#endif
			//加入对象监听列表
			if (!_worldEventListeners.TryGetValue(listener.EventType, out var list)) {
				list = new List<EventListener>(10);
				_worldEventListeners.Add(listener.EventType, list);
			}
		}

		public void RemoveWorldListener(EventListener listener) {
#if UNITY_EDITOR
			if (listener == null) {
				throw new Exception("listener is null");
			}

			if (listener.EventType == EEventType.NoInit) {
				Debug.LogError("listener 未绑定Event！");
				return;
			}
#endif
			if (_worldEventListeners.TryGetValue(listener.EventType, out var list)) {
				list.Remove(listener);
			}
		}

		/// <summary>
		/// 发送全局事件，会触发所有WorldListener和ActorListener中GlobalListener被设置为true的监听
		/// </summary>
		/// <param name="eventType">事件类型</param>
		/// <param name="board">事件信息</param>
		public void FireWorldEvent(EEventType eventType, VariableBoard board = null) {
			//然后通知世界监听者
			foreach (var eventList in _worldEventListeners.Values) {
				foreach (var listener in eventList) {
					listener.OnEventFired(board);
				}
			}
			//Actor事件，仅通知给对应的Actor
			foreach (var unitListener in _evtListenerCollections.Values) {
				unitListener.FireWorldEvent(eventType, board);
			}
		}
	}
}