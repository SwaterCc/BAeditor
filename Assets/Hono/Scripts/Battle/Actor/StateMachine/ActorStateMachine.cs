using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        /// <summary>
        /// 基础状态机基类
        /// 目前定义的状态：闲置，移动，战斗，硬直，死亡
        /// </summary>
        public class ActorStateMachine
        {
            public Actor Actor { get; }
            public ActorLogic Logic { get; }
            public EActorLogicStateType CurStateType => _curStateType;

            private ActorLogicState _current;

            private EActorLogicStateType _curStateType;

            private EActorLogicStateType _nextStateType;

            private readonly Dictionary<EActorLogicStateType, ActorLogicState> _states;

            public ActorStateMachine(ActorLogic logic)
            {
                Logic = logic;
                Actor = logic.Actor;

                _states = new Dictionary<EActorLogicStateType, ActorLogicState>()
                {
                    { EActorLogicStateType.Idle, new IdleState(this,EActorLogicStateType.Idle) },
                    { EActorLogicStateType.Skill, new SkillState(this, EActorLogicStateType.Skill) },
                    { EActorLogicStateType.Move, new MoveState(this, EActorLogicStateType.Move) },
                    { EActorLogicStateType.Death, new DeathState(this, EActorLogicStateType.Death) },
                    { EActorLogicStateType.Stiff, new StiffState(this, EActorLogicStateType.Stiff) },
                };
            }

            public void Init()
            {
                foreach (var state in _states)
                {
                    state.Value.Init();
                }
                
                _nextStateType = EActorLogicStateType.Idle;
                _curStateType = _nextStateType;
                _current = _states[_curStateType];
                _current.Enter();
            }

            public void UnInit()
            {
                _current = null;
            }
            
            public void SwitchState(EActorLogicStateType nextLogicStateType)
            {
                _nextStateType = nextLogicStateType;
            }

            public void Tick(float dt)
            {
                if(_current == null) return;
                
                _current.Tick(dt);

                var hasAutoSwitch = _current.TryGetAutoSwitchState(out var autoNext);

                var realNext = CurStateType != _nextStateType ? _nextStateType : autoNext;
                
                if (CurStateType != _nextStateType || hasAutoSwitch)
                {
                    _current.Exit();
                    _current = _states[realNext];
                    _curStateType = realNext;
					_nextStateType = _curStateType;
					_current.Enter();
                }
            }
        }
    }
}