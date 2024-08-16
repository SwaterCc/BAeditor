using System.Collections.Generic;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityState
        {
            private Ability _ability;
            private readonly Dictionary<EAbilityState, AbilityRunCycle> _cycles;
            private bool _hasExecuteOrder;
            public bool HasExecuteOrder => _hasExecuteOrder;
            private AbilityRunCycle _curCycle;
            public AbilityRunCycle Current => _curCycle;

            public AbilityState(Ability ability)
            {
                _ability = ability;
                _cycles = new Dictionary<EAbilityState, AbilityRunCycle>()
                {
                    { EAbilityState.Init, new InitRunCycle(_ability) },
                    { EAbilityState.Ready, new Ready(_ability) },
                    { EAbilityState.PreExecute, new PreExecute(_ability) },
                    { EAbilityState.Executing, new Executing(_ability) },
                    { EAbilityState.EndExecute, new EndExecute(_ability) },
                };

                _hasExecuteOrder = false;
            }

            public AbilityRunCycle GetState(EAbilityState state)
            {
                return _cycles[state];
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

                while (_curCycle.CanExit())
                {
                    _curCycle.Exit();
                    _curCycle = _cycles[_curCycle.GetNextState()];
                    _curCycle.Enter();
                    if (_curCycle.CurState == EAbilityState.Executing)
                    {
                        _hasExecuteOrder = false;
                    }
                }

                _curCycle.Tick(dt);
            }

            public void TryExecute()
            {
                _hasExecuteOrder = _curCycle.CurState == EAbilityState.Ready;
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