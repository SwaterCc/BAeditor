#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using UnityEngine;
using Ability = Hono.Scripts.Battle.AbilityFramework.Ability;

#endregion

namespace Hono.Scripts.Battle.Core
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
        /// 技能是否能够释放
        /// </summary>
        public bool IsEnable => (!_isDisable) && (_curCdPercent <= 0) && (!IsExecuting);
        
        /// <summary>
        /// 技能是否被禁用
        /// </summary>
        private bool _isDisable;
        
        /// <summary>
        /// 真实CD，受技能等级，属性影响
        /// </summary>
        private float _realCd;
        
        /// <summary>
        /// 当前CD比例
        /// </summary>
        private float _curCdPercent;

        /// <summary>
        /// 战斗资源是否足够释放技能
        /// </summary>
        private bool _resEnough;
        
        public void OnRent(Ability logic, SkillData data)
        {
            SkillData = data;
            //读技能等级信息没实现
            _curCdPercent = 0;
            _isDisable = false;
            IsExecuting = false;
            logic.ExecuteEndCallBack += onAbilityEnd;
            checkResource();
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
            costResource();
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
            _curCdPercent = 1;
        }
        
        /// <summary>
        /// CD结束时调用
        /// </summary>
        private void CdEnd()
        {
            _curCdPercent = 0;
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
                            var mp = Unit.GetAttr((EAttrType)resItem.ResId);
                            _resEnough = mp >= resItem.Value;
                            break;
                        case EBattleResourceType.Item:
                            break;
                        case EBattleResourceType.Buff:
                            _resEnough = Unit.GetComponent<BuffCollection>().GetBuffLayer(resItem.ResId) >=
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
                            var mp = Unit.GetAttr((EAttrType)resItem.ResId);
                            Unit.SetAttr((EAttrType)resItem.ResId, mp - resItem.Value, false);
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
                var finalCdPct = (Unit.GetAttr(EAttrType.AttrSkillCDPCT) / 10000f + 1);
                if (finalCdPct < 0) finalCdPct = 0;
                _curCdPercent -= ((dt / _realCd) * finalCdPct);
                _cdTick?.Invoke(_curCdPercent);
                if (_curCdPercent <= 0)
                {
                    CdEnd();
                }
            }

            //更新攻速
            var attackSpeed = Unit.GetAttr(EAttrType.AttrAttackSpeedPCT) / 10000f + 1;
            attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, 10);
            Ability.TimeScaleFactory = attackSpeed;
        }

        public bool TryUseSkill()
        {
            checkResource();

            if (!_resEnough) return false;

            if (!IsEnable) return false;

            var targetUids = new List<int>();

           
            if (targetUids.Count > 0)
            {
                Debug.Log($"[UseSkill] Actor{Unit.Uid} -->执行了技能 {Ability.Id}");
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
            Unit = null;
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