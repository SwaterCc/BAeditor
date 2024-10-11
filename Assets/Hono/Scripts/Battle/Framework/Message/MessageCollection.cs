using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// msg要能存储，要能当帧执行
    /// </summary>
    public class MessageCollection
    {
        private readonly Dictionary<string, List<MessageListener>> _msgHandlers;

        public int Uid { get; }

        public MessageCollection(Actor actor)
        {
            Uid = actor.Uid;
            _msgHandlers = new Dictionary<string, List<MessageListener>>();
        }

        public void Init()
        {
            MessageCenter.Instance.Register(Uid, this);
        }

        /// <summary>
        /// 直接调用msg，该消息将不会进入缓存
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="p5"></param>
        /// <returns></returns>
        public bool SendMsg(string msg, object p1, object p2, object p3, object p4, object p5)
        {
            if (_msgHandlers.TryGetValue(msg, out var listeners))
            {
                if (listeners.Count > 0)
                {
                    foreach (var listener in listeners)
                    {
                        listener.Invoke(p1, p2, p3, p4, p5);
                    }

                    return true;
                }
            }

            return false;
        }

        public bool SendMsgCheck(string msgKey)
        {
            if (!_msgHandlers.TryGetValue(msgKey, out var listeners)) return false;
            return listeners.Count > 0;
        }
        
        public void AddListener(MessageListener listener)
        {
            if (!_msgHandlers.TryGetValue(listener.MsgKey, out var handlers))
            {
                handlers = new List<MessageListener>();
                _msgHandlers.Add(listener.MsgKey, handlers);
            }

            handlers.Add(listener);
        }

        public void RemoveListener(MessageListener listener)
        {
            if (_msgHandlers.TryGetValue(listener.MsgKey, out var handlers))
            {
                handlers.Remove(listener);
            }
        }

        public void UnInit()
        {
            MessageCenter.Instance.Unregister(Uid);
        }
    }
}