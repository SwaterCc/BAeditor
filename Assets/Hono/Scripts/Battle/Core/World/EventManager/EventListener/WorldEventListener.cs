using System;

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 全局Listener，不会像ActorEventListener一样在回收时失效，不建议在Actor内部使用
    /// </summary>
    public class WorldEventListener : EventListener
    {
        public WorldEventListener() { }
        public WorldEventListener(EEventType eventType, Action<VariableBoard> callback) : base(eventType, true, callback) { }

        public WorldEventListener(EEventType bindEventType = EEventType.NoInit,
            float eventTriggerInterval = 0,
            IEventChecker eventChecker = null,
            Action<VariableBoard> eventFireCallback = null) : base(bindEventType, true, eventTriggerInterval, eventChecker, eventFireCallback) { }
    }
    
    public class WorldEventListener<TParam1> : GenericEventListener<TParam1>
    {
        public WorldEventListener(EEventType eventType)
        {
            EventType = eventType;
            IsWorldListener = true;
        }
    }

    public class WorldEventListener<TParam1, TParam2> : GenericEventListener<TParam1, TParam2>
    {
        public WorldEventListener(EEventType eventType)
        {
            EventType = eventType;
            IsWorldListener = true;
        }
    }

    public class WorldEventListener<TParam1, TParam2, TParam3> : GenericEventListener<TParam1, TParam2, TParam3>
    {
        public WorldEventListener(EEventType eventType)
        {
            EventType = eventType;
            IsWorldListener = true;
        }
    }
}