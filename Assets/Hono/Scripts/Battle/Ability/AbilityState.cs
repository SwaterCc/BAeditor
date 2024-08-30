using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityState
        {
            private Ability _ability;
            public Ability Ability => _ability;
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
                    { EAbilityState.Init, new InitRunCycle(this) },
                    { EAbilityState.Ready, new Ready(this) },
                    { EAbilityState.PreExecute, new PreExecute(this) },
                    { EAbilityState.Executing, new Executing(this) },
                    { EAbilityState.EndExecute, new EndExecute(this) },
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