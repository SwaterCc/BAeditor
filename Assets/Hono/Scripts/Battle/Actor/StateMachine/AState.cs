using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public abstract class AState {
			protected class AStateTransform {
				public bool IsEnable;
				public EActorState Next { get; }
				private readonly Func<bool> _transCondition;

				public AStateTransform(EActorState next, Func<bool> condition = null, bool isEnable = true) {
					Next = next;
					IsEnable = isEnable;
					_transCondition = condition;
				}

				public bool Invoke() {
					if (!IsEnable) return false;
					if (IsEnable && _transCondition == null) return true;
					return IsEnable && _transCondition.Invoke();
				}
			}

			private ActorStateMachine _stateMachine;
			protected readonly Dictionary<EActorState, List<AStateTransform>> _transDict;

			protected bool _canExit;
			private AStateTransform _next;

			protected AState(ActorStateMachine machine) {
				_stateMachine = machine;
				_next = null;
				_canExit = false;
				_transDict = new Dictionary<EActorState, List<AStateTransform>>() {
					{ EActorState.Idle, new List<AStateTransform>() },
					{ EActorState.Battle, new List<AStateTransform>() },
					{ EActorState.Move, new List<AStateTransform>() },
					{ EActorState.Stiff, new List<AStateTransform>() },
					{ EActorState.Death, new List<AStateTransform>() },
				};
			}

			public bool HasTrans(EActorState go) {
				return _transDict[go].Count == 0;
			}

			public EActorState StateType => getStateType();
			protected abstract EActorState getStateType();

			public virtual bool CanExit() {
				return _canExit;
			}

			public EActorState GetNext() {
				return _next.Next;
			}

			public abstract void Init();
			protected virtual void onEnter() { }

			public virtual void Enter() {
				onEnter();
			}

			public virtual void OnChangeState(EActorState next) {
				foreach (var transform in _transDict[next]) {
					if (transform.Invoke()) {
						_next = transform;
						_canExit = true;
					}
				}
			}

			protected virtual void onTick(float dt) { }

			public void Tick(float dt) {
				onTick(dt);

				if (!_canExit) {
					return;
				}

				foreach (var stateTrans in _transDict) {
					foreach (var transform in stateTrans.Value) {
						if (transform.Invoke()) {
							_next = transform;
						}
					}
				}
			}

			protected virtual void onExit() { }

			public void Exit() {
				onExit();
				_canExit = false;
				_next = null;
			}
		}
	}
}