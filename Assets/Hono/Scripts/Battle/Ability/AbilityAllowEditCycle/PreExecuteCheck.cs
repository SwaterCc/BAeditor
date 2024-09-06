using System;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class PreExecuteChecker
        {
            private readonly Ability _ability;
            private readonly AbilityState _state;
            private readonly AbilityExecutor _executor;
            private readonly EAbilityAllowEditCycle _headNode;

            public PreExecuteChecker(Ability ability)
            {
                _ability = ability;
                _state = ability._state;
                _executor = ability._executor;
                _headNode = EAbilityAllowEditCycle.OnPreExecuteCheck;
            }

            public bool GetCheckRes()
            {
                bool checkRes = true;

                if (_executor.HeadNodeHasChildren(_headNode))
                {
                    _ability._variables.Add(_executor.AbilityData.PreCheckerVarName, false);

                    _executor.ExecuteCycleNode(_headNode);
                    
                    _state.GetCycleCallback(_headNode).OnEnter?.Invoke();
                    _state.GetCycleCallback(_headNode).OnTick?.Invoke();
                    _state.GetCycleCallback(_headNode).OnExit?.Invoke();
                    //TODO:性能问题
                    checkRes = (bool)_ability._variables.GetVariable(_executor.AbilityData.PreCheckerVarName);
                    _ability._variables.Remove(_executor.AbilityData.PreCheckerVarName);
                }

                return checkRes;
            }
        }
    }
}