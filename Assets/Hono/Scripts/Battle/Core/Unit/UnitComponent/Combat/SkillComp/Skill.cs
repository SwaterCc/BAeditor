#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
      public class Skill : ICPoolObject
        {
            /// <summary>
            /// 技能Id
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// 技能配置
            /// </summary>
            public SkillData SkillData { get; private set; }

            /// <summary>
            /// logic
            /// </summary>
            public Unit Logic { get; private set; }

            /// <summary>
            /// 技能是否被禁用
            /// </summary>
            private bool _isDisable;

            /// <summary>
            /// 技能是否在执行中
            /// </summary>
            private bool _isExecuting;

            public bool IsExecuting => _isExecuting;

            /// <summary>
            /// 技能是否能够释放
            /// </summary>
            public bool IsEnable => (!_isDisable) && (_curCdPercent <= 0) && (!_isExecuting);

            /// <summary>
            /// 技能等级
            /// </summary>
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

            /// <summary>
            /// 当前CD比例
            /// </summary>
            private float _curCdPercent;

            /// <summary>
            /// 战斗资源是否足够释放技能
            /// </summary>
            private bool _resEnough;

            /// <summary>
            /// 真实CD，受技能等级，属性影响
            /// </summary>
            private float _realCd;

            /// <summary>
            /// cd回调
            /// </summary>
            private Action<float> _cdTick;

            /// <summary>
            /// 废弃，后续采用索敌组件
            /// </summary>
            private RangeFilterSetting _skillTargetSetting;

            /// <summary>
            /// cd数据
            /// </summary>
            //private readonly SkillCdEventInfo _cdEventInfo = new();

            /// <summary>
            /// 技能使用Info
            /// </summary>
            //private readonly UsedSkillEventInfo _usedSkillEventInfo = new();

            /// <summary>
            /// 技能的Ability
            /// </summary>
            public Ability Ability { get; private set; }

            /// <summary>
            /// 技能的目标
            /// </summary>
            public List<int> SkillTargetIdList;

            public void OnRent(Unit logic, int skillId, int level)
            {
                SkillData = AssetManager.Instance.GetData<SkillData>(skillId);

                //读技能等级信息没实现

                Logic = logic;
                _curCdPercent = 0;
                _isDisable = false;
                _isExecuting = false;

               

                Ability = Logic.AddAbility(Id);
                Ability.AddCycleCallback(EAbilityCycle.PreExecute, onAbilityBegin);
                Ability.AddCycleCallback(EAbilityCycle.EndExecute, onAbilityEnd);
                if (SkillData.skillType != ESkillType.PassiveSkill)
                {
                    Ability.Execute();
                }


                Level = level;

                checkResource();
            }

            private void onAbilityBegin()
            {
                _isExecuting = true;
                if (SkillData.skillType != ESkillType.PassiveSkill && SkillData.skillType != ESkillType.EffectSkill)
                {
                    //Logic._stateMachine.SwitchState(EActorStateType.Attack);
                }

                if (SkillData.costTimingType == EResCostTimingType.BeforeExecute)
                {
                    costResource();
                }

                if (SkillData.enterCdType == EEnterCdType.BeforeExecute)
                {
                    CdBegin();
                }

                //_usedSkillEventInfo.SkillId = Ability.Id;
                //_usedSkillEventInfo.UserUid = Logic.Uid;

               //
            }

            private void onAbilityEnd()
            {
                _isExecuting = false;

                if (SkillData.costTimingType == EResCostTimingType.AfterExecute)
                {
                    costResource();
                }

                if (SkillData.enterCdType == EEnterCdType.AfterExecute)
                {
                    CdBegin();
                }

                if (SkillData.skillType != ESkillType.PassiveSkill && SkillData.skillType != ESkillType.EffectSkill)
                {
                    //Logic._stateMachine.SwitchState(EActorStateType.Idle);
                }

             //
            }

            /// <summary>
            /// 后续改成UI跳字
            /// </summary>
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

            #region CD

            /// <summary>
            /// 计算真实CD
            /// </summary>
            private void calculateCd()
            {
               // _realCd = SkillData.skillCD;
            }

            /// <summary>
            /// cd开始
            /// </summary>
            private void CdBegin()
            {
                calculateCd();
                _curCdPercent = 1;
                //EventManager.Instance.TriggerActorEvent(Logic.Uid, EEventType.SkillCDBegin, _cdEventInfo);
            }

            /// <summary>
            /// 添加CDTick回调
            /// </summary>
            /// <param name="tickCallBack"></param>
            private void AddCdTick(Action<float> tickCallBack)
            {
                _cdTick += tickCallBack;
            }

            /// <summary>
            /// CD结束时调用
            /// </summary>
            private void CdEnd()
            {
                _curCdPercent = 0;
                //EventManager.Instance.TriggerActorEvent(Logic.Uid, EEventType.SkillCDEnd, _cdEventInfo);
                _cdTick = null;
            }

            /// <summary>
            ///  减少cd
            /// </summary>
            public void LessCd(float second)
            {
                var percent = second / _realCd;

                _curCdPercent -= percent;
                _curCdPercent = Math.Clamp(_curCdPercent, 0, 1);
            }

            #endregion

            #region 资源检测与消耗

            /// <summary>
            /// 资源检测
            /// </summary>
            private void checkResource()
            {
                if (SkillData.skillResCheck.Count == 0)
                {
                    _resEnough = true;
                    return;
                }

                foreach (var resItems in SkillData.skillResCheck)
                {
                    foreach (var resItem in resItems.Items)
                    {
                        switch (resItem.ResourceType)
                        {
                            case EBattleResourceType.Energy:
                                var mp = Logic.GetAttr((EAttrType)resItem.ResId);
                                _resEnough = mp >= resItem.Value;
                                break;
                            case EBattleResourceType.Item:
                                break;
                            case EBattleResourceType.Buff:
                                _resEnough = Logic.GetComponent<BuffComp>().GetBuffLayer(resItem.ResId) >=
                                             resItem.Value;
                                break;
                        }
                    }
                }
            }

            /// <summary>
            /// 资源消耗
            /// </summary>
            private void costResource()
            {
                foreach (var resItems in SkillData.skillResCost)
                {
                    //组检测
                    foreach (var resItem in resItems.Items)
                    {
                        //单项检测
                        switch (resItem.ResourceType)
                        {
                            case EBattleResourceType.Energy:
                                var mp = Logic.GetAttr((EAttrType)resItem.ResId);
                                Logic.SetAttr((EAttrType)resItem.ResId, mp - resItem.Value, false);
                                break;
                        }
                    }
                }

                checkResource();
            }

            #endregion

            public void OnTick(float dt)
            {
                //CD
                if (_curCdPercent > 0)
                {
                    var finalCdPct = (Logic.GetAttr(EAttrType.AttrSkillCDPCT) / 10000f + 1);
                    if (finalCdPct < 0) finalCdPct = 0;
                    _curCdPercent -= ((dt / _realCd) * finalCdPct);
                    _cdTick?.Invoke(_curCdPercent);
                    if (_curCdPercent <= 0)
                    {
                        CdEnd();
                    }
                }

                //更新攻速
                var attackSpeed = Logic.GetAttr(EAttrType.AttrAttackSpeedPCT) / 10000f + 1;
                attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, 10);
                Ability.TimeScaleFactory = attackSpeed;
            }

            public bool TryUseSkill()
            {
                checkResource();

                if (!_resEnough) return false;

                if (!IsEnable) return false;

                var targetUids = new List<int>();

                /*//选敌
                if (!SkillData.SelectSelf)
                {
                    ActorManager.Instance.UseFilter(Logic.Self, _skillTargetSetting, ref targetUids);
                }
                else
                {
                    targetUids.Add(Logic.Uid);
                }*/

                if (targetUids.Count > 0)
                {
                    Debug.Log($"[UseSkill] Actor{Logic.Uid} -->执行了技能 {Ability.Id}");
                    Ability.Execute();

                    /*if (SkillData.ForceFaceTarget)
                    {
                        if (ActorManager.Instance.TryGetActor(targetUids[0], out var target))
                        {
                            var dir = (target.Pos - Logic.Self.Pos).normalized;
                            dir.y = 0;
                            Logic.Self.Rot = Quaternion.FromToRotation(Vector3.forward, dir);
                        }
                    }*/

                    checkResource();
                }
                else
                {
                    //Debug.Log("技能没有找到目标！");
                    return false;
                }

                return true;
            }

            public void SetDisable(bool value)
            {
                _isDisable = value;
            }
            
            public void OnRecycle()
            {
                Id = 0;
                SkillData = null;
                Logic = null;
                _isDisable = false;
                _isExecuting = false;
                _level = 0;
                _curCdPercent = 0;
                _resEnough = false;
                _realCd = 0;
                _cdTick = null;
                _skillTargetSetting = null;
              

                Ability.Stop();
                Ability = null;
            }
        }
}