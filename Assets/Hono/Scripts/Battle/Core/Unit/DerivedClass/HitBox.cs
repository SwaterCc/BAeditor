using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 脱手打击盒
    /// </summary>
    public class HitBox : Unit, IGPoolObject
    {
        private enum HitBoxType
        {
            NoInit = 0,
            LockTargetSingleHit,
            LockTargetAreaHit,
            HitArea
        }

        /// <summary>
        /// 攻击者
        /// </summary>
        private Unit _attacker;
        /// <summary>
        /// 已经持续的时间
        /// </summary>
        private float _duration;
        /// <summary>
        /// 来源类型
        /// </summary>
        private EDamageSourceType _sourceType;
        /// <summary>
        /// 来源能力的Id
        /// </summary>
        private int _sourceAbilityId;
        /// <summary>
        /// 最大命中次数
        /// </summary>
        private int _maxHitNumber;
        /// <summary>
        /// 当前打击检测次数
        /// </summary>
        private int _curHitNumber;
        /// <summary>
        /// 启动延迟
        /// </summary>
        private float _delayTime;
        /// <summary>
        /// 检测间隔
        /// </summary>
        private float _interval;
        /// <summary>
        /// 禁用事件发送
        /// </summary>
        private bool _disableEventTrigger;
        /// <summary>
        /// 伤害Id
        /// </summary>
        private int _damageId;
        /// <summary>
        /// 锁定打击目标
        /// </summary>
        private Unit _target;
        /// <summary>
        /// aoe打击数据
        /// </summary>
        private RangeFilterSetting _aoeSetting;
        /// <summary>
        /// 打击盒子类型
        /// </summary>
        private HitBoxType _hitBoxType;

        /// <summary>
        /// 锁定目标产生单体打击
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="damageSourceType"></param>
        /// <param name="sourceAbilityId"></param>
        /// <param name="maxHitNumber"></param>
        /// <param name="delayTime"></param>
        /// <param name="interval"></param>
        /// <param name="disableEventTrigger"></param>
        /// <param name="damageId"></param>
        public void LockTargetSingleHit(Unit attacker,
            Unit target,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int maxHitNumber,
            float delayTime,
            float interval,
            bool disableEventTrigger = false,
            int damageId = 0)
        {
            Uid = World.Current.GetUid();
            _hitBoxType = HitBoxType.LockTargetSingleHit;
            _attacker = attacker;
            _duration = 0;
            _target = target;
            _sourceType = damageSourceType;
            _sourceAbilityId = sourceAbilityId;
            _maxHitNumber = maxHitNumber;
            _delayTime = delayTime;
            _interval = interval;
            _disableEventTrigger = disableEventTrigger;
            _damageId = damageId;
            _attacker.RecycleCallBack += onAttackerRecycle;
            _target.RecycleCallBack += onTargetRecycle;
        }

        /// <summary>
        /// 锁定目标产生AOE打击
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="damageSourceType"></param>
        /// <param name="sourceAbilityId"></param>
        /// <param name="maxHitNumber"></param>
        /// <param name="aoeSetting"></param>
        /// <param name="delayTime"></param>
        /// <param name="interval"></param>
        /// <param name="disableEventTrigger"></param>
        /// <param name="damageId"></param>
        public void LockTargetAreaHit(Unit attacker,
            Unit target,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int maxHitNumber,
            RangeFilterSetting aoeSetting,
            float delayTime,
            float interval,
            bool disableEventTrigger = false,
            int damageId = 0)
        {
            Uid = World.Current.GetUid();
            _hitBoxType = HitBoxType.LockTargetAreaHit;
            _attacker = attacker;
            _duration = 0;
            _target = target;
            _aoeSetting = aoeSetting;
            _sourceType = damageSourceType;
            _sourceAbilityId = sourceAbilityId;
            _maxHitNumber = maxHitNumber;
            _delayTime = delayTime;
            _interval = interval;
            _disableEventTrigger = disableEventTrigger;
            _damageId = damageId;
            _attacker.RecycleCallBack += onAttackerRecycle;
            _target.RecycleCallBack += onTargetRecycle;
        }

        /// <summary>
        /// 攻击区域
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="worldPos"></param>
        /// <param name="yAngle"></param>
        /// <param name="damageSourceType"></param>
        /// <param name="sourceAbilityId"></param>
        /// <param name="maxHitNumber"></param>
        /// <param name="aoeSetting"></param>
        /// <param name="delayTime"></param>
        /// <param name="interval"></param>
        /// <param name="disableEventTrigger"></param>
        /// <param name="damageId"></param>
        public void HitArea(Unit attacker,
            Vector3 worldPos,
            float yAngle,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int maxHitNumber,
            RangeFilterSetting aoeSetting,
            float delayTime,
            float interval,
            bool disableEventTrigger = false,
            int damageId = 0)
        {
            _hitBoxType = HitBoxType.HitArea;
            _attacker = attacker;
            _duration = 0;
            _aoeSetting = aoeSetting;
            _sourceType = damageSourceType;
            _sourceAbilityId = sourceAbilityId;
            _maxHitNumber = maxHitNumber;
            _delayTime = delayTime;
            _interval = interval;
            _disableEventTrigger = disableEventTrigger;
            _damageId = damageId;
            UnitTransform.Pos = worldPos;
            UnitTransform.Rot = Quaternion.AngleAxis(yAngle, Vector3.up);
            _attacker.RecycleCallBack += onAttackerRecycle;
        }

        protected override void onTick(float dt)
        {
            _duration += dt;

            if (_curHitNumber <= 0)
            {
                if (_duration > _delayTime)
                {
                    Hit();
                }
            }
            else
            {
                if (_duration > _interval)
                {
                    Hit();
                }
            }

            if (_curHitNumber >= _maxHitNumber)
            {
                World.Current.RemoveUnit(this);
            }
        }

        private void Hit()
        {
            _duration = 0;
            ++_curHitNumber;
            if (_attacker == null)
            {
                return;
            }

            switch (_hitBoxType)
            {
                case HitBoxType.LockTargetSingleHit:
                    if (_target == null) return;
                    HitSystem.Instance.SingleHit(_attacker, _target, _sourceType, _sourceAbilityId,
                                                 _disableEventTrigger, _damageId);
                    break;
                case HitBoxType.LockTargetAreaHit:
                    if (_target == null) return;
                    HitSystem.Instance.AreaHit(_attacker, _target.UnitTransform.Pos, _target.UnitTransform.YAxisAngle,
                                               _sourceType,
                                               _sourceAbilityId, _aoeSetting, _disableEventTrigger, _damageId);
                    break;
                case HitBoxType.HitArea:
                    HitSystem.Instance.AreaHit(_attacker, UnitTransform.Pos, UnitTransform.YAxisAngle, _sourceType,
                                               _sourceAbilityId,
                                               _aoeSetting, _disableEventTrigger, _damageId);
                    break;
            }
        }

        private void onAttackerRecycle(Unit _)
        {
            _attacker = null;
        }

        private void onTargetRecycle(Unit _)
        {
            _target = null;
        }

        public override void Recycle()
        {
            GPool<HitBox>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            if (_attacker != null)
            {
                _attacker.RecycleCallBack -= onAttackerRecycle;
            }

            if (_target != null)
            {
                _target.RecycleCallBack -= onTargetRecycle;
            }

            _attacker = null;
            _duration = 0;
            _sourceType = 0;
            _sourceAbilityId = 0;
            _maxHitNumber = 0;
            _curHitNumber = 0;
            _delayTime = 0;
            _interval = 0;
            _disableEventTrigger = false;
            _damageId = 0;
            _target = null;
            _aoeSetting = null;
            _hitBoxType = 0;
        }
    }
}