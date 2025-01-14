#region

using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    //当前游戏模式
    public class BulletLogic : ActorLogic, IAPoolObject
    {
        private BulletData _bulletData;

        private MotionComp _motionComp;
        private VFXComp _vfxComp;

        private int _hitCount;
        private float _duration;

        private Damage _damage;
        private Actor _attacker;
        private int _targetUid;
        private Vector3 _targetPos;
        private bool _isExpire;
        private bool _hasError;
        private BulletSetting _setting;

        public BulletLogic()
        {
            _motionComp = addComponent(new MotionComp(this));
            _vfxComp = addComponent(new VFXComp(this));
        }

        private void setBulletExpire(Actor actor)
        {
            _isExpire = true;
            ActorManager.Instance.RemoveActor(Uid);
        }

        protected override void onInit()
        {
            _bulletData = AssetManager.Instance.GetData<BulletData>(GetAttr(EAttrType.AttrConfigId));
            _targetUid = (Variables.Get<RefInt>("targetUid"));
            _attacker = ActorManager.Instance.GetActor(GetAttr(EAttrType.AttrSourceActorUid));
            _attacker.ExitSceneCallBack += setBulletExpire;

            Actor target = ActorManager.Instance.GetActor(_targetUid);
            if (target == null)
            {
                _hasError = true;
                setBulletExpire(null);
            }
            else
            {
                target.ExitSceneCallBack += setBulletExpire;
                _targetPos = target.Pos;
            }
        }

        protected override void onEnterScene()
        {
            Self.Rot = _attacker.Rot * Quaternion.AngleAxis(_setting.Angle, Vector3.up);
            Self.Pos = _attacker.Pos + Self.Rot * _setting.Offset;

            var motionSetting = new MotionSetting()
            {
                MoveType = _bulletData.MotionType,
                Speed = _setting.Speed,
                Duration = _bulletData.BulletLifeTime,
                MovingFaceToTarget = true,
                TriggerEventClose = true
            };

            _motionComp.AddMotion(_targetUid, motionSetting, onBulletCollision);
            var ability = Self.Abilities.AwardAbility(_bulletData.id);
            ability.Execute();
        }

        private void onBulletCollision(int uid)
        {
            if (_hasError || _isExpire)
            {
                return;
            }

            /*if (!ActorManager.Instance.CheckActorPassFilter(Self, uid, _bulletData.rangeFilterSetting))
            {
                return;
            }*/

            if (_bulletData.IsHitPathActor)
            {
                ++_hitCount;
                onHit(uid);

                if (_hitCount >= _bulletData.MaxHitCount || uid == _targetUid)
                {
                    dead();
                }
            }
            else
            {
                if (uid != _targetUid) return;

                onHit(_targetUid);
                dead();
            }
        }
        

        protected override void onTick(float dt)
        {
            if (_hasError || _isExpire)
            {
                return;
            }

            _duration += dt;
            if (_duration > _bulletData.BulletLifeTime)
            {
                dead();
            }
        }

        private void onHit(int targetUid)
        {
            var target = ActorManager.Instance.GetActor(targetUid);
            if (target == null) 
                return;
            
            if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp)) 
                return;
            
            var hitDamageInfo = _damage.MakeDamage(1, 1, false);
            
            var vb = APool<VariableBoard>.Pool.Rent();
            vb.InitByHitDamageInfo(hitDamageInfo);
            EventManager.Instance.FireEvent(EEventType.OnHit, Self.Uid, vb);
            APool<VariableBoard>.Pool.Recycle(vb);

            beHurtComp.OnBeHurt(hitDamageInfo);
        }

        private void dead()
        {
            ActorManager.Instance.RemoveActor(Self.Uid);
        }

        public override void RecycleLogicObject()
        {
            APool<BulletLogic>.Pool.Recycle(this);
        }
    }
}