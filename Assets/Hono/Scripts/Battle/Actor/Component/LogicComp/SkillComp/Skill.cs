#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class Skill
        {
            public int Id => Data.ID;
            public SkillData Data;
            public bool IsEnable => (!_isDisable) && (_curCdPercent <= 0) && (!_isExecuting);

            public bool IsExecuting => _isExecuting;

            private int _level;

            public int Level
            {
                get => _level;

                set
                {
                    _level = value;
                    if (_level <= 0)
                    {
                        Debug.LogWarning($"Skill{Id} level被减到0 强行设置为1");
                        _level = 1;
                    }

                    if (_level > 10)
                    {
                        Debug.LogWarning($"Skill{Id} level超过10 强行设置为10");
                        _level = 10;
                    }
                }
            }

            private float _curCdPercent;
            private int _abilityUid;
            private bool _isDisable;
            private bool _resEnough;
            private bool _isExecuting;
            private float _maxCd;
            private ActorLogic _logic;
            private FilterSetting _skillTargetSetting;
            private SkillCdEventInfo _cdEventInfo;

            private Action<float> _cdTick;

            public Skill(ActorLogic logic, int skillId, int level)
            {
                Data = AssetManager.Instance.GetData<SkillData>(skillId);
                _curCdPercent = 0;
                _logic = logic;
                _isDisable = false;
                _isExecuting = false;

                _skillTargetSetting = Data.CustomFilter;

                var ability = _logic._abilityController.CreateAbility(Data.SkillId);
                _abilityUid = ability.Uid;

                ability.GetCycleCallback(EAbilityAllowEditCycle.OnPreExecute).OnEnter += onAbilityBegin;
                ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnExit += onAbilityEnd;

                _logic._abilityController.AwardAbility(ability, Data.SkillType == ESkillType.PassiveSkill);

                _cdEventInfo = new SkillCdEventInfo()
                {
                    SkillId = Id, SkillBelongActorUid = _logic.Uid, AddCdTickFunc = AddCdTick
                };

                Level = level;

                resourceCheck();
            }

            private void onAbilityBegin()
            {
                _isExecuting = true;
                if (Data.SkillType != ESkillType.PassiveSkill && Data.SkillType != ESkillType.RogueSkill)
                {
                    _logic._stateMachine.SwitchState(EActorLogicStateType.Skill);
                }

                if (Data.CostType == EResCostType.BeforeExecute)
                {
                    resourceCost();
                }

                if (Data.EcdMode == ECDMode.BeforeExecute)
                {
                    CdBegin();
                }

                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnSkillUseSuccess,
                    new UsedSkillEventInfo() { SkillId = _abilityUid, CasterUid = _logic.Uid });
            }

            public void ForceStop()
            {
                _logic._abilityController.ForceStopAbility(_abilityUid);
            }

            private void onAbilityEnd()
            {
                _isExecuting = false;

                if (Data.CostType == EResCostType.AfterExecute)
                {
                    resourceCost();
                }

                if (Data.EcdMode == ECDMode.AfterExecute)
                {
                    CdBegin();
                }

                if (Data.SkillType != ESkillType.PassiveSkill && Data.SkillType != ESkillType.RogueSkill)
                {
                    _logic._stateMachine.SwitchState(EActorLogicStateType.Idle);
                }

                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnSkillStop,
                    new UsedSkillEventInfo() { SkillId = _abilityUid, CasterUid = _logic.Uid });
            }

            /// <summary>
            ///     cd开始
            /// </summary>
            private void CdBegin()
            {
                calculateCd();
                _curCdPercent = 1;
                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.SkillCDBegin, _cdEventInfo);
            }

            private void AddCdTick(Action<float> tickCallBack)
            {
                _cdTick += tickCallBack;
            }

            private void CdEnd()
            {
                _curCdPercent = 0;
                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.SkillCDEnd, _cdEventInfo);
                _cdTick = null;
            }

            public void SendFailedMsg()
            {
                if (_isDisable)
                {
                    Debug.Log("被禁止使用");
                }

                if (!_resEnough)
                {
                    Debug.Log("能量不足");
                }

                if (_curCdPercent != 0)
                {
                    Debug.Log("Cd中！");
                }

                if (_isExecuting)
                {
                    Debug.Log("技能释放中！");
                }
            }

            /// <summary>
            ///     减少cd
            /// </summary>
            public void LessCd(float second)
            {
                var percent = second / _maxCd;

                _curCdPercent -= percent;
                _curCdPercent = Math.Clamp(_curCdPercent, 0, 1);
            }

            private void calculateCd()
            {
                _maxCd = Data.SkillCD;
            }

            private void resourceCheck()
            {
                if (Data.SkillResCheck.Count == 0)
                {
                    _resEnough = true;
                    return;
                }

                foreach (var resItems in Data.SkillResCheck)
                {
                    foreach (var resItem in resItems.Items)
                    {
                        switch (resItem.ResourceType)
                        {
                            case EBattleResourceType.Energy:
                                var mp = _logic.GetAttr<int>((ELogicAttr)resItem.ResId);
                                _resEnough = mp >= resItem.Value;
                                break;
                            case EBattleResourceType.Item:
                                break;
                            case EBattleResourceType.Buff:
                                _resEnough = _logic.GetComponent<BuffComp>().GetBuffLayer(resItem.ResId) >=
                                             resItem.Value;
                                break;
                        }
                    }
                }
            }

            private void resourceCost()
            {
                if (Data.SkillResCheck.Count == 0)
                {
                    _resEnough = true;
                    return;
                }

                foreach (var resItems in Data.SkillResCost)
                {
                    //组检测
                    foreach (var resItem in resItems.Items)
                    {
                        //单项检测
                        switch (resItem.ResourceType)
                        {
                            case EBattleResourceType.Energy:
                                var mp = _logic.GetAttr<int>((ELogicAttr)resItem.ResId);
                                _logic.SetAttr((ELogicAttr)resItem.ResId, mp - resItem.Value, false);
                                break;
                        }
                    }
                }

                resourceCheck();
            }


            public void OnTick(float dt)
            {
                //CD
                if (_curCdPercent > 0)
                {
                    var finalCDPct = (_logic.GetAttr<int>(ELogicAttr.AttrSkillCDPCT) / 10000f + 1);
                    if (finalCDPct < 0) finalCDPct = 0;
                    _curCdPercent -= ((dt / _maxCd) * finalCDPct);
                    _cdTick?.Invoke(_curCdPercent);
                    if (_curCdPercent <= 0)
                    {
                        CdEnd();
                    }
                }

                //更新攻速
                var attackSpeed = _logic.GetAttr<int>(ELogicAttr.AttrAttackSpeedPCT) / 10000f + 1;
                attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, 10);
                if (_logic.AbilityController.TryGetAbility(_abilityUid, out var ability))
                {
                    ability.TimeScaleFactory = attackSpeed;
                }
            }

            public bool TryUseSkill()
            {
                resourceCheck();

                if (!_resEnough) return false;

                if (!IsEnable) return false;

                var targetUids = new List<int>();

                //选敌
                if (!Data.SelectSelf)
                {
                    targetUids.AddRange(ActorManager.Instance.UseFilter(_logic.Actor, _skillTargetSetting));
                }
                else
                {
                    targetUids.Add(_logic.Uid);
                }

                if (targetUids.Count > 0)
                {
                    _logic.SetAttr(ELogicAttr.AttrAttackTargetUids, targetUids, false);
                    Debug.Log($"[UseSkill] Actor{_logic.Uid} -->执行了技能 {_abilityUid}");
                    _logic._abilityController.ExecutingAbility(_abilityUid);

                    if (Data.ForceFaceTarget)
                    {
                        if (ActorManager.Instance.TryGetActor(targetUids[0], out var target))
                        {
                            var dir = (target.Pos - _logic.Actor.Pos).normalized;
                            dir.y = 0;
                            _logic.SetAttr(ELogicAttr.AttrRot, Quaternion.FromToRotation(Vector3.forward, dir), false);
                        }
                    }

                    resourceCheck();
                }
                else
                {
                    //Debug.Log("技能没有找到目标！");
                    return false;
                }

                return true;
            }

            public static bool operator <(Skill control1, Skill control2)
            {
                return control1.Data.PriorityATK > control2.Data.PriorityDEF;
            }

            public static bool operator >(Skill control1, Skill control2)
            {
                return control1.Data.PriorityDEF > control2.Data.PriorityATK;
            }

            public void Destroy()
            {
                _logic._abilityController.RemoveAbility(_abilityUid);
            }
        }
    }
}