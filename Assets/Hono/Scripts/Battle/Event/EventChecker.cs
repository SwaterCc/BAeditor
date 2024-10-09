using System;

namespace Hono.Scripts.Battle.Event {
	public interface IEventChecker {
		public EBattleEventType EventType { get; }
		public bool Check(int triggerEventActorUid, IEventInfo info);
		public void Invoke(IEventInfo info);
	}

	public abstract class EventChecker : IEventChecker {
		/// <summary>
		/// 事件类型
		/// </summary>
		private EBattleEventType _eventType;
		public EBattleEventType EventType => _eventType;

		/// <summary>
		/// 检测通过后调用函数
		/// </summary>
		private Action<IEventInfo> _func;

		/// <summary>
		/// 失效
		/// </summary>
		private bool _isDisable;

		/// <summary>
		/// Checker属于的ActorUid
		/// </summary>
		protected readonly int _checkerBelongActorUid;

		/// <summary>
		/// 是否仅监听全部的actor发送的消息
		/// </summary>
		private bool _listenAllActor;

		protected EventChecker(EBattleEventType eventType, Actor actor, Action<IEventInfo> func = null) {
			_func = func;
			_eventType = eventType;
			_isDisable = false;
			_checkerBelongActorUid = actor.Uid;
		}
		
		protected EventChecker(EBattleEventType eventType, int actorUid, Action<IEventInfo> func = null) {
			_func = func;
			_eventType = eventType;
			_isDisable = false;
			_checkerBelongActorUid = actorUid;
		}

		public void BindFunc(Action<IEventInfo> func) {
			_func ??= func;
		}

		public void SetDisable(bool flag) {
			_isDisable = flag;
		}

		public void SetIsListenAll(bool flag) {
			_listenAllActor = flag;
		}

		public bool Check(int triggerEventActorUid, IEventInfo info) {
			if (!_listenAllActor) {
				return triggerEventActorUid == _checkerBelongActorUid && onCheck(info);
			}

			return onCheck(info);
		}

		protected abstract bool onCheck(IEventInfo info);

		public virtual void Invoke(IEventInfo info) {
			if (_isDisable) return;
			_func?.Invoke(info);
		}

		public void UnRegister() {
			BattleEventManager.Instance.UnRegister(this);
		}
	}
}