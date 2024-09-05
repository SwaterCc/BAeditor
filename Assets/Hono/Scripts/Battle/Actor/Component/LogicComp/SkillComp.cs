using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class Skill
        {
            public int Id => Data.ID;
            public SkillData Data;
            public bool IsEnable => (!_isDisable) || (_curCdPercent <= 0 && _resEnough);

            private int _level;
            private float _curCdPercent;
            private int _abilityUid;
            private bool _isDisable;
            private bool _resEnough;
            private float _maxCd;
            private ActorLogic _logic;
            private FilterSetting _skillTargetSetting;

            public Skill(ActorLogic logic, SkillData data)
            {
                _level = 1;
                _curCdPercent = 0;
                _logic = logic;
                Data = data;
                _isDisable = false;

                if (data.SkillTargetType != ESkillTargetType.Self)
                {
                    if (data.UseCustomFilter)
                    {
                        _skillTargetSetting = data.CustomFilter;
                    }
                    else
                    {
                        _skillTargetSetting = new FilterSetting();
                        var boxData = new CheckBoxSphere();
                        boxData.Radius = data.AttackRange;
                        boxData.ShapeType = ECheckBoxShapeType.Sphere;
                        _skillTargetSetting.BoxData = boxData;

                        var range = new FilterRange()
                        {
                            RangeType = EFilterRangeType.Faction,
                        };
                        switch (data.SkillTargetType)
                        {
                            case ESkillTargetType.Enemy:
                                range.Value = (int)EFactionType.Enemy;
                                break;
                            case ESkillTargetType.Friendly:
                                range.Value = (int)EFactionType.Friendly;
                                break;
                        }
                        _skillTargetSetting.Ranges.Add(range);
                    }
                }

                var ability = _logic._abilityController.CreateAbility(data.SkillId);
                _abilityUid = ability.Uid;

                if (data.CostType == EResCostType.AfterExecute)
                {
                    ability.GetCycleCallback(EAbilityAllowEditCycle.OnPreExecute).OnEnter += resourceCost;
                }
                else
                {
                    ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnEnter += resourceCost;
                }

                if (data.EcdMode == ECDMode.AfterExecute)
                {
                    ability.GetCycleCallback(EAbilityAllowEditCycle.OnPreExecute).OnEnter += CdBegin;
                }
                else
                {
                    ability.GetCycleCallback(EAbilityAllowEditCycle.OnEndExecute).OnEnter += CdBegin;
                }

                _logic._abilityController.AwardAbility(ability, false);

                resourceCheck();
            }

            /// <summary>
            /// cd开始
            /// </summary>
            public void CdBegin()
            {
                calculateCd();
                _curCdPercent = 1;
            }

            /// <summary>
            /// 减少cd
            /// </summary>
            public void LessCd(float second)
            {
                var percent = second / _maxCd;
                _curCdPercent -= percent;
            }

            private void calculateCd()
            {
                _maxCd = Data.SkillCD;
            }

            private void resourceCheck()
            {
                foreach (var resItems in Data.SkillResCheck)
                {
                    foreach (var resItem in resItems.Items)
                    {
                        switch (resItem.ResourceType)
                        {
                            case EBattleResourceType.Energy:
                                var mp = _logic.GetAttr<int>((ELogicAttr)resItem.ResId);
                                _resEnough = mp > resItem.Value;
                                break;
                            case EBattleResourceType.Item:
                                break;
                            case EBattleResourceType.Buff:
                                _resEnough = _logic.GetComponent<BuffComp>().GetBuffLayer(resItem.ResId) >
                                             resItem.Value;
                                break;
                        }
                    }
                }
            }

            private void resourceCost()
            {
                foreach (var resItems in Data.SkillResCost)
                {
                    foreach (var resItem in resItems.Items)
                    {
                        switch (resItem.ResourceType)
                        {
                            case EBattleResourceType.Energy:
                                var mp = _logic.GetAttr<int>((ELogicAttr)resItem.ResId);
                                _logic.SetAttr((ELogicAttr)resItem.ResId, mp - resItem.Value, false);
                                break;
                        }
                    }
                }
            }

            public void OnTick(float dt)
            {
                //CD
                if (_curCdPercent > 0)
                {
                    _curCdPercent -= (dt / _maxCd);
                }

                if (!_resEnough)
                {
                    resourceCheck();
                }
            }

            public void OnSkillUsed()
            {
                int targetUid = -1;
                //选敌
                if (Data.SkillTargetType != ESkillTargetType.Self)
                {
                    var targetList = ActorManager.Instance.UseFilter(_logic, _skillTargetSetting);
                    float minDistance = Data.AttackRange;
                    var selfPos = _logic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                  
                    foreach (var uid in targetList)
                    {
                        var target = ActorManager.Instance.GetActor(uid);
                        var targetPos = target.Logic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                        var newMinDis = Vector3.Distance(selfPos, targetPos);
                        if (newMinDis < minDistance)
                        {
                            minDistance = newMinDis;
                            targetUid = uid;
                        }
                    }
                }
                else
                {
                    targetUid = _logic.Uid;
                }
                
                if (targetUid > 0)
                {
                    _logic.SetAttr(ELogicAttr.AttrAttackTargetUid, targetUid, false);
                    _logic._abilityController.ExecutingAbility(_abilityUid);
                    resourceCheck();
                }
                else
                {
                    Debug.Log("技能没有找到目标！");
                }
            }

            public static bool operator <(Skill control1, Skill control2)
            {
                return control1.Data.PriorityATK > control2.Data.PriorityDEF;
            }

            public static bool operator >(Skill control1, Skill control2)
            {
                return control1.Data.PriorityDEF > control2.Data.PriorityATK;
            }
        }

        public class SkillComp : ALogicComponent
        {
            //管理技能cd
            //管理技能释放前消耗检测
            //管理技能消耗资源
            //管理技能释放打断优先级(有问题需要讨论)
            //管理技能升级养成数据
            //技能的内核逻辑依靠Ability
            //Ability可通过该组件获取技能数据
            private Dictionary<int, Skill> _skills = new();

            /// <summary>
            /// 当前在运行的技能
            /// </summary>
            private Skill _curSkill;

            public SkillComp(ActorLogic logic) : base(logic) { }

            public override void Init()
            {
                foreach (var skillId in _actorLogic.LogicData.ownerSkills)
                {
                    var skillCtrl = new Skill(_actorLogic, AssetManager.Instance.GetData<SkillData>(skillId));
                    _skills.Add(skillId, skillCtrl);
                }
            }

            protected override void onTick(float dt)
            {
                foreach (var pSKill in _skills)
                {
                    pSKill.Value.OnTick(dt);
                }
            }

            public void UseSkill(int skillId)
            {
                if (!_skills.TryGetValue(skillId, out var skillState))
                {
                    return;
                }

                if (_curSkill != null && _curSkill > skillState)
                {
                    return;
                }

                if (skillState.IsEnable)
                {
                    _actorLogic._stateMachine.ChangeState(EActorState.Battle);
                    skillState.OnSkillUsed();
                }
            }
        }
    }
}