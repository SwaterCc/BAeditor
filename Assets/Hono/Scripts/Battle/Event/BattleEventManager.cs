using System;
using System.Collections.Generic;


namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 管理战斗逻辑中事件节点的注册和监听
    /// </summary>
    public class BattleEventManager
    {
        private static BattleEventManager _instance;

        public static BattleEventManager Instance
        {
            get
            {
                //TODO:目前简单写，后续要判定线程加锁 @shirui
                return _instance ??= new BattleEventManager();
            }
        }

        /// <summary>
        /// 事件注册列表
        /// </summary>
        private readonly Dictionary<EBattleEventType, List<IEventChecker>> _eventDict = new(128);

        public void Register<TBindEventInfo>(EventChecker checker)
        {
            checker.BindEventInfoType<TBindEventInfo>();
            Register(checker);
        }

        public void Register(IEventChecker checker)
        {
            if (!_eventDict.TryGetValue(checker.EventType, out var handles))
            {
                handles = new List<IEventChecker>();
                _eventDict.Add(checker.EventType, handles);
            }

            if (!handles.Contains(checker))
            {
                handles.Add(checker);
            }
        }

        public void UnRegister(IEventChecker checker)
        {
            if (!_eventDict.TryGetValue(checker.EventType, out var handles)) return;
            if (handles.Contains(checker))
            {
                handles.Remove(checker);
            }
        }
        

        /// <summary>
        /// 触发事件
        /// </summary>
        public void TriggerEvent(EBattleEventType eventType)
        {
            if (!_eventDict.TryGetValue(eventType, out var checkers)) return;
            foreach (var checker in checkers)
            {
                checker.Invoke(null);
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        public void TriggerEvent(EBattleEventType eventType, IEventInfo eventInfo)
        {
            if (!_eventDict.TryGetValue(eventType, out var checkers)) return;
            foreach (var checker in checkers)
            {
                if (checker.compare(eventInfo))
                {
                    checker.Invoke(eventInfo);
                }
            }
        }
    }
}