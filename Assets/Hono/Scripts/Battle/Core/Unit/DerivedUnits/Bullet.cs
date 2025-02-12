using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;


namespace Hono.Scripts.Battle.Core
{
    public class Bullet : Unit , IGPoolObject
    {
        private BulletData _bulletData;

        private int _hitCount;
        private float _duration;
        
        private Actor _attacker;
        private int _targetUid;
        
        private bool _hasError;
        
        public void Init(BulletData bulletData,)
        {
           
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
        
       

        public override void Recycle()
        {
            GPool<Bullet>.Pool.Recycle(this);
        }
        

        public void OnRecycle()
        {
           
        }
    }
}