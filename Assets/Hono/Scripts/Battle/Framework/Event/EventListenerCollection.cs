using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core;
using UnityEngine;

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 事件容器，
    /// </summary>
    public class EventListenerCollection
    {
        private readonly Dictionary<EEventType, List<EventListener>> _eventListeners;
        private readonly int _listenerListCapacity;

        public EventListenerCollection(int listenerListCapacity = 8)
        {
            _eventListeners = new Dictionary<EEventType, List<EventListener>>(10);
            _listenerListCapacity = listenerListCapacity;
        }

        public void AddListener(EventListener listener)
        {
            if (!_eventListeners.TryGetValue(listener.EventType, out var list))
            {
                list = new List<EventListener>(_listenerListCapacity);
                _eventListeners.Add(listener.EventType, list);
            }
            else
            {
                if (list.Contains(listener))
                {
                    Debug.LogError("重复添加相同的listener");
                    return;
                }
            }

            list.Add(listener);
        }

        public void FireEvent(EEventType eventType, VariableBoard board = null)
        {
            if (!_eventListeners.TryGetValue(eventType, out var listeners))
                return;

            foreach (var listener in listeners)
            {
                listener.OnEventFired(board);
            }
        }

        public void RemoveListener(EventListener listener)
        {
            if (_eventListeners.TryGetValue(listener.EventType, out var list))
            {
                list.Remove(listener);
            }
        }

        public bool Contains(EventListener listener)
        {
            return _eventListeners.TryGetValue(listener.EventType, out var listeners) && listeners.Contains(listener);
        }

        public void Tick(float dt)
        {
            foreach (var listeners in _eventListeners.Values)
            {
                foreach (var listener in listeners)
                {
                    listener.Tick(dt);
                }
            }
        }

        public void Clear()
        {
            foreach (var listeners in _eventListeners.Values)
            {
                listeners.Clear();
            }
        }
    }

    public class UnitEventListenerCollection : EventListenerCollection
    {
        public Unit Unit { get; }

        public UnitEventListenerCollection(Unit unit, int listenerListCapacity = 8) : base(listenerListCapacity)
        {
            Unit = unit;
        }
    }
}