#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using NUnit.Framework;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public interface IEventChecker
    {
        public EBattleEventType EventType { get; }
        public bool CheckActorOnly(int triggerEventActorUid, IEventInfo info);
        public bool CheckGlobal(IEventInfo info);
        public void Invoke(IEventInfo info);
    }
    
    //事件监听对象的基本配置
    public class EventListener
    {
        //监听的事件
        private EBattleEventType _eventType;

        //监听间隔
        private float _triggerInterval;

        //事件触发后的数据
        //一个event对应一个数据结构

        //eventinfo 是个类似黑板的容器

        private EventChecker _checker;
        
        private Action<VariableBoard> _callback;

        public void SetupChecker(EventChecker checker)
        {
            _checker = checker;
        }

        public void SetCallBack() { }

        public void Invoke(VariableBoard board)
        {
            if (_checker == null) { }
        }
    }

    public abstract class EventChecker : IEventChecker
    {
        /// <summary>
        ///     事件类型
        /// </summary>
        private EBattleEventType _eventType;

        public EBattleEventType EventType => _eventType;

        /// <summary>
        ///     检测通过后调用函数
        /// </summary>
        private Action<IEventInfo> _func;

        /// <summary>
        ///     失效
        /// </summary>
        private bool _isDisable;

        /// <summary>
        ///     Checker属于的ActorUid
        /// </summary>
        protected readonly int CheckerBelongActorUid;

        /// <summary>
        ///     是否仅监听全部的actor发送的消息
        /// </summary>
        private bool _listenAllActor;

        protected EventChecker(EBattleEventType eventType, Actor actor, Action<IEventInfo> func = null)
        {
            _func = func;
            _eventType = eventType;
            _isDisable = false;
            CheckerBelongActorUid = actor.Uid;
        }

        protected EventChecker(EBattleEventType eventType, int actorUid, Action<IEventInfo> func = null)
        {
            _func = func;
            _eventType = eventType;
            _isDisable = false;
            CheckerBelongActorUid = actorUid;
        }

        public void BindFunc(Action<IEventInfo> func)
        {
            _func ??= func;
        }

        public void SetDisable(bool flag)
        {
            _isDisable = flag;
        }

        public void SetIsListenAll(bool flag)
        {
            _listenAllActor = flag;
        }

        public bool CheckActorOnly(int triggerEventActorUid, IEventInfo info)
        {
            if (!_listenAllActor)
            {
                return triggerEventActorUid == CheckerBelongActorUid && onCheck(info);
            }

            return onCheck(info);
        }

        public bool CheckGlobal(IEventInfo info)
        {
            return onCheck(info);
        }

        protected abstract bool onCheck(IEventInfo info);

        public virtual void Invoke(IEventInfo info)
        {
            if (_isDisable) return;
            _func?.Invoke(info);
        }
    }

    public static class EventCheckerEx
    {
        public static void Register(this EventChecker checker)
        {
            EventManager.Instance.Register(checker);
        }

        public static void UnRegister(this EventChecker checker)
        {
            EventManager.Instance.UnRegister(checker);
        }
    }
}