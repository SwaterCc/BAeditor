namespace Hono.Scripts.Battle {

		public abstract class ActorState {
			protected ActorLogic _actorLogic;
			protected ActorStateMachine _stateMachine;
			private float _duration;
			public EActorStateType StateType { get; }
			public float StateDuration => _duration;

			protected ActorState(ActorStateMachine machine, EActorStateType stateType) {
				StateType = stateType;
				_stateMachine = machine;

				_actorLogic = machine.Logic;
			}

			public virtual void Init() { }
			protected virtual void onEnter() { }

			public void Enter() {
				_duration = 0;
				onEnter();
			}

			protected virtual void onTick(float dt) { }

			public void Tick(float dt) {
				onTick(dt);
				_duration += dt;
			}

			public abstract bool TryGetAutoSwitchState(out EActorStateType next);

			protected virtual void onExit() { }

			public void Exit() {
				onExit();
			}
		}
	
}