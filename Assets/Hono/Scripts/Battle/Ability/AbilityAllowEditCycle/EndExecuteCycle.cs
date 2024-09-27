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
                //_ability.Variables.Clear();
			
                //Ö¸Áî³·Ïú
                /*foreach (var command in _ability._commands) {
                    command.Undo();
                }*/
            }
        }
    }
}