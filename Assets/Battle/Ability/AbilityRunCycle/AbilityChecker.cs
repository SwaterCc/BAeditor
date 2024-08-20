using System;
using System.Collections.Generic;

namespace Battle
{
    public partial class Ability
    {
        public bool GetCheckerRes(EAbilityCycleType checker)
        {
            return _checkers[checker].GetCheckRes();
        }

        private abstract class AbilityChecker
        {
            private bool _checkRes;
            private readonly Ability _ability;
            private readonly AbilityExecutor _executor;
            private Func<bool> _baseChecker;

            protected AbilityChecker(Ability ability)
            {
                _ability = ability;
                _executor = ability._executor;
                _checkRes = true;
            }

            public void SetBaseChecker(Func<bool> func)
            {
                _baseChecker = func;
            }
            
            protected abstract EAbilityCycleType getHeadType();

            public bool GetCheckRes()
            {
                if (_baseChecker != null)
                {
                    _checkRes = _baseChecker.Invoke();
                }

                if (_executor.HeadNodeHasChildren(getHeadType()))
                {
                    buildContext();
                    string key = "CHECKER_RES";
                    var collection = _ability.GetVariableCollection();

                    collection.Add(key, new ValueBox<bool>());

                    _executor.ExecuteCycleNode(getHeadType());

                    _checkRes = _checkRes && (bool)collection.GetVariable(key);
                    clearContext();
                    collection.Remove(key);
                }

                return true;
            }

            protected virtual void buildContext() { }

            protected virtual void clearContext() { }
        }

        private class PreAwardChecker : AbilityChecker
        {
            protected override EAbilityCycleType getHeadType()
            {
                return EAbilityCycleType.OnPreAwardCheck;
            }

            public PreAwardChecker(Ability ability) : base(ability) { }
        }
        
        private class PreExecuteCheck : AbilityChecker
        {
            public PreExecuteCheck(Ability ability) : base(ability) { }
            protected override EAbilityCycleType getHeadType()
            {
                return EAbilityCycleType.OnPreExecuteCheck;
            }
        }
    }
}