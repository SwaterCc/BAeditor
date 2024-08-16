using System;
using System.Collections.Generic;
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

        private GameObject _hitBoxObject;

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
            //加载打击点数据
            //...
        }

        private GameObject createHitPrefab()
        {
            //创建打击盒
            
            //设置位置偏移旋转缩放

            return null;
        }

        private void getTarget()
        {
            
        }
        
        public override void Tick(float dt)
        {
            base.Tick(dt);

            //有效时间
            if (_duration >= _hitData.BeginTime && _duration <= _hitData.EndTime)
            {
                switch (_hitData.HitType)
                {
                    case EHitType.Aoe:
                        _hitBoxObject ??= createHitPrefab();
                        break;
                    case EHitType.LockTarget:
                        getTarget();
                        break;
                }
                
                

                
            }
            
            _intervalDuration += dt;
            _duration += dt;
            
            if(_duration <= _hitData.EndTime)
            {
                _isDisposable = true;
            }
        }
    }
}