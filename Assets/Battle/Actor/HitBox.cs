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

        private bool _isLoaded;
        
        /// <summary>
        /// 召唤者
        /// </summary>
        private int _source;

        private int _targetId;

        private ESelectPosType _selectPosType;

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
            _hitData = await Addressables
                .LoadAssetAsync<HitData>($"Assets/AbilityData/HitData/{ConfigId}.asset")
                .ToUniTask();
            _intervalDuration = _hitData.Interval;
        }

        private async UniTask createHitPrefab()
        {
            string path = "";
            switch (((AoeHitData)_hitData).HitAreaType)
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
            _hitBoxObject.transform.position = ((AoeHitData)_hitData).HitAreaData.Offset + actorPos;
        }

        private void getTarget()
        {
            var lockHit = (LockTargetHitData)_hitData;
            foreach (var actorId in lockHit.TargetIds)
            {
                var actor = BattleManager.Instance.GetActor(actorId);
                _beHurtList.Add(actor);
            }
        }

        private void onHit(Dictionary<string, object> damage)
        {
            foreach (var beHurt in _beHurtList)
            {
                BattleEventManager.Instance.TriggerEvent(EBattleEventType.Hit, new HitEventInfo());
                //调用自身Ability
                beHurt.BeHurt(damage);
            }
        }

        public override void Tick(float dt)
        {
            base.Tick(dt);

            if(!_isLoaded) return;
            
            //有效时间
            if (_duration >= _hitData.DelayTime && _duration <= _hitData.DelayTime + _hitData.Duration)
            {
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
                    
                }
            }

            _intervalDuration += dt;
            _duration += dt;

            if (_duration >= _hitData.Duration + _hitData.DelayTime)
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