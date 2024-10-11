using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;


namespace Hono.Scripts.Battle.Event
{
    /// <summary>
    /// 管理战斗逻辑中事件节点的注册和监听
    /// </summary>
    public class BattleEventManager : Singleton<BattleEventManager>, IBattleFrameworkEnterExit
    {
        /// <summary>
        /// 事件注册列表
        /// </summary>
        private readonly Dictionary<EBattleEventType, List<IEventChecker>> _eventDict = new(128);

        public void OnEnterBattle()
        {
            _eventDict.Clear();
        }

        public void OnExitBattle()
        {
            _eventDict.Clear();
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
        public void TriggerEvent(int actorUid, EBattleEventType eventType, IEventInfo eventInfo = null)
        {
            if (!_eventDict.TryGetValue(eventType, out var checkers)) return;
            foreach (IEventChecker checker in checkers)
            {
                if (checker.Check(actorUid, eventInfo))
                {
                    checker.Invoke(eventInfo);
                }
            }
        }
    }
}