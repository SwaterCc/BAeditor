#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilityFramework;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core
{
    public partial class CombatComp
    {
        public class Skill : IGPoolObject
        {
            /// <summary>
            /// 技能Id
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// 技能数据
            /// </summary>
            public SkillData SkillData { get; private set; }

            /// <summary>
            /// 技能数据修改器
            /// </summary>
            public SkillModifier Modifier { get; } = new();

            /// <summary>
            /// 技能是否在执行中
            /// </summary>
            public ESkillFlag Flag { get; private set; }

            /// <summary>
            /// 当前CD比例
            /// </summary>
            public float CdPercent { get; private set; }

            /// <summary>
            /// 绑定的Ability
            /// </summary>
            private Ability _ability;
            /// <summary>
            /// 战斗组件
            /// </summary>
            private CombatComp _combatComp;

            /// <summary>
            /// 选中的世界坐标
            /// </summary>
            private Vector3 _selectWorldPos;

            /// <summary>
            /// 选中的单体目标
            /// </summary>
            private Unit _selectSingleTarget;

            /// <summary>
            /// 选中的方向
            /// </summary>
            private float _selectYAxisAngle;
            
            /// <summary>
            /// 范围筛选的所有目标uid
            /// </summary>
            private List<int> _selectUnitInArea = new(30); 

            public void OnRent(CombatComp combatComp, SkillData data)
            {
                SkillData = data;
                CdPercent = 0;
                Flag = 0;

                _ability = combatComp.Unit.AddAbility(SkillData.id);
                _ability.ExecuteEndCallBack += onAbilityEnd;

                if (!_combatComp.checkSkillResourceEnough(this))
                {
                    AddFlag(ESkillFlag.EnergyNotEnough);
                }
            }

            /// <summary>
            /// 执行技能
            /// </summary>
            public void Play()
            {
                _ability.Execute(false);
                AddFlag(ESkillFlag.Executing);
                if (SkillData.enterCdType == EEnterCDType.BeforeExecute)
                {
                    CdBegin();
                }

                _combatComp.costEnergy(this);
            }

            private void onAbilityEnd()
            {
                RemoveFlag(ESkillFlag.Executing);
                if (SkillData.enterCdType == EEnterCDType.AfterExecute)
                {
                    CdBegin();
                }

                _selectSingleTarget = null;
                _selectWorldPos = Vector3.zero;
                _selectYAxisAngle = 0;
                _selectUnitInArea.Clear();
            }

            public void AddFlag(ESkillFlag flag)
            {
                Flag |= flag;
            }

            public bool HasFlag(ESkillFlag flag)
            {
                return (Flag & flag) == flag;
            }

            public void RemoveFlag(ESkillFlag flag)
            {
                Flag = (Flag & ~flag);
            }

            #region CD

            /// <summary>
            /// cd开始
            /// </summary>
            private void CdBegin()
            {
                CdPercent = 1;
                AddFlag(ESkillFlag.CD);
            }

            /// <summary>
            /// CD结束时调用
            /// </summary>
            private void CdEnd()
            {
                CdPercent = 0;
                RemoveFlag(ESkillFlag.CD);
            }

            /// <summary>
            ///  减少cd
            /// </summary>
            public void LessCd(float second)
            {
                var percent = second / SkillData.skillCd;
                CdPercent -= percent;
                CdPercent = Math.Clamp(CdPercent, 0, 1);
            }

            #endregion

            public void OnTick(float dt)
            {
                //CD
                if (CdPercent > 0)
                {
                    var finalCdPct = ((_combatComp.Unit.GetAttr(EAttrType.AttrSkillCDPCT) +
                                       Modifier.GetAttrModify(EAttrType.AttrSkillCDPCT)) / 10000f + 1);
                    if (finalCdPct < 0) finalCdPct = 0;
                    CdPercent -= ((dt / SkillData.skillCd) * finalCdPct);

                    if (CdPercent <= 0)
                    {
                        CdEnd();
                    }
                }

                //能量检测
                if (HasFlag(ESkillFlag.EnergyNotEnough))
                {
                    if (_combatComp.checkSkillResourceEnough(this))
                    {
                        RemoveFlag(ESkillFlag.EnergyNotEnough);
                    }
                }

                //更新攻速
                var attackSpeed =
                    (_combatComp.Unit.GetAttr(EAttrType.AttrAttackSpeedPCT) +
                     Modifier.GetAttrModify(EAttrType.AttrAttackSpeedPCT)) / 10000f + 1;
                attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, 10);
                _ability.TimeScaleFactory = attackSpeed;
            }

            public void OnRecycle()
            {
                Id = 0;
                SkillData = null;
                CdPercent = 0;
                Flag = 0;
                _combatComp = null;
                _ability.Stop();
                _ability = null;
            }
        }
    }
}