using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundReadyState : RoundState
        {
            public RoundReadyState(RoundController roundController) : base(roundController) { }

            public override ERoundState GetRoundState() => ERoundState.Ready;
            

            protected override void onEnter()
            {
                foreach (var abilityId in CurrentRoundData.ReadyStageAbilityIds)
                {
                    BattleManager.battleController.RunAbility(abilityId);
                }
            }

            protected override void onTick(float dt)
            {
                if (CurrentRoundData.ReadyStageTime >= 0 && Duration > CurrentRoundData.ReadyStageTime)
                {
                    Round.SwitchState(ERoundState.Running);
                }
            }

            protected override void onExit()
            {
                foreach (var abilityId in CurrentRoundData.ReadyStageAbilityIds)
                {
                    BattleManager.battleController.RemoveAbility(abilityId);
                }
            }
        }
    }
}