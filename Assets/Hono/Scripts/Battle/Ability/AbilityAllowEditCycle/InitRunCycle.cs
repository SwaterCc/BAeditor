namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class OnInitCycle : AbilityRunCycle
        {
            public OnInitCycle(AbilityState state) : base(state) { }

            protected override EAbilityAllowEditCycle getCycleType()
            {
                return EAbilityAllowEditCycle.OnInit;
            }

            protected override EAbilityState getCurState() => EAbilityState.Init;

            protected override void onEnter()
            {
                //注册事件
                _executor.RegisterEventNode();
            }

            public override EAbilityState GetNextState()
            {
                return EAbilityState.Ready;
            }
        }

    }
}