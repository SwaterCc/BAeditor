namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class EndExecuteCycle : AbilityRunCycle
        {
            public EndExecuteCycle(AbilityState state) : base(state) { }
            protected override EAbilityAllowEditCycle getCycleType() => EAbilityAllowEditCycle.OnEndExecute;

            protected override EAbilityState getCurState() => EAbilityState.EndExecute;

            public override EAbilityState GetNextState() => EAbilityState.Ready;

            protected override void onExit()
            {
                _ability._variables.Clear();
			
                //指令撤销
                foreach (var command in _ability._commands) {
                    command.Undo();
                }
            }
        }
    }
}