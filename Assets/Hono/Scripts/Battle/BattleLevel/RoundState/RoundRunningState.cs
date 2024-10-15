using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private class RoundRunningState : RoundState
        {
            private bool _isEnd;
            private const float CheckInterval = 0.5f;
            private float _checkDt;
            private readonly List<RoundScoreCondition> _finalTimeCheck = new(8);
            private readonly List<RoundScoreCondition> _frameCheck = new(8);
            private bool _fristTick;
            public RoundRunningState(RoundController roundController) : base(roundController) { }
            private readonly MonsterGenEventInfo _monsterGenEventInfo = new();
            public override ERoundState GetRoundState() => ERoundState.Running;

            protected override void onEnter()
            {
                _fristTick = true;
                foreach (var abilityId in CurrentRoundData.RunningAbilityIds)
                {
                    BattleManager.BattleController.RunAbility(abilityId);
                }

                foreach (var condition in CurrentRoundData.SuccessConditions)
                {
                    if (condition.ScoreNow || CurrentRoundData.RunningCheckTime <= 0)
                    {
                        _frameCheck.Add(condition);
                    }
                    else
                    {
                        _finalTimeCheck.Add(condition);
                    }
                }
            }

            //阶段最终检测时长
            //失败条件-即刻结算
            //成功条件-最终时长到达后检测
            //当不限制时间时，所有成功条件都变成即刻结算条件
            protected override void onTick(float dt)
            {
                if(_fristTick)
                    firstTick();
                
                if (_checkDt > CheckInterval)
                {
                    _checkDt = 0;

                    int successCount = 0;
                    foreach (var successCondition in _frameCheck)
                    {
                        if (checkCondition(successCondition))
                        {
                            ++successCount;
                        }
                    }
                    if (successCount == _frameCheck.Count && successCount != 0)
                    {
                        Round.SwitchState(ERoundState.SuccessScoring);
                        return;
                    }

                    
                    foreach (var failedCondition in CurrentRoundData.FailedConditions)
                    {
                        if (checkCondition(failedCondition))
                        {
                            Round.SwitchState(ERoundState.FailedScoring);
                            return;
                        }
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

            private void firstTick()
            {
                foreach (var info in CurrentRoundData.MonsterBuilderLinkInfos)
                {
                    _monsterGenEventInfo.MonsterConfigId = info.ConfigId;
                    _monsterGenEventInfo.SingleUid = info.MonsterBuilderUid;
                    BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.OnCallMonsterGen,_monsterGenEventInfo);
                }

                foreach (var triggerUid in CurrentRoundData.TriggerBoxLinkInfos)
                {
                    if (ActorManager.Instance.TryGetActor(triggerUid, out var actor))
                    {
                        ((TriggerBoxModelController)actor.ModelController).SetActive(true);
                    }
                }
                
                _fristTick = false;
            }
            
            private bool checkCondition(RoundScoreCondition condition)
            {
                var rtInfo = Round.GameRunningState.BattleGroundHandle.RuntimeInfo;
                switch (condition.TargetType)
                {
                    case ERoundTargetType.FactionId:
                        switch (condition.ConditionType)
                        {
                            case ERoundConditionType.Survival:
                                return rtInfo.GetRoundSurvivalFaction(condition.TargetParam) >=
                                       condition.ConditionCount;
                            case ERoundConditionType.Death:
                                return rtInfo.GetRoundDeadFaction(condition.TargetParam) >=
                                       condition.ConditionCount;
                        }
                        break;
                    case ERoundTargetType.SpecialUid:
                        var hasActor = ActorManager.Instance.TryGetActor(condition.TargetParam, out _);
                        switch (condition.ConditionType)
                        {
                            case ERoundConditionType.Survival:
                                return hasActor;
                            case ERoundConditionType.Death:
                                return !hasActor;
                        }
                        break;
                    case ERoundTargetType.Tag:
                        Debug.LogError("Tag还没实现");
                        break;
                }
                return false;
            }

            protected override void onExit()
            {
                foreach (var abilityId in CurrentRoundData.RunningAbilityIds)
                {
                    BattleManager.BattleController.RemoveAbility(abilityId);
                }
                
                foreach (var triggerUid in CurrentRoundData.TriggerBoxLinkInfos)
                {
                    if (ActorManager.Instance.TryGetActor(triggerUid, out var actor))
                    {
                        ((TriggerBoxModelController)actor.ModelController).SetActive(false);
                    }
                }
            }
        }
    }
}