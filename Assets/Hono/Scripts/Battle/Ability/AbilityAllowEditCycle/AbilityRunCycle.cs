
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private interface ITimer
        {
            public bool IsFinish();
            public void Step(float dt);
            public bool NeedCall();
            public void OnCallTimer();
        }

        private abstract class AbilityRunCycle
        {
            protected Ability _ability;
            protected AbilityExecutor _executor;
            protected AbilityState _state;

            protected readonly HashSet<ITimer> _timerNodes;
            protected readonly List<ITimer> _removes;
            public EAbilityState CurState => getCurState();

            protected AbilityRunCycle(AbilityState state)
            {
                _state = state;
                _ability = _state.Ability;
                _executor = _state.Ability._executor;
                _timerNodes = new HashSet<ITimer>();
                _removes = new List<ITimer>();
            }

            public virtual void TimerStart(ITimer callBack)
            {
                _timerNodes.Add(callBack);
            }

            protected abstract EAbilityAllowEditCycle getCycleType();
            protected abstract EAbilityState getCurState();

            public void Enter()
            {
                onEnter();
                if (CurState != EAbilityState.Ready)
                    _executor.ExecuteCycleNode(getCycleType());
                _state.GetCycleCallback(getCycleType()).OnEnter?.Invoke();
            }

            protected virtual void onEnter() { }

            public virtual void Tick(float dt)
            {
                foreach (var timer in _timerNodes)
                {
                    timer.Step(dt);
                    if (timer.NeedCall())
                    {
                        timer.OnCallTimer();
                    }

                    if (timer.IsFinish())
                    {
                        _removes.Add(timer);
                    }
                }

                onTick(dt);
                _state.GetCycleCallback(getCycleType()).OnTick?.Invoke();
                
                foreach (var timer in _removes)
                {
                    _timerNodes.Remove(timer);
                }

                _removes.Clear();
            }

            protected virtual void onTick(float dt) { }

            public void Exit()
            {
                _state.GetCycleCallback(getCycleType()).OnExit?.Invoke();
                onExit();
                _timerNodes.Clear();
                _removes.Clear();
            }

            protected virtual void onExit() { }

            public abstract EAbilityState GetNextState();

            public virtual bool CanExit()
            {
                return _timerNodes.Count == 0;
            }

            public virtual void OnReset() {
	            
            }
            
            public virtual void OnReload() { }
        }
    }
}