using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public abstract class AState
        {
            protected class AStateTransform
            {
                public bool IsEnable;
                public EActorState Next { get; }
                private readonly Func<bool> _transCondition;

                public AStateTransform(EActorState next, Func<bool> exitCondition = null, bool isEnable = true)
                {
                    Next = next;
                    IsEnable = isEnable;
                    _transCondition = exitCondition;
                }

                public bool Invoke()
                {
                    if (!IsEnable || _transCondition == null) return false;
                    return IsEnable && _transCondition.Invoke();
                }
            }

            protected ActorLogic _actorLogic;
            protected ActorStateMachine _stateMachine;
            protected readonly Dictionary<EActorState, List<AStateTransform>> _transDict;
            
            protected EActorState _defaultState;

            private AStateTransform _next;

            protected AState(ActorStateMachine machine, ActorLogic actorLogicLogic)
            {
                _stateMachine = machine;
                _actorLogic = actorLogicLogic;
                _defaultState = EActorState.Idle;
                _next = null;
                _transDict = new Dictionary<EActorState, List<AStateTransform>>()
                {
                    { EActorState.Idle, new List<AStateTransform>() },
                    { EActorState.Battle, new List<AStateTransform>() },
                    { EActorState.Move, new List<AStateTransform>() },
                    { EActorState.Stiff, new List<AStateTransform>() },
                    { EActorState.Death, new List<AStateTransform>() },
                };
            }

            public bool HasTrans(EActorState go)
            {
                return _transDict[go].Count == 0;
            }

            public EActorState StateType => getStateType();
            protected abstract EActorState getStateType();

            public virtual bool CanExit(out EActorState next)
            {
                foreach (var pState in _transDict)
                {
                    foreach (var transform in pState.Value)
                    {
                        if (transform.Invoke())
                        {
                            next = transform.Next;
                            return true;
                        }
                    }
                }

                next = EActorState.Idle;
                return false;
            }

            public abstract void Init();
            protected virtual void onEnter() { }

            public virtual void Enter()
            {
                onEnter();
            }

            protected virtual void onTick(float dt) { }

            public void Tick(float dt)
            {
                onTick(dt);
            }

            protected virtual void onExit() { }

            public void Exit()
            {
                onExit();
                _next = null;
            }
        }
    }
}