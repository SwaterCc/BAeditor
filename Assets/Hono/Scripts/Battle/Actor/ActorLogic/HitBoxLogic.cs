using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击点目前是每隔一段时间对打击目标结算一次伤害
    /// </summary>
    public class HitBoxLogic : ActorLogic
    {
        private HitBoxData _hitBoxData;
        private float _intervalDuration;
        private int _curCount;

        private int _sourceActorId;
        private int _sourceAbilityId;
        private EAbilityType _sourceAbilityType;
        private Action<ActorLogic, DamageInfo> _hitProcess;
        private FilterSetting _filterSetting;

        private readonly Dictionary<BeHurtComp, int> _hitCountDict = new();

        public HitBoxLogic(int uid, ActorLogicData logicData) : base(uid, logicData)
        {
            _filterSetting = new FilterSetting();
        }

        protected override void initAttrs()
        {
            //设置坐标
            var target = ActorManager.Instance.GetActor(_sourceActorId);
            var pos = target.Logic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            var rot = target.Logic.GetAttr<Vector3>(ELogicAttr.AttrRot);
            SetAttr(ELogicAttr.AttrPosition, pos, false);
            SetAttr(ELogicAttr.AttrRot, rot, false);

        }

        protected override void onInit()
        {
            _intervalDuration = _hitBoxData.Interval;
            _curCount = 0;

            _sourceActorId = GetAttr<int>(ELogicAttr.AttrSourceActorUid);
            _sourceAbilityId = GetAttr<int>(ELogicAttr.AttrSourceAbilityUid);
            _sourceAbilityType = GetAttr<EAbilityType>(ELogicAttr.SourceAbilityType);


            switch (_hitBoxData.HitType)
            {
                case EHitType.Aoe:
                    _hitProcess = aoeHit;
                    break;
                case EHitType.Single:
                    _hitProcess = singleHit;
                    break;
            }


            _filterSetting.BoxData = _hitBoxData.AoeData;
        }

        protected override void registerChildComp() { }

        private void hitCounter(BeHurtComp beHurtComp)
        {
            if (_hitCountDict.TryGetValue(beHurtComp, out var count))
            {
                if (count < _hitBoxData.ValidCount)
                {
                    ++count;
                }
            }
            else
            {
                _hitCountDict.Add(beHurtComp, 1);
            }
        }

        private void singleHit(ActorLogic attacker, DamageInfo damageInfo)
        {
            var targetId = attacker.GetAttr<int>(ELogicAttr.AttrAttackTargetUid);
            var target = ActorManager.Instance.GetActor(targetId);
            if (target == null) return;
            if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp)) return;
            hitCounter(beHurtComp);
            var damageItem = AssetManager.Instance.GetData<DamageItem>(damageInfo.DamageConfigId);
            var res = LuaInterface.GetDamageResults(attacker, target.Logic, damageInfo, damageItem);
            beHurtComp.OnBeHurt(res);
        }

        private void aoeHit(ActorLogic attacker, DamageInfo damageInfo)
        {
            //aoe会根据目标坐标二次筛选
            var targetIds = ActorManager.Instance.UseFilter(this, _filterSetting);

            foreach (var targetUid in targetIds)
            {
                var target = ActorManager.Instance.GetActor(targetUid);
                if (target == null) return;
                if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp)) return;
                hitCounter(beHurtComp);
                var damageItem = AssetManager.Instance.GetData<DamageItem>(damageInfo.DamageConfigId);
                var res = LuaInterface.GetDamageResults(attacker, target.Logic, damageInfo, damageItem);
                beHurtComp.OnBeHurt(res);
            }
        }

        private void onHit()
        {
            var attacker = ActorManager.Instance.GetActor(_sourceActorId).Logic;

            var damageInfo = new DamageInfo();
            damageInfo.DamageConfigId = _hitBoxData.DamageConfigId;
            damageInfo.SourceActorId = _sourceActorId;
            damageInfo.SourceAbilityId = _sourceAbilityId;
            damageInfo.SourceAbilityType = _sourceAbilityType;

            for (int i = 0; i < _hitBoxData.OnceHitDamageCount; i++)
            {
                _hitProcess.Invoke(attacker, damageInfo);
            }
        }

        protected override void onTick(float dt)
        {
            var interval = _curCount == 0 ? _hitBoxData.FirstInterval : _hitBoxData.Interval;

            if (_intervalDuration > interval)
            {
                ++_curCount;
                _intervalDuration = 0;
                onHit();
            }

            _intervalDuration += dt;

            if (_curCount >= _hitBoxData.MaxCount)
            {
                ActorManager.Instance.RemoveActor(this.Uid);
            }
        }

        protected override void onDestroy() { }
    }
}