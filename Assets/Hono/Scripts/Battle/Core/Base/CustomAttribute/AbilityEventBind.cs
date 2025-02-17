#region

using System;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AbilityEventBind : System.Attribute
    {
        public Type CheckerType;

        public Type EventInfoKeyType;

        /// <summary>
        /// 事件绑定
        /// </summary>
        /// <param name="eventInfoKeyType">事件EventInfoKey绑定</param>
        /// <param name="checkerType">检查器绑定</param>
        public AbilityEventBind(Type eventInfoKeyType, Type checkerType)
        {
            CheckerType = checkerType;
            EventInfoKeyType = eventInfoKeyType;
        }
    }
}