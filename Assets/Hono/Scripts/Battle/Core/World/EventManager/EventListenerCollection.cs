using Hono.Scripts.Battle.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Event {
	using EventListenerLookUp = Dictionary<EEventType, GList<EventListener>>;

	/// <summary>
	/// 事件容器，
	/// </summary>
	public class EventListenerCollection {
		private readonly EventListenerLookUp _unitEvtListenerLookup;
		private readonly EventListenerLookUp _worldEvtListenerLookup;

		public EventListenerCollection(int listenerListCapacity = 8) {
			_unitEvtListenerLookup = new EventListenerLookUp(listenerListCapacity);
			_worldEvtListenerLookup = new EventListenerLookUp(listenerListCapacity);
		}

		public void AddListener(EventListener listener, bool isWorldListener) {
			EventListenerLookUp lookup = isWorldListener ? _worldEvtListenerLookup : _unitEvtListenerLookup;
			if (!lookup.TryGetValue(listener.EventType, out var list)) {
				list = GPool<GList<EventListener>>.Pool.Rent();
				lookup.Add(listener.EventType, list);
			}
			else {
				if (list.Contains(listener)) {
					Debug.LogError("重复添加相同的listener");
					return;
				}
			}

			list.Add(listener);
		}

		public void RemoveListener(EventListener listener) {
			if (_worldEvtListenerLookup.TryGetValue(listener.EventType, out var list1)) {
				if (list1.Remove(listener)) {
					return;
				}
			}

			if (_unitEvtListenerLookup.TryGetValue(listener.EventType, out var list2)) {
				if (list2.Remove(listener)) {
					return;
				}
			}

			Debug.LogError("找不到对应的Listener");
		}

		/// <summary>
		/// 触发所有EventListener
		/// </summary>
		/// <param name="eventType"></param>
		/// <param name="board"></param>
		public void FireEvent(EEventType eventType, VariableBoard board = null) {
			if (_worldEvtListenerLookup.TryGetValue(eventType, out var worldListeners)) {
				foreach (var listener in worldListeners) {
					listener.OnEventFired(board);
				}
			}

			if (_unitEvtListenerLookup.TryGetValue(eventType, out var unitListeners)) {
				foreach (var listener in unitListeners) {
					listener.OnEventFired(board);
				}
			}
		}

		/// <summary>
		/// 仅触发监听世界级事件的Event
		/// </summary>
		/// <param name="eventType"></param>
		/// <param name="board"></param>
		public void FireWorldEvent(EEventType eventType, VariableBoard board = null) {
			if (_worldEvtListenerLookup.TryGetValue(eventType, out var worldListeners)) {
				foreach (var listener in worldListeners) {
					listener.OnEventFired(board);
				}
			}
		}

		public void Tick(float dt) {
			foreach (var listeners in _unitEvtListenerLookup.Values) {
				foreach (var listener in listeners) {
					listener.Tick(dt);
				}
			}

			foreach (var listeners in _worldEvtListenerLookup.Values) {
				foreach (var listener in listeners) {
					listener.Tick(dt);
				}
			}
		}

		public void Clear() {
			foreach (var listeners in _worldEvtListenerLookup.Values) {
				GPool<GList<EventListener>>.Pool.Recycle(listeners);
			}

			_worldEvtListenerLookup.Clear();
			foreach (var listeners in _unitEvtListenerLookup.Values) {
				GPool<GList<EventListener>>.Pool.Recycle(listeners);
			}

			_unitEvtListenerLookup.Clear();
		}
	}
}