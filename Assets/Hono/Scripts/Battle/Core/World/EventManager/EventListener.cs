using System;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 事件监听对象
    /// 基础功能为监听指定全局事件或者是某个Actor的事件
    /// 可通过传入检测器来细化事件检测
    /// </summary>
    public class EventListener
    {
        /// <summary>
        /// 当前监听的事件
        /// </summary>
        public EEventType EventType { get; protected set; }

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
        private IEventChecker _checker;

        /// <summary>
        /// 事件触发回调
        /// </summary>
        private Action<VariableBoard> _callback;

        /// <summary>
        /// 是否失效
        /// </summary>
        public bool IsDisable { get; set; }

        public EventListener() { }

        public EventListener(EEventType eventType, Action<VariableBoard> callback) : this(
            bindEventType: eventType, eventFireCallback: callback) { }

        public EventListener(EEventType bindEventType = EEventType.NoInit,
            float eventTriggerInterval = 0,
            IEventChecker eventChecker = null,
            Action<VariableBoard> eventFireCallback = null)
        {
            EventType = bindEventType;
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

        public void SetupChecker(IEventChecker checker)
        {
            if (_checker != null)
            {
                GPoolManager.Instance.RecycleAObject(checker);                
            }
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

            if (_checker == null)
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

    public interface IEventChecker : IGPoolObject
    {
        public bool Check(in VariableBoard board);
    }
}