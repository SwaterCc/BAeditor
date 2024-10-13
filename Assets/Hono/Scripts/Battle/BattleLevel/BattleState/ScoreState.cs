using UnityEngine.SceneManagement;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public class ScoreState : BattleState
        {
            public ScoreState(BattleGround battleGroundHandle, EBattleStateType stateType) : base(battleGroundHandle,
                stateType) { }

            protected override void onEnter()
            {
                if (BattleGroundHandle._isScoreSuccess)
                {
                    BattleUIInterface.CallUI("battleScoreSuccessUI", null, onUIClose);
                }
                else
                {
                    BattleUIInterface.CallUI("battleScoreFailedUI", null, onUIClose);
                }
            }

            private void onUIClose(IUIPassData data)
            {
                BattleGroundHandle.switchState(EBattleStateType.NoGaming);
                BattleManager.Instance.PopBattleGround();
            }

            protected override void onExit() { }
        }
    }
}