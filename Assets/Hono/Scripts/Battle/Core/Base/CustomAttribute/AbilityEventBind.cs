#region

using System;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AbilityEventBind : System.Attribute
    {
        public readonly string CheckerGetFunc;

        public readonly Type EventInfoKeyType;

        /// <summary>
        /// 事件绑定
        /// </summary>
        /// <param name="eventInfoKeyType">事件EventInfoKey绑定</param>
        /// <param name="checkerGetFunc">检查器获取函数</param>
        public AbilityEventBind(Type eventInfoKeyType, string checkerGetFunc)
        {
            CheckerGetFunc = checkerGetFunc;
            EventInfoKeyType = eventInfoKeyType;
        }
    }
}