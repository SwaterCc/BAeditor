using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Battle.Auto;
using Battle.Event;
using BattleAbility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Battle
{
    /// <summary>
    /// 打击盒 本质上是伤害信息收集器
    /// </summary>
    public class HitBox : Actor
    {
        private HitBoxConfig _hitBoxConfig;

        private float _duration;

        private float _intervalDuration;

        private int _count;

        private AoeHitBoxHandle _hitBoxObject;

        private List<IBeHurt> _beHurtList;

        private bool _isLoaded;

        /// <summary>
        /// 来源id，所有的来源来自于Ability
        /// </summary>
        private int _source;
        
        /// <summary>
        /// 召唤者id
        /// </summary>
        private int _summerId;

        private int _targetId;

        private ESelectPosType _selectPosType;

       // private EDamageType _damageType;

        public HitBox(int configId, int source, ESelectPosType selectPosType)
        {
            ConfigId = configId;
            _duration = 0f;
            _beHurtList = new List<IBeHurt>();
            _source = source;
            _selectPosType = selectPosType;
        }

        public override void Init()
        {
            onLoad();
        }

        private async void onLoad()
        {
            _isLoaded = false;
             await loadHitData();
             await createHitPrefab();
             _isLoaded = true;
        }
        
        private async UniTask loadHitData()
        {
            _hitBoxConfig = await Addressables
                .LoadAssetAsync<HitBoxConfig>($"Assets/AbilityData/HitBoxConfig/{ConfigId}.asset")
                .ToUniTask();
            _intervalDuration = _hitBoxConfig.Interval;
        }

        private async UniTask createHitPrefab()
        {
            string path = "";
            switch (((AoeHitBoxConfig)_hitBoxConfig).HitAreaType)
            {
                case EHitAreaType.Rect:
                    path = "Assets/HitBox/Cube.prefab";
                    break;
                case EHitAreaType.Sphere:
                    path = "Assets/HitBox/Sphere.prefab";
                    break;
                case EHitAreaType.Cylinder:
                    path = "Assets/HitBox/Sphere.prefab";
                    break;
            }
            
            if(string.IsNullOrEmpty(path)) return;

            //创建打击盒
            var gameObject = await Addressables
                .LoadAssetAsync<GameObject>(path)
                .ToUniTask();

            gameObject = Object.Instantiate(gameObject);
            _hitBoxObject = gameObject.GetComponent<AoeHitBoxHandle>();
            
            Actor actor = null;
            //设置位置偏移旋转缩放
            switch (_selectPosType)
            {
                case ESelectPosType.Self:
                    actor = BattleManager.Instance.GetActor(_source);
                    break;
                case ESelectPosType.Target:
                    actor = BattleManager.Instance.GetActor(_targetId);
                    break;
            }

            if (actor == null) return;

            var actorPos = (Vector3)(actor.GetAttrCollection().GetAttr(EAttributeType.Position).GetBox());
            _hitBoxObject.transform.position = ((AoeHitBoxConfig)_hitBoxConfig).HitAreaData.Offset + actorPos;
        }

        private void getTarget()
        {
            var lockHit = (LockTargetHitBoxConfig)_hitBoxConfig;
            foreach (var actorId in lockHit.TargetIds)
            {
                var actor = BattleManager.Instance.GetActor(actorId);
                _beHurtList.Add(actor);
            }
        }

        private void check()
        {
            foreach (var beHurt in _beHurtList)
            {
                BattleEventManager.Instance.TriggerEvent(EBattleEventType.Hit, new HitEventInfo());
                //调用自身Ability
                //beHurt.BeHurt(damage);
            }
        }

        public override void Tick(float dt)
        {
            base.Tick(dt);

            if(!_isLoaded) return;
            
            //有效时间
            if (_duration >= _hitBoxConfig.DelayTime && _duration <= _hitBoxConfig.DelayTime + _hitBoxConfig.Duration)
            {
                switch (_hitBoxConfig.HitType)
                {
                    case EHitType.Aoe:
                        _beHurtList = _hitBoxObject.GetHitList();
                        break;
                    case EHitType.LockTarget:
                        getTarget();
                        break;
                }

                if (_intervalDuration >= _hitBoxConfig.Interval)
                {
                    check();
                    _intervalDuration = 0;
                    
                }
            }

            _intervalDuration += dt;
            _duration += dt;

            if (_duration >= _hitBoxConfig.Duration + _hitBoxConfig.DelayTime)
            {
                _isDisposable = true;
            }
        }

        public override void OnDestroy()
        {
            Object.Destroy(_hitBoxObject.gameObject);
            base.OnDestroy();
        }
    }
}