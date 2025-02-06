using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;


namespace Hono.Scripts.Battle.Core
{
    public class Bullet : Unit
    {
        private BulletData _bulletData;

        private int _hitCount;
        private float _duration;

        private DamageManager _damageManager;
        private Actor _attacker;
        private int _targetUid;
        private Vector3 _targetPos;
        private bool _isExpire;
        private bool _hasError;
        private BulletSetting _setting;

        public Bullet()
        {
            _motionComp = addComponent(new Battle.Unit.MotionComp(this));
            _vfxComp = addComponent(new Battle.Unit.VFXComp(this));
        }

        private void setBulletExpire(Actor actor)
        {
            _isExpire = true;
            Battle.UnitManager.Instance.RemoveActor(Uid);
        }

        public void Init()
        {
            _bulletData = AssetManager.Instance.GetData<BulletData>(Self.GetAttr(EAttrType.AttrConfigId));
            _targetUid = (Variables.Get<RefInt>("targetUid"));
            _attacker = Battle.UnitManager.Instance.GetActor(Self.GetAttr(EAttrType.AttrSourceActorUid));
            _attacker.ExitSceneCallBack += setBulletExpire;

            Actor target = Battle.UnitManager.Instance.GetActor(_targetUid);
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
            var target = Battle.UnitManager.Instance.GetActor(targetUid);
            if (target == null)
                return;

            if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp))
                return;

            var hitDamageInfo = _damageManager.MakeDamage(1, 1, false);

            var vb = GPool<VariableBoard>.Pool.Rent();
            vb.InitByHitDamageInfo(hitDamageInfo);
            EventManager.Instance.FireWorldEvent(EEventType.OnHit, Self.Uid, vb);
            GPool<VariableBoard>.Pool.Recycle(vb);

            beHurtComp.OnBeHurt(hitDamageInfo);
        }

        private void dead()
        {
            Battle.UnitManager.Instance.RemoveActor(Self.Uid);
        }

        public override void Recycle()
        {
            GPool<BulletLogic>.Pool.Recycle(this);
        }
    }
}