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
            public bool IsExecuting { get; private set; }

            /// <summary>
            /// 技能是否被禁用
            /// </summary>
            public bool IsDisable;


            /// <summary>
            /// 真实CD，受技能等级，属性影响
            /// </summary>
            private float _realCd;

            /// <summary>
            /// 当前CD比例
            /// </summary>
            public float CdPercent { get; private set; }

            /// <summary>
            /// 战斗资源是否足够释放技能
            /// </summary>
            private bool _resEnough;

            /// <summary>
            /// 绑定的Ability
            /// </summary>
            private Ability _ability;

            /// <summary>
            /// 战斗组件
            /// </summary>
            private CombatComp _combatComp;

            public void OnRent(CombatComp combatComp, SkillData data)
            {
                SkillData = data;
                CdPercent = 0;
                IsDisable = false;
                IsExecuting = false;
                _ability = combatComp.Unit.AddAbility(SkillData.abilityId);
                _ability.ExecuteEndCallBack += onAbilityEnd;
            }

            /// <summary>
            /// 执行技能
            /// </summary>
            public void Play()
            {
                _ability.Execute(false);
                IsExecuting = true;
                if (SkillData.enterCdType == EEnterCdType.BeforeExecute)
                {
                    CdBegin();
                }

                _combatComp.costResource(SkillData);
            }

            private void onAbilityEnd()
            {
                IsExecuting = false;

                if (SkillData.enterCdType == EEnterCdType.AfterExecute)
                {
                    CdBegin();
                }
            }

            /// <summary>
            /// 后续改成UI跳字
            /// </summary>
            public void SendFailedMsg()
            {
                if (IsDisable)
                {
                    Debug.Log("被禁止使用");
                }

                if (!_resEnough)
                {
                    Debug.Log("能量不足");
                }

                if (CdPercent != 0)
                {
                    Debug.Log("Cd中！");
                }

                if (IsExecuting)
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
                CdPercent = 1;
            }

            /// <summary>
            /// CD结束时调用
            /// </summary>
            private void CdEnd()
            {
                CdPercent = 0;
            }

            /// <summary>
            ///  减少cd
            /// </summary>
            public void LessCd(float second)
            {
                var percent = second / _realCd;

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
                    CdPercent -= ((dt / _realCd) * finalCdPct);

                    if (CdPercent <= 0)
                    {
                        CdEnd();
                    }
                }

                //更新攻速
                var attackSpeed =
                    (_combatComp.Unit.GetAttr(EAttrType.AttrAttackSpeedPCT) +
                     Modifier.GetAttrModify(EAttrType.AttrAttackSpeedPCT)) / 10000f + 1;
                attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, 10);
                _ability.TimeScaleFactory = attackSpeed;
            }

            public void SetDisable(bool value)
            {
                IsDisable = value;
            }

            public void OnRecycle()
            {
                Id = 0;
                SkillData = null;
                _combatComp = null;
                IsDisable = false;
                IsExecuting = false;
                CdPercent = 0;
                _resEnough = false;
                _realCd = 0;
                _ability.Stop();
                _ability = null;
            }
        }
    }
}