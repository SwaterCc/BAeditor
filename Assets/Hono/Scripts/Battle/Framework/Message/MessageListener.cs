#region

using System;
using Hono.Scripts.Battle.Base;

#endregion

namespace Hono.Scripts.Battle.Message
{
    public class MessageListener
    {
        /// <summary>
        /// 关联的Actor
        /// </summary>
        public int BindActorUid { get; private set; }

        /// <summary>
        /// 绑定的MsgKey
        /// </summary>
        public string MsgKey { get; private set; }

        /// <summary>
        /// msg收到消息时的回调
        /// </summary>
        private Action<VariableBoard> _callback;

        public MessageListener()
        {
            BindActorUid = -1;
            MsgKey = null;
            _callback = null;
        }

        public MessageListener(int bindActorUid, string msgKey, Action<VariableBoard> callback)
        {
            BindActorUid = bindActorUid;
            MsgKey = msgKey;
            _callback = callback;
        }

        public void Bind(string msgKey, Action<VariableBoard> callback)
        {
            MsgKey = msgKey;
            _callback = callback;
        }

        public void Invoke(VariableBoard board)
        {
            _callback?.Invoke(board);
        }

        public void Clear()
        {
            BindActorUid = -1;
            MsgKey = null;
            _callback = null;
        }
    }
}