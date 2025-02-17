using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;


namespace Hono.Scripts.Battle.Core
{
    //锁目标 直线
    //方向  直线
    public class Bullet : Unit, IGPoolObject
    {
        private enum EBulletType
        {
            NoInit = 0,
            LockTargetBullet,
            DirectionBullet,
        }

        /// <summary>
        /// 攻击者
        /// </summary>
        private Unit _attacker;
        /// <summary>
        /// 子弹数据
        /// </summary>
        private BulletData _bulletData;
        /// <summary>
        /// 当前命中次数
        /// </summary>
        private int _curHitNumber;
        /// <summary>
        /// 累计存活时间
        /// </summary>
        private float _duration;
        /// <summary>
        /// 命中目标的damageId
        /// </summary>
        private int _hitTargetDamageId;
        /// <summary>
        /// 命中非目标的damageId
        /// </summary>
        private int _hitNotTargetDamageId;
        /// <summary>
        /// 锁定对象
        /// </summary>
        private Unit _target;
        /// <summary>
        /// 目标坐标
        /// </summary>
        private Vector3 _targetPos;
        /// <summary>
        /// 子弹类型
        /// </summary>
        private EBulletType _bulletType;
        /// <summary>
        /// 检测盒子
        /// </summary>
        private CheckBoxData _checkBox;
        /// <summary>
        /// 最大命中数量
        /// </summary>
        private List<int> _maxHitResul = new(50);
        /// <summary>
        /// 碰撞cd
        /// </summary>
        private float _hitCountdown;
        /// <summary>
        /// 伤害来源类型
        /// </summary>
        private EDamageSourceType _damageSourceType;
        
        
        /// <summary>
        /// 锁定目标的子弹
        /// </summary>
        public void LockTargetBullet(Unit attacker,
            Unit target,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int hitTargetDamageId,
            int hitNotTargetDamageId = 0)
        {
            Uid = World.Current.GetUid();
            _attacker = attacker;
            _target = target;
            _bulletData = bulletData;
            _hitTargetDamageId = hitTargetDamageId;
            _hitNotTargetDamageId = _hitNotTargetDamageId;
            _attacker.RecycleCallBack += onAttackerRemove;
            _target.AfterTickCallBack += onTargetTick;
            _target.RecycleCallBack += onTargetRemove;
            _bulletType = EBulletType.LockTargetBullet;
            _hitCountdown = 0;
            AddAbility(_bulletData.BulletAbility);
            _checkBox = new CheckBoxData()
            {
                ShapeType = ECheckBoxShapeType.Sphere,
                Radius = _bulletData.hitRadius,
            };
        }

        /// <summary>
        /// 向指定方向飞行的子弹
        /// </summary>
        public void DirectionBullet(Unit attacker,
            float yAxisAngle,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int hitTargetDamageId,
            int hitNotTargetDamageId)
        {
            Uid = World.Current.GetUid();
            _attacker = attacker;
            _bulletData = bulletData;
            _hitTargetDamageId = hitTargetDamageId;
            _hitNotTargetDamageId = _hitNotTargetDamageId;
            UnitTransform.Rot = Quaternion.AngleAxis(yAxisAngle, Vector3.up);
            _bulletType = EBulletType.DirectionBullet;
            _hitCountdown = 0;
            AddAbility(_bulletData.BulletAbility);
        }

        protected override void onBeforeFirstTick()
        {
            //设置初始速度
            SetAttr(EAttrType.AttrMoveSpeedPCTAdd, (int)(_bulletData.speed * 10000));
            SetAttr(EAttrType.AttrRotSpeedPCTAdd,  (int)(_bulletData.rotSpeed * 10000));
            ExecuteAbility(_bulletData.id);
        }

        protected override void onTick(float dt)
        {
            _duration += dt;

            if (_hitCountdown > 0)
            {
                _hitCountdown -= dt;
            }

            if (_attacker == null)
            {
                return;
            }

            move(dt);

            if (_duration > _bulletData.lifeTime)
            {
                World.Current.RemoveUnit(this);
            }
        }

        private void move(float dt)
        {
            switch (_bulletType)
            {
                case EBulletType.LockTargetBullet:
                    //获取目标方向
                    var dir = (_targetPos - UnitTransform.Pos).normalized;

                    //转向角度
                    var angle = Vector3.SignedAngle(UnitTransform.Forward, dir, Vector3.up) * dt;
                    var rotSpeed = (GetAttr(EAttrType.AttrRotSpeedPCT) / 10000f) * dt;
                    float realRotAngle = rotSpeed < 0 ? angle : Mathf.Min(rotSpeed, angle);

                    //获取移动方向
                    UnitTransform.Rot = Quaternion.AngleAxis(realRotAngle, Vector3.up) * UnitTransform.Rot;
                    var moveDt = UnitTransform.Forward * ((GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f) * dt);
                    UnitTransform.Pos += moveDt;

                    if (_target == null) return;

                    if (!_bulletData.ignoreAllNotTarget)
                    {
                        //判定命中
                        checkCollision();
                    }

                    var range = _bulletData.hitRadius + _target.UnitTransform.Radius;
                    if (Vector3.Distance(_targetPos, UnitTransform.Pos) < range)
                    {
                        //和目标的距离小于半径和
                        HitSystem.Instance.SingleHit(_attacker, _target, _damageSourceType, _bulletData.id, false,
                                                     _hitTargetDamageId);
                    }

                    break;
                case EBulletType.DirectionBullet:
                    UnitTransform.Pos += UnitTransform.Forward * ((GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f) * dt);

                    if (!_bulletData.ignoreAllNotTarget)
                    {
                        checkCollision();
                    }

                    break;
            }
        }

        private void checkCollision()
        {
            if (CommonUtility.HitRayCast(_checkBox, UnitTransform.Pos, UnitTransform.Rot, ref _maxHitResul))
            {
                foreach (var uid in _maxHitResul)
                {
                    if (uid == Uid)
                    {
                        continue;
                    }

                    if (!World.Query.ConditionFilter(_attacker, uid, _bulletData.notTargetHitCondition))
                    {
                        return;
                    }

                    if (_hitCountdown > 0)
                    {
                        return;
                    }

                    var target = World.Query.GetUnit(uid);
                    HitSystem.Instance.SingleHit(_attacker, target, _damageSourceType, _bulletData.id, false,
                                                 _hitNotTargetDamageId);
                    _hitCountdown = _bulletData.minHitInterval;
                    ++_curHitNumber;
                    if (_curHitNumber > _bulletData.maxHitCount)
                    {
                        World.Current.RemoveUnit(this);
                    }
                }
            }
        }

        private void onAttackerRemove(Unit _)
        {
            _attacker = null;
        }

        private void onTargetTick(Unit target, float dt)
        {
            _targetPos = target.UnitTransform.Pos;
        }

        private void onTargetRemove(Unit _)
        {
            _target = null;
        }

        public override void Recycle()
        {
            GPool<Bullet>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            _attacker.RecycleCallBack -= onAttackerRemove;
            _target.AfterTickCallBack -= onTargetTick;
            _target.RecycleCallBack -= onTargetRemove;

            _attacker = default;
            _bulletData = default;
            _curHitNumber = default;
            _duration = default;
            _hitTargetDamageId = default;
            _hitNotTargetDamageId = default;
            _target = default;
            _targetPos = default;
            _bulletType = default;
            _hitCountdown = default;
            _maxHitResul.Clear();
        }
    }
}