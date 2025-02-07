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
       
        private bool _isExpire;
        private bool _hasError;
        private BulletSetting _setting;

        private void setBulletExpire(Actor actor)
        {
            _isExpire = true;
          
        }

        public void Init()
        {
           
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

        protected override void OnRemove()
        {
            
        }

        private void onHit(int targetUid)
        {
        }

        private void dead()
        {
       
        }
    }
}