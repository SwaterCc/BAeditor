using System;

namespace Hono.Scripts.Battle.Event
{
    public abstract class GenericEventListener<TParam1> : EventListener
    {
        private Action<TParam1> _genericCallback;

        /// <summary>
        /// 设置泛型回调函数
        /// </summary>
        public void SetGenericCallback(Action<TParam1> callback)
        {
            _genericCallback = callback;
        }

        /// <summary>
        /// 触发事件并传递泛型参数
        /// </summary>
        protected void TriggerEventWithParams(TParam1 param1)
        {
            if (IsDisable || EventType == EEventType.NoInit)
            {
                return;
            }

            // 调用泛型回调函数
            _genericCallback?.Invoke(param1);
        }
    }

    public abstract class GenericEventListener<TParam1, TParam2> : EventListener
    {
        private Action<TParam1, TParam2> _genericCallback;

        /// <summary>
        /// 设置泛型回调函数
        /// </summary>
        public void SetGenericCallback(Action<TParam1, TParam2> callback)
        {
            _genericCallback = callback;
        }

        /// <summary>
        /// 触发事件并传递泛型参数
        /// </summary>
        protected void TriggerEventWithParams(TParam1 param1, TParam2 param2)
        {
            if (IsDisable || EventType == EEventType.NoInit)
            {
                return;
            }

            // 调用泛型回调函数
            _genericCallback?.Invoke(param1, param2);
        }
    }

    public abstract class GenericEventListener<TParam1, TParam2, TParam3> : EventListener
    {
        private Action<TParam1, TParam2, TParam3> _genericCallback;

        /// <summary>
        /// 设置泛型回调函数
        /// </summary>
        public void SetGenericCallback(Action<TParam1, TParam2, TParam3> callback)
        {
            _genericCallback = callback;
        }

        /// <summary>
        /// 触发事件并传递泛型参数
        /// </summary>
        protected void TriggerEventWithParams(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            if (IsDisable || EventType == EEventType.NoInit)
            {
                return;
            }

            // 调用泛型回调函数
            _genericCallback?.Invoke(param1, param2, param3);
        }
    }
}