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
                    //ǿ�Ƽ��һ��
                    foreach (var successCondition in _round.RoundData.SuccessConditions)
                    {
                        switch (successCondition.ConditionType)
                        {
                            //��Ĺ�ϵ
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
                //�׶����ռ��ʱ��
                //ʧ������-���̽���
                //�ɹ�����-����ʱ���������
                //��������ʱ��ʱ�����гɹ���������ɼ��̽�������
                bool successFlag = false;
                foreach (var successCondition in _round.RoundData.SuccessConditions)
                {
                    //���Ǽ����������������м��ʱ��
                    if (successCondition.Flag && _round.RoundData.RunningCheckTime >= 0) continue;
                    switch (successCondition.ConditionType)
                    {
                        //��Ĺ�ϵ
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
                        //��Ĺ�ϵ
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