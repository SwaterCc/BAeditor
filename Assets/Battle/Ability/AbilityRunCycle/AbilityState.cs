using System.Collections.Generic;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityState
        {
            private Ability _ability;
            private AbilityExecutor _executor;
            private Dictionary<EAbilityState, AbilityRunCycle> _cycles;
            private bool _isActive;
            public bool IsActive => _isActive;
            private AbilityRunCycle _curCycle;
            public AbilityRunCycle Current => _curCycle;

            public AbilityState(Ability ability)
            {
                _ability = ability;
                _executor = _ability._executor;
                _cycles = new Dictionary<EAbilityState, AbilityRunCycle>()
                {
                    { EAbilityState.Init, new InitRunCycle(_ability) },
                    { EAbilityState.Ready, new Ready(_ability) },
                    { EAbilityState.PreExecute, new PreExecute(_ability) },
                    { EAbilityState.Executing, new Executing(_ability) },
                    { EAbilityState.EndExecute, new EndExecute(_ability) },
                };

                _isActive = false;
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
                }

                _curCycle.Tick(dt);
            }

            public bool TryActive()
            {
                _isActive = _curCycle.CurState == EAbilityState.Ready;
                return _isActive;
            }

            public void ActivationFailed()
            {
                _isActive = false;
            }

            public void Stop()
            {
                _curCycle.Exit();
                _curCycle = null;
            }
        }
    }
}