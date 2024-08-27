using System.Collections.Generic;
using Battle.Damage;
using Battle.GamePlay;
using Battle.Tools;
using BattleAbility;
using UnityEngine;

namespace Battle
{
    public class HitBoxLogic : ActorLogic
    {
        private HitBoxConfig _hitBoxConfig;

        private float _duration;

        private float _intervalDuration;

        private int _count;


        public HitBoxLogic(int configId) : base(configId) { }

        protected override void onInit()
        {
            _duration = 0;
            _intervalDuration = 0;
            _count = 0;
        }

        protected override void registerChildComp() { }


        private void hit()
        {
            List<int> targets = null;
            switch (_hitBoxConfig.HitType)
            {
                case EHitType.Aoe:
                    targets = CommonUtility.CheckAABB(_hitBoxConfig.AoeData, Vector3.one);
                    break;
                case EHitType.LockTarget:
                    break;
            }

            if (targets == null) return;

            var damageInfo = new DamageInfo();
            var source = this.GetSimpleAttr<int>(EAttributeType.Source).GetValue();
            var attacker = ActorManager.Instance.GetActor(source).ActorLogic;
            foreach (var target in targets)
            {
                var targetLogic = ActorManager.Instance.GetActor(target).ActorLogic;
                var res = DamageLuaInterface.GetDamageResults(attacker, targetLogic, damageInfo);
                targetLogic.GetComponent<BeHurtComp>().OnBeHurt(res);
            }
        }

        protected override void onTick(float dt)
        {
            //有效时间
            if (_duration >= _hitBoxConfig.DelayTime && _duration <= _hitBoxConfig.DelayTime + _hitBoxConfig.Duration)
            {
                if (_intervalDuration >= _hitBoxConfig.Interval)
                {
                    _intervalDuration = 0;
                    hit();
                }
            }

            _intervalDuration += dt;
            _duration += dt;

            if (_duration >= _hitBoxConfig.Duration + _hitBoxConfig.DelayTime) { }
        }
    }
}