using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		/// <summary>
		/// 基础状态机基类
		/// 目前定义的状态：闲置，移动，战斗，硬直，死亡
		/// </summary>
		public class ActorStateMachine : ITick {
			private ActorLogic _actorLogic;
			private AState _current;
			private EActorState _defaultState;
			private Dictionary<EActorState, AState> _states;
			public AState Current => _current;
			
			public ActorStateMachine(ActorLogic actor) {
				_actorLogic = actor;
				_defaultState = EActorState.Idle;
				_states = new Dictionary<EActorState, AState>()
				{
					{ EActorState.Idle ,new IdleState(this,_actorLogic)},
					{ EActorState.Battle ,new BattleState(this,_actorLogic)},
					{ EActorState.Move ,new MoveState(this,_actorLogic)},
					{ EActorState.Death ,new DeathState(this,_actorLogic)},
					{ EActorState.Stiff ,new StiffState(this,_actorLogic)},
				};
			}
			
			public void Init()
			{
				foreach (var state in _states) {
					state.Value.Init();
				}
			}

			public void ChangeState(EActorState state) {
				if (_current != null && _current.HasTrans(state)) {
					_current.Exit();
					_states[state].Enter();
				}
			}

			public void Tick(float dt) {
				//两种切换，自然切换，和强制切换
				if (_current == null) {
					//第一次运行，进入默认状态
					_current = _states[_defaultState];
					_current.Enter();
				}

				if (_current.CanExit(out var next)) {
					_current.Exit();
					_current= _states[next];
					_current.Enter();
				}

				_current.Tick(dt);
			}
		}
	}
}