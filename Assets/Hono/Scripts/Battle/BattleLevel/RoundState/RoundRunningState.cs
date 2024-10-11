using System;

namespace Hono.Scripts.Battle
{
    public partial class BattleLevelController
    {
        private class RoundRunningState : RoundState
        {
            private bool _isEnd;
            private ERoundState _nextState;

            public RoundRunningState(Round round) : base(round)
            {
                _nextState = ERoundState.SuccessScoring;
            }

            public override ERoundState GetRoundState() => ERoundState.Running;

            public override ERoundState GetNextState()
            {
                return _nextState;
            }

            public override bool IsAutoEnd()
            {
                if (_round.RoundData.RunningCheckTime > 0 && _stateDuration > _round.RoundData.RunningCheckTime)
                {
                    bool successFlag = false;
                    //强制检测一次
                    foreach (var successCondition in _round.RoundData.SuccessConditions)
                    {
                        switch (successCondition.ConditionType)
                        {
                            //或的关系
                            case ERoundConditionType.SpecialFactionIdAllKilled:
                                break;
                            case ERoundConditionType.SpecialFactionIdKilledCount:
                                break;
                            case ERoundConditionType.SpecialFactionIdAliveAll:
                                break;
                            case ERoundConditionType.SpecialFactionIdAliveCount:
                                break;
                        }
                    }

                    _isEnd = true;
                    _nextState = successFlag ? ERoundState.SuccessScoring : ERoundState.FailedScoring;
                }

                return _isEnd;
            }

            protected override void onEnter()
            {
                _isEnd = false;
            }

            protected override void onTick(float dt)
            {
                //阶段最终检测时长
                //失败条件-即刻结算
                //成功条件-最终时长到达后检测
                //当不限制时间时，所有成功条件都变成即刻结算条件
                bool successFlag = false;
                foreach (var successCondition in _round.RoundData.SuccessConditions)
                {
                    //不是即刻条件则跳过且有检测时间
                    if (successCondition.Flag && _round.RoundData.RunningCheckTime >= 0) continue;
                    switch (successCondition.ConditionType)
                    {
                        //或的关系
                        case ERoundConditionType.SpecialFactionIdAllKilled:
                            break;
                        case ERoundConditionType.SpecialFactionIdKilledCount:
                            break;
                        case ERoundConditionType.SpecialFactionIdAliveAll:
                            break;
                        case ERoundConditionType.SpecialFactionIdAliveCount:
                            break;
                    }
                }

                if (successFlag)
                {
                    _isEnd = true;
                    _nextState = ERoundState.SuccessScoring;
                }


                bool failed = false;
                foreach (var failedCondition in _round.RoundData.FailedConditions)
                {
                    switch (failedCondition.ConditionType)
                    {
                        //或的关系
                        case ERoundConditionType.SpecialFactionIdAllKilled:
                            break;
                        case ERoundConditionType.SpecialFactionIdKilledCount:
                            break;
                        case ERoundConditionType.SpecialFactionIdAliveAll:
                            break;
                        case ERoundConditionType.SpecialFactionIdAliveCount:
                            break;
                    }
                }

                if (failed)
                {
                    _isEnd = true;
                    _nextState = ERoundState.FailedScoring;
                }
            }

            protected override void onExit()
            {
                _isEnd = false;
            }
        }
    }
}