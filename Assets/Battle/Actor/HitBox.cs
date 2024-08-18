using System;
using System.Collections.Generic;
using Battle.Event;
using BattleAbility;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle
{
    /// <summary>
    /// 打击盒
    /// </summary>
    public class HitBox : Actor
    {
        private HitData _hitData;

        private float _duration;

        private float _intervalDuration;

        private int _count;

        private AoeHitBoxHandle _hitBoxObject;

        private List<IBeHurt> _beHurtList;
        
        public HitBox(int configId)
        {
            ConfigId = configId;
            _duration = 0f;
            _beHurtList = new List<IBeHurt>();
        }
        
        public override void Init()
        {
            _abilityController = new AbilityController(this);
            //Addresable加载打击点数据
            //...

            _intervalDuration = _hitData.Interval;
        }

        private AoeHitBoxHandle createHitPrefab()
        {
            //创建打击盒
            
            //设置位置偏移旋转缩放

            return null;
        }

        private void getTarget()
        {
            var lockHit =  (LockTargetHitData)_hitData;
            foreach (var actorId in lockHit.TargetIds)
            {
                var actor = BattleManager.Instance.GetActor(actorId);
                _beHurtList.Add(actor);
            }
        }
        
        private void onHit(Dictionary<string, IValueBox> damage)
        {
            foreach (var beHurt in _beHurtList)
            {
                BattleEventManager.Instance.TriggerEvent(EBattleEventType.Hit,new HitEventInfo());
                //调用自身Ability
                beHurt.BeHurt(damage);
            }
        }
        
        public override void Tick(float dt)
        {
            base.Tick(dt);

            //有效时间
            if (_duration >= _hitData.BeginTime && _duration <= _hitData.EndTime)
            {
                _hitBoxObject ??= createHitPrefab();
                
                switch (_hitData.HitType)
                {
                    case EHitType.Aoe:
                        _beHurtList = _hitBoxObject.GetHitList();
                        break;
                    case EHitType.LockTarget:
                        getTarget();
                        break;
                }
                
                if (_intervalDuration >= _hitData.Interval)
                {
                    onHit(_hitData.Damage);
                    _intervalDuration = 0;
                    ++_count;
                }
            }
            
            _intervalDuration += dt;
            _duration += dt;
            
            if(_duration <= _hitData.EndTime || _count > _hitData.ValidCount)
            {
                _isDisposable = true;
            }
        }
    }
}