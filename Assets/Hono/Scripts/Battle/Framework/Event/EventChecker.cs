#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using NUnit.Framework;

#endregion

namespace Hono.Scripts.Battle.Event
{
    [Serializable]
    public abstract class EventChecker
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public EEventType EventType { get; }

        protected EventChecker(EEventType eventType)
        {
            EventType = eventType;
        }

        public bool Check(in VariableBoard board)
        {
            return onCheck(board);
        }
       
        protected abstract bool onCheck(in VariableBoard board);
    }
}