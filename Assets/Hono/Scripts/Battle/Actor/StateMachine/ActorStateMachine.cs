#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 逻辑状态机
    /// 目前定义的状态：闲置，移动，攻击，硬直，死亡
    /// 状态切换逻辑，和拥有的状态机由具体的logic定义
    /// </summary>
    public class ActorStateMachine
    {
        public Actor Actor { get; }
        public ActorLogic Logic { get; }
        public EActorStateType CurStateType => _curStateType;

        private ActorState _current;

        private EActorStateType _curStateType;

        private EActorStateType _nextStateType;

        private readonly Dictionary<EActorStateType, ActorState> _states;
        private readonly Dictionary<EActorStateType, StateTransform> _stateTransforms;

        public ActorStateMachine(ActorLogic logic)
        {
            Logic = logic;
            Actor = logic.Self;
        }

        public void AddState() { }

        public void AddTransform() { }

        public void InitWhenEnterScene()
        {
            foreach (var state in _states)
            {
                state.Value.Init();
            }
        }

        public void SwitchState(EActorStateType nextStateType)
        {
            _nextStateType = nextStateType;
        }

        public void Tick(float dt)
        {
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