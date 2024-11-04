namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public class BuildTeamsState : BattleState
        {
            public BuildTeamsState(BattleGround controller, EBattleStateType stateType) :
                base(controller, stateType) { }

            protected override void onEnter()
            {
                //发消息打开编队UI
                //BattleUIInterface.CallUI<BuildTeamsUIRoot>();
            }

            private void setTeams()
            {
                BattleGroundHandle.switchState(EBattleStateType.LoadBattleGround);
            }

            protected override void onExit() { }
        }
    }
}