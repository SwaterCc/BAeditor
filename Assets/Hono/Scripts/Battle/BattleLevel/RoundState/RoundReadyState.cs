#region

#endregion

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundReadyState : RoundState
        {
            private bool _firstEnter;
            private bool _createTeamFlag;

            public RoundReadyState(RoundController roundController) : base(roundController)
            {
                _firstEnter = true;
            }

            public override ERoundState GetRoundState() => ERoundState.Ready;


            protected override void onEnter()
            {
                Round.GameRunningState.BattleGroundHandle.RtInfo.ClearRound();
                foreach (var abilityId in CurrentRoundData.ReadyStageAbilityIds)
                {
                    BattleManager.BattleController.RunAbility(abilityId);
                }

                //创建队伍
                if (_firstEnter)
                {
                    //TeamSelectPanel.Instance.FirstOpen();
                }

                if (Round.GameRunningState.BattleGroundHandle._levelData.ReadyRoundStateSpik && !_firstEnter)
                {
                    Round.SwitchState(ERoundState.Running);
                }

                if (!Round.GameRunningState.BattleGroundHandle._levelData.ReadyRoundStateSpik)
                {
                    //WarDeployPanel.Instance.Show();
                }
            }

            protected override void onTick(float dt) { }

            protected override void onExit()
            {
                foreach (var abilityId in CurrentRoundData.ReadyStageAbilityIds)
                {
                    BattleManager.BattleController.RemoveAbility(abilityId);
                }

                if (!Round.GameRunningState.BattleGroundHandle._levelData.ReadyRoundStateSpik || _firstEnter)
                {
                    Round.GameRunningState.BattleGroundHandle._pawnTeamController.CreatePawnTeams();
                }

                if (!Round.GameRunningState.BattleGroundHandle._levelData.ReadyRoundStateSpik)
                {
                   // WarDeployPanel.Instance.Hide();
                }

                _firstEnter = false;
            }
        }
    }
}