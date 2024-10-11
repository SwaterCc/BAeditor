using System;

namespace Hono.Scripts.Battle
{
    public class MessageListener
    {
        public string MsgKey { get; }

        private readonly Action<object, object, object, object, object> _callback;

        public MessageListener(string msgKey, Action<object, object, object, object, object> callback)
        {
            MsgKey = msgKey;
            _callback = callback;
        }

        public void Invoke(object p1, object p2, object p3, object p4, object p5)
        {
            _callback.Invoke(p1, p2, p3, p4, p5);
        }
    }
}