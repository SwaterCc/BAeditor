#region

using System;

#endregion

namespace Hono.Scripts.Battle.Message {
	public class MessageListener {
		public string MsgKey { get; private set; }

		private Action<object, object, object, object, object> _callback;

		public MessageListener() { }

		public MessageListener(string msgKey, Action<object, object, object, object, object> callback) {
			MsgKey = msgKey;
			_callback = callback;
		}

		public void Bind(string msgKey, Action<object, object, object, object, object> callback) {
			MsgKey = msgKey;
			_callback = callback;
		}

		public void Invoke(object p1, object p2, object p3, object p4, object p5) {
			_callback.Invoke(p1, p2, p3, p4, p5);
		}

		public void Reset() {
			MsgKey = null;
			_callback = null;
		}
	}
}