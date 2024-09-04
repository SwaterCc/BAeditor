namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class PreExecuteCycle : AbilityRunCycle
        {
            public PreExecuteCycle(AbilityState state) : base(state) { }

            protected override EAbilityAllowEditCycle getCycleType() => EAbilityAllowEditCycle.OnPreExecute;

            protected override EAbilityState getCurState() => EAbilityState.PreExecute;

            public override EAbilityState GetNextState() => EAbilityState.Executing;
        }
    }
   
}