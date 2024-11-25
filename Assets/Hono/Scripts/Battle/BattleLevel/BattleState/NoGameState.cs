namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public class NoGameState : BattleState
        {
            public NoGameState(BattleGround controller, EBattleStateType stateType) : base(controller,
                stateType) { }

            protected override void onEnter() { }

            protected override void onTick(float dt) { }

            protected override void onExit() { }
        }
    }
}