#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Event;

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    ///     打击点目前是每隔一段时间对打击目标结算一次伤害
    /// </summary>
    public class HitBoxLogic : ActorLogic, IAPoolObject
    {
        private HitBoxData _hitBoxData;

        private Actor _attacker;
        private Actor _target;

        private float _intervalDuration;
        private int _curCount;

        private bool _isExpire;
        private readonly Damage _damage;
        private FilterSetting _filterSetting;
        private List<int> _aoeTargetIds = new(32);
        private readonly Dictionary<BeHurtComp, int> _hitCountDict = new(16);
        private readonly List<BeHurtComp> _hurtComps = new(32);

        public HitBoxLogic()
        {
            _damage = new Damage(Self);
        }

        protected override void onInit()
        {
            _hitBoxData = (HitBoxData)(Variables.Get("hitBoxData"));
            //因为是同帧，所以攻击者必然存在
            _attacker = ActorManager.Instance.GetActor(GetAttr(EAttrType.AttrSourceActorUid));
            _attacker.ExitSceneCallBack += setHitBoxExpire;
            //打击目标
            var targetUid = (int)(Variables.Get("targetUid"));
            if (targetUid != 0)
            {
                _target = ActorManager.Instance.GetActor(targetUid);
            }

            if (_target != null)
            {
                _target.ExitSceneCallBack += setHitBoxExpire;
            }

            _isExpire = _target == null;

            if (!_isExpire)
            {
                _damage.Init(_attacker, _target, _hitBoxData.DamageConfigId);
            }
        }

        protected override void onEnterScene()
        {
            _curCount = 0;
            _intervalDuration = _hitBoxData.Interval;
            _filterSetting = _hitBoxData.FilterSetting;
            Self.Pos = _target.Pos;
            Self.Rot = _attacker.Rot;
        }

        protected override void onTick(float dt)
        {
            if (_isExpire) return;

            var interval = _curCount == 0 ? _hitBoxData.FirstInterval : _hitBoxData.Interval;

            if (_intervalDuration > interval)
            {
                ++_curCount;
                _intervalDuration = 0;
                onHit();
            }

            _intervalDuration += dt;

            if (_curCount >= _hitBoxData.MaxCount)
            {
                ActorManager.Instance.RemoveActor(Uid);
            }
        }

        private void onHit()
        {
            for (int i = 0; i < _hitBoxData.OnceHitDamageCount; i++)
            {
                switch (_hitBoxData.HitType)
                {
                    case EHitType.Aoe:
                        aoeHit();
                        break;
                    case EHitType.Single:
                        singleHit();
                        break;
                }
            }
        }

        private void setHitBoxExpire(Actor actor)
        {
            _isExpire = true;
            ActorManager.Instance.RemoveActor(Uid);
        }

        private void singleHit()
        {
            if (!_target.Logic.TryGetComponent(out BeHurtComp beHurtComp)) return;
            hitCounter(beHurtComp);
            var hitInfo = _damage.MakeDamage(1, _hitCountDict[beHurtComp], _hitBoxData.CriticalFlag);
            BattleEventManager.Instance.TriggerActorEvent(_attacker.Uid, EBattleEventType.OnHit, hitInfo);
            beHurtComp.OnBeHurt(hitInfo);
        }

        private void aoeHit()
        {
            //aoe会根据目标坐标二次筛选
            ActorManager.Instance.UseFilter(Self, _filterSetting, ref _aoeTargetIds);

            if (_aoeTargetIds.Count == 0)
            {
                ActorManager.Instance.RemoveActor(this.Uid);
                return;
            }

            foreach (var targetUid in _aoeTargetIds)
            {
                var target = ActorManager.Instance.GetActor(targetUid);
                if (target == null)
                    continue;
                if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp))
                    continue;
                _hurtComps.Add(beHurtComp);
            }

            foreach (var beHurtComp in _hurtComps)
            {
                hitCounter(beHurtComp);
                var hitInfo = _damage.MakeDamage(_hurtComps.Count, _hitCountDict[beHurtComp], _hitBoxData.CriticalFlag);
                BattleEventManager.Instance.TriggerActorEvent(_attacker.Uid, EBattleEventType.OnHit, hitInfo);
                beHurtComp.OnBeHurt(hitInfo);
            }
        }

        private void hitCounter(BeHurtComp beHurtComp)
        {
            if (_hitCountDict.TryGetValue(beHurtComp, out var count))
            {
                if (count < _hitBoxData.ValidCount)
                {
                    ++count;
                }
            }
            else
            {
                _hitCountDict.Add(beHurtComp, 1);
            }
        }

        public override void RecycleLogicObject()
        {
            APool<HitBoxLogic>.Pool.Recycle(this);
        }

        protected override void OnChildRecycle()
        {
            _hitBoxData = null;

            _attacker = null;
            _target = null;

            _intervalDuration = 0;
            _curCount = 0;

            _isExpire = false;

            _filterSetting = null;
            _aoeTargetIds.Clear();
            _hitCountDict.Clear();
            _hurtComps.Clear();
            _damage.Clear();
        }
    }
}