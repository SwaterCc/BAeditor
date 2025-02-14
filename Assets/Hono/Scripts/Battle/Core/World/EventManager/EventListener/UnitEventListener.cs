using System;

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// Actor对象内部使用的EventListener，该Listener会在Actor被回收时失效
    /// </summary>
    public class UnitEventListener : EventListener
    {
        public UnitEventListener() { }
        public UnitEventListener(EEventType eventType, bool isWorldListener, Action<VariableBoard> callback) : base(eventType, isWorldListener, callback) { }

        public UnitEventListener(EEventType bindEventType = EEventType.NoInit,
            bool isWorldListener = false,
            float eventTriggerInterval = 0,
            IEventChecker eventChecker = null,
            Action<VariableBoard> eventFireCallback = null) : base(bindEventType, isWorldListener, eventTriggerInterval, eventChecker, eventFireCallback) { }
    }
    
    public class UnitEventListener<TParam1> : GenericEventListener<TParam1>
    {
        public UnitEventListener(EEventType eventType, bool isWorldListener = false)
        {
            EventType = eventType;
            IsWorldListener = isWorldListener;
        }
    }

    public class UnitEventListener<TParam1, TParam2> : GenericEventListener<TParam1, TParam2>
    {
        public UnitEventListener(EEventType eventType, bool isWorldListener = false)
        {
            EventType = eventType;
            IsWorldListener = isWorldListener;
        }
    }

    public class UnitEventListener<TParam1, TParam2, TParam3> : GenericEventListener<TParam1, TParam2, TParam3>
    {
        public UnitEventListener(EEventType eventType, bool isWorldListener = false)
        {
            EventType = eventType;
            IsWorldListener = isWorldListener;
        }
    }
}