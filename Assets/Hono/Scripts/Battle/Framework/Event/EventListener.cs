using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 事件监听对象
    /// <para/>
    /// 基础功能为监听指定全局事件或者是某个Actor的事件
    /// <para/>
    /// 可通过传入检测器来细化事件检测
    /// </summary>
    public class EventListener
    {
        /// <summary>
        /// 当前监听的事件
        /// </summary>
        public EEventType EventType { get; private set; }

        /// <summary>
        /// 已等待的间隔时长
        /// </summary>
        private float _waitTriggerDuration;

        /// <summary>
        /// 监听间隔
        /// </summary>
        private float _triggerInterval;

        /// <summary>
        /// 检查器
        /// </summary>
        private EventChecker _checker;

        /// <summary>
        /// 事件触发回调
        /// </summary>
        private Action<VariableBoard> _callback;

        /// <summary>
        /// 是否监听全局事件
        /// </summary>
        public bool IsGlobalListener { get; set; }

        /// <summary>
        /// 是否失效
        /// </summary>
        public bool IsDisable { get; set; }

        protected EventListener() { }

        protected EventListener(EEventType eventType, bool isGlobalListener, Action<VariableBoard> callback) : this(
            bindEventType: eventType, isGlobalListener: isGlobalListener, eventFireCallback: callback) { }

        protected EventListener(EEventType bindEventType = EEventType.NoInit,
            bool isGlobalListener = false,
            float eventTriggerInterval = 0,
            EventChecker eventChecker = null,
            Action<VariableBoard> eventFireCallback = null)
        {
            EventType = bindEventType;
            IsGlobalListener = isGlobalListener;
            _triggerInterval = eventTriggerInterval;
            _callback = eventFireCallback;
            _checker = eventChecker;
        }

        public void Clear()
        {
            EventType = EEventType.NoInit;
            _triggerInterval = 0;
            _callback = null;
            _checker = null;
        }

        /// <summary>
        /// 绑定事件类型，仅允许在事件类型未初始化时绑定，如果事件已绑定再次调用会报错，使用Clear函数后可重新绑定
        /// </summary>
        /// <param name="eventType"></param>
        /// <exception cref="Exception"></exception>
        public void BindEvent(EEventType eventType)
        {
            if (EventType != EEventType.NoInit)
            {
                throw new Exception("不允许更改已经绑定的事件监听");
            }

            EventType = eventType;
        }

        public void SetupChecker(EventChecker checker)
        {
            _checker = checker;
        }

        public void SetCallback(Action<VariableBoard> eventFireCallback)
        {
            _callback = eventFireCallback;
        }

        /// <summary>
        /// 设置事件触发间隔
        /// </summary>
        /// <param name="interval"></param>
        public void SetInterval(float interval)
        {
            _triggerInterval = interval;
        }

        public void Tick(float dt)
        {
            if (_triggerInterval == 0)
            {
                return;
            }

            _waitTriggerDuration += dt;
        }

        public void OnEventFired(VariableBoard board)
        {
            if (IsDisable)
            {
                return;
            }

            if (_waitTriggerDuration < _triggerInterval)
            {
                return;
            }

            if (_checker == null || _checker.EventType != EventType)
            {
                _callback?.Invoke(board);
                _waitTriggerDuration = 0;
            }
            else
            {
                if (_checker.Check(board))
                {
                    _callback?.Invoke(board);
                    _waitTriggerDuration = 0;
                }
            }
        }
    }

    /// <summary>
    /// Actor对象内部使用的EventListener，该Listener会在Actor被回收时失效
    /// </summary>
    public class ActorEventListener : EventListener
    {
        public ActorEventListener() { }
        public ActorEventListener(EEventType eventType, bool isGlobalListener, Action<VariableBoard> callback) : base(eventType, isGlobalListener, callback) { }

        public ActorEventListener(EEventType bindEventType = EEventType.NoInit,
            bool isGlobalListener = false,
            float eventTriggerInterval = 0,
            EventChecker eventChecker = null,
            Action<VariableBoard> eventFireCallback = null) : base(bindEventType, isGlobalListener, eventTriggerInterval, eventChecker, eventFireCallback) { }
    }

    /// <summary>
    /// 全局Listener，不会像ActorEventListener一样在回收时失效，不建议在Actor内部使用
    /// </summary>
    public class GlobalEventListener : EventListener
    {
        public GlobalEventListener() { }
        public GlobalEventListener(EEventType eventType, Action<VariableBoard> callback) : base(eventType, true, callback) { }

        public GlobalEventListener(EEventType bindEventType = EEventType.NoInit,
            float eventTriggerInterval = 0,
            EventChecker eventChecker = null,
            Action<VariableBoard> eventFireCallback = null) : base(bindEventType, true, eventTriggerInterval, eventChecker, eventFireCallback) { }
    }
}