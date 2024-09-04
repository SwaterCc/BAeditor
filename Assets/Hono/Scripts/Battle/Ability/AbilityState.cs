using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        public class CycleCallback
        {
            public Action OnEnter;
            public Action OnTick;
            public Action OnExit;
        }

        private class AbilityState
        {
            public Ability Ability => _ability;
            public AbilityRunCycle Current => _curCycle;
            public bool HasExecuteOrder => _hasExecuteOrder;

            private readonly Ability _ability;
            private AbilityRunCycle _curCycle;
            private bool _hasExecuteOrder;
            private readonly Dictionary<EAbilityState, AbilityRunCycle> _cycles;
            private readonly Dictionary<EAbilityAllowEditCycle, CycleCallback> _callbackDict;

            public AbilityState(Ability ability)
            {
                _ability = ability;
                _cycles = new Dictionary<EAbilityState, AbilityRunCycle>()
                {
                    { EAbilityState.Init, new OnInitCycle(this) },
                    { EAbilityState.Ready, new ReadyCycle(this) },
                    { EAbilityState.PreExecute, new PreExecuteCycle(this) },
                    { EAbilityState.Executing, new ExecutingCycle(this) },
                    { EAbilityState.EndExecute, new EndExecuteCycle(this) },
                };

                _callbackDict = new Dictionary<EAbilityAllowEditCycle, CycleCallback>()
                {
                    { EAbilityAllowEditCycle.OnInit, new CycleCallback() },
                    { EAbilityAllowEditCycle.OnReady, new CycleCallback() },
                    { EAbilityAllowEditCycle.OnPreExecuteCheck, new CycleCallback() },
                    { EAbilityAllowEditCycle.OnPreExecute, new CycleCallback() },
                    { EAbilityAllowEditCycle.OnExecuting, new CycleCallback() },
                    { EAbilityAllowEditCycle.OnEndExecute, new CycleCallback() },
                };

                _hasExecuteOrder = false;
            }

            public AbilityRunCycle GetState(EAbilityState state)
            {
                return _cycles[state];
            }

            public CycleCallback GetCycleCallback(EAbilityAllowEditCycle allowEditCycle)
            {
                return _callbackDict[allowEditCycle];
            }

            /// <summary>
            /// Ability初始化的下一帧执行
            /// </summary>
            /// <param name="dt"></param>
            public void Tick(float dt)
            {
                if (_curCycle == null)
                {
                    //第一次
                    _curCycle = _cycles[EAbilityState.Init];
                    _curCycle.Enter();
                }

                _curCycle.Tick(dt);

                while (_curCycle.CanExit())
                {
                    var nextState = _curCycle.GetNextState();
                    _curCycle.Exit();
                    _curCycle = _cycles[nextState];
                    _curCycle.Enter();
                    if (_curCycle.CurState == EAbilityState.Executing)
                    {
                        _hasExecuteOrder = false;
                    }
                }
            }

            public void TryExecute()
            {
                _hasExecuteOrder = true;
            }

            public void ExecuteFailed()
            {
                _hasExecuteOrder = false;
            }

            public void Stop()
            {
                _curCycle.Exit();
                _curCycle = null;
            }

            public void OnDestroy()
            {
                _hasExecuteOrder = false;
                Stop();
            }
        }
    }
}