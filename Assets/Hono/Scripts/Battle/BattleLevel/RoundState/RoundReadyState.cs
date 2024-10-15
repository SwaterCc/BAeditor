using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundReadyState : RoundState
        {
            private bool _createTeam;

            public RoundReadyState(RoundController roundController) : base(roundController)
            {
                _createTeam = true;
            }

            public override ERoundState GetRoundState() => ERoundState.Ready;
            

            protected override void onEnter()
            {
                Round.GameRunningState.BattleGroundHandle.RuntimeInfo.ClearRound();
                Round.GameRunningState.BattleGroundHandle.RuntimeInfo.CurRoundLastTime = -1;
                foreach (var abilityId in CurrentRoundData.ReadyStageAbilityIds)
                {
                    BattleManager.BattleController.RunAbility(abilityId);
                }
                
                //´´½¨¶ÓÎé
                if (_createTeam)
                {
                    Round.GameRunningState.BattleGroundHandle._pawnTeamController.CreatePawnTeams();
                    _createTeam = false;
                }
            }

            protected override void onTick(float dt)
            {
                if (CurrentRoundData.ReadyStageTime >= 0 && Duration > CurrentRoundData.ReadyStageTime)
                {
                    Round.SwitchState(ERoundState.Running);
                }

                if (CurrentRoundData.ReadyStageTime >= 0)
                {
                    Round.GameRunningState.BattleGroundHandle.RuntimeInfo.CurRoundLastTime = CurrentRoundData.ReadyStageTime - Duration;
                }
            }

            protected override void onExit()
            {
                foreach (var abilityId in CurrentRoundData.ReadyStageAbilityIds)
                {
                    BattleManager.BattleController.RemoveAbility(abilityId);
                }
            }
        }
    }
}