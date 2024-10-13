using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundRunningState : RoundState
        {
            private bool _isEnd;
            private const float CheckInterval = 0.5f;
            private float _checkDt;
            private List<RoundScoreCondition> _finalTimeCheck = new();
            private List<RoundScoreCondition> _frameCheck = new();

            public RoundRunningState(RoundController roundController) : base(roundController) { }

            public override ERoundState GetRoundState() => ERoundState.Running;

            protected override void onEnter()
            {
                foreach (var abilityId in CurrentRoundData.RunningAbilityIds)
                {
                    BattleManager.battleController.RunAbility(abilityId);
                }
            }

            protected override void onTick(float dt)
            {
                //阶段最终检测时长
                //失败条件-即刻结算
                //成功条件-最终时长到达后检测
                //当不限制时间时，所有成功条件都变成即刻结算条件
                if (_checkDt > CheckInterval)
                {
                    _checkDt = 0;

                    bool successFlag = true;
                    foreach (var successCondition in CurrentRoundData.SuccessConditions)
                    {
                        successFlag = successFlag && checkCondition(successCondition);
                    }
                    if (successFlag)
                    {
                        Round.SwitchState(ERoundState.SuccessScoring);
                        return;
                    }

                    bool failed = true;
                    foreach (var failedCondition in CurrentRoundData.FailedConditions)
                    {
                        failed = failed && checkCondition(failedCondition);
                    }
                    if (failed)
                    {
                        Round.SwitchState(ERoundState.FailedScoring);
                        return;
                    }
                }

                _checkDt += dt;

                if (CurrentRoundData.RunningCheckTime > 0 && Duration > CurrentRoundData.RunningCheckTime)
                {
                   
                    foreach (var condition in _finalTimeCheck)
                    {
                        if (!checkCondition(condition))
                        {
                            Round.SwitchState(ERoundState.FailedScoring);
                            return;
                        }
                    }
                    Round.SwitchState(ERoundState.SuccessScoring);
                }
            }

            private bool checkCondition(RoundScoreCondition condition)
            {
                switch (condition.ConditionType)
                {
                    
                }
                return false;
            }

            protected override void onExit()
            {
                foreach (var abilityId in CurrentRoundData.RunningAbilityIds)
                {
                    BattleManager.battleController.RemoveAbility(abilityId);
                }
            }
        }
    }
}