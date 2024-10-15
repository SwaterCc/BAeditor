using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    //当前游戏模式
    public class BulletLogic : ActorLogic
    {
        private BulletData _bulletData;
        private int _targetUid;
        private Vector3 _targetPos;
        
        private int _sourceActorId;

        private MotionComp _motionComp;
        private VFXComp _vfxComp;

        private int _hitCount;
        private float _duration;
        
        public BulletLogic(Actor actor) : base(actor) { }

        protected override void setupAttrs()
        {
            SetAttr(ELogicAttr.AttrUnselectable, 1, false);
        }

        protected override void onInit()
        {
            _sourceActorId = GetAttr<int>(ELogicAttr.AttrSourceActorUid);
            _bulletData = AssetManager.Instance.GetData<BulletData>(GetAttr<int>(ELogicAttr.AttrConfigId));

            var summoner = ActorManager.Instance.GetActor(_sourceActorId);
            _targetUid = (int)(Variables.Get("targetUid"));
           
            var summonerPos = summoner.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            var summonerRot = summoner.GetAttr<Quaternion>(ELogicAttr.AttrRot);

            var startPos = summonerPos + summonerRot * _bulletData.Offset;

            SetAttr(ELogicAttr.AttrPosition, startPos, false);
            
            var motionSetting = new MotionSetting()
            {
                MoveType = _bulletData.MotionType,
                Speed = _bulletData.BulletSpeed,
                Duration = _bulletData.BulletLifeTime,
                TriggerEventClose = true
            };

            _motionComp.AddMotion(_targetUid, motionSetting, onBulletCollision);
           
            AbilityController.AwardAbility(_bulletData.Id, true);
        }

        protected override void setupComponents()
        {
            _motionComp = new MotionComp(this);
            _vfxComp = new VFXComp(this);
            addComponent(_motionComp);
            addComponent(_vfxComp);
        }

        private void dead()
        {
            ActorManager.Instance.RemoveActor(Actor.Uid);
        }
        
        protected override void onTick(float dt)
        {
            _duration += dt;
            if (_duration > _bulletData.BulletLifeTime)
            {
                dead();
            }
        }
        
        private HitInfo makeHitInfo() {
            var hitInfo = new HitInfo();
            hitInfo.SourceActorId = _sourceActorId;
            hitInfo.SourceAbilityConfigId = _bulletData.Id;
            hitInfo.SourceAbilityType = (int)EAbilityType.Bullet;
            hitInfo.SourceAbilityUId = _bulletData.Id;
            hitInfo.DamageConfigId = _bulletData.DamageConfigId;
            return hitInfo;
        }

        private DamageConfig makeDamageConfig() {
            var damageConfig = new DamageConfig();
            var damage = ConfigManager.Table<DamageTable>().Get(_bulletData.DamageConfigId);
            damageConfig.DamageType = (EDamageType)damage.DamageType;
            damageConfig.ElementType = (EDamageElementType)damage.ElementType;
            damageConfig.FormulaName = damage.FormulaName;
            damageConfig.ImpactValue = damage.ImpactValue;
            damageConfig.DamageRatio = damage.DamageRatio;
            foreach (var addiId in damage.AdditiveId) {
                var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
                var funcInfo = new DamageFuncInfo();
                funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
                funcInfo.ConditionIds = damageAddi.ConditionIds;
                funcInfo.ConditionParams = damageAddi.ConditionParams;
                funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
                damageConfig.AddiTypes.Add(funcInfo);
            }

            foreach (var multiId in damage.MultiplyId) {
                var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
                var funcInfo = new DamageFuncInfo();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                damageConfig.MultiTypes.Add(funcInfo);
            }

            return damageConfig;
        }
        
        private void onHit(int targetUid) {
            var attacker = ActorManager.Instance.GetActor(_sourceActorId);

            var damageInfo = new DamageInfo();
            damageInfo.SourceAbilityConfigId = _bulletData.Id;
            damageInfo.SourceAbilityType = EAbilityType.Bullet;
            var tags = (List<int>)Variables.Get("abilityTags");
            if (tags != null) {
                damageInfo.Tags.AddRange(tags);
            }
            else {
                Debug.Log("tag == null");
            }
            
            var target = ActorManager.Instance.GetActor(targetUid);
            if (target == null) return;
            if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp)) return;
           
            var damageItem = makeDamageConfig();
            var res = LuaInterface.GetDamageResults(attacker, target, damageInfo, damageItem);
            var hitInfo = makeHitInfo();
            BattleEventManager.Instance.TriggerActorEvent(Actor.Uid, EBattleEventType.OnHit, hitInfo);
			
            var hitDamageInfo = new HitDamageInfo(hitInfo);
            hitDamageInfo.HitTargetUid = _targetUid;
            hitInfo.HitBoxHitCount = 1;
            hitDamageInfo.IsKillTarget = (target.GetAttr<int>(ELogicAttr.AttrHp) - res.DamageValue) <= 0;
            BattleEventManager.Instance.TriggerActorEvent(Actor.Uid, EBattleEventType.OnHitDamage, hitDamageInfo);
			
            beHurtComp.OnBeHurt(hitDamageInfo);
        }

        private void onBulletCollision(int uid)
        {
            if (!ActorManager.Instance.CheckActorPassFilter(Actor, uid, _bulletData.FilterSetting))
            {
                return;
            }
            
            if (_bulletData.IsHitPathActor)
            {
                ++_hitCount;
                onHit(uid);
                
                if (_hitCount >= _bulletData.MaxHitCount || uid == _targetUid)
                {
                    dead();
                }
            }
            else {
	            if (uid != _targetUid) return;
            
	            onHit(_targetUid);
	            dead();
            }
        }
    }
}