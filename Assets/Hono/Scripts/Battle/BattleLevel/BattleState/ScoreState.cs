#region

#endregion

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
                /*if (BattleGroundHandle._isScoreSuccess)
                {
                    LevelFinishPanel.Instance.ShowSuccess();
                }
                else
                {
                    LevelFinishPanel.Instance.ShowFailure();
                }*/

                if ((EBattleModeType)BattleGroundHandle.BattleConfig.BattleType == EBattleModeType.War &&
                    BattleGroundHandle.SaveFileDict != null)
                {
                    BattleGroundHandle.SaveFileDict.BattleSaveFiles.Remove(BattleGroundHandle.BattleGroundConfigId);
                }
            }

            protected override void onExit() { }
        }
    }
}