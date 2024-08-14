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

            private AbilityRunCycle _curCycle;
            public AbilityRunCycle Current => _curCycle;
            public AbilityState(Ability ability)
            {
                _ability = ability;
                _executor = _ability._executor;
                _cycles = new Dictionary<EAbilityState, AbilityRunCycle>()
                {
                    
                };
            }

            /// <summary>
            /// Ability初始化的下一帧执行
            /// </summary>
            /// <param name="dt"></param>
            public void Tick(float dt)
            {
                if (_curCycle == null)
                {//第一次
                    _curCycle = _cycles[EAbilityState.Init];
                    _curCycle.OnEnter();
                }

                if (_curCycle.IsFinish)
                {
                    _curCycle.OnExit();
                    _curCycle = _cycles[_curCycle.GetNextState()];
                    _curCycle.OnEnter();
                }
                
                _curCycle.OnTick(dt);
            }

            public void Stop()
            {
                _curCycle.OnExit();
                _curCycle = null;
            }
        }   
    }
}