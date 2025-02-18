using System;
using System.Collections.Generic;
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
        /// 特效组件
        /// </summary>
        private VFXComp _vfxComp;
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
        private List<int> _maxHitResult = new(50);
        /// <summary>
        /// 碰撞cd
        /// </summary>
        private float _hitCountdown;

        /// <summary>
        /// 伤害来源类型
        /// </summary>
        public EDamageSourceType DamageSourceType { get; private set; }

        /// <summary>
        /// 伤害来源AbilityId
        /// </summary>
        private int _sourceAbilityId;

        /// <summary>
        /// 锁定目标的子弹
        /// </summary>
        public void LockTargetBullet(Unit attacker,
            Unit target,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int hitTargetDamageId,
            int hitNotTargetDamageId = 0)
        {
            initBulletBase(attacker, bulletData, damageSourceType, sourceAbilityId, hitTargetDamageId, hitNotTargetDamageId);
            _target = target;
            _target.AfterTickCallBack += onTargetTick;
            _target.RecycleCallBack += onTargetRemove;
            _bulletType = EBulletType.LockTargetBullet;
            UnitTransform.Pos = _attacker.UnitTransform.Pos;
        }

        /// <summary>
        /// 向指定方向飞行的子弹
        /// </summary>
        public void DirectionBullet(Unit attacker,
            float yAxisAngle,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int hitTargetDamageId,
            int hitNotTargetDamageId)
        {
            initBulletBase(attacker, bulletData, damageSourceType, sourceAbilityId, hitTargetDamageId, hitNotTargetDamageId);
            _bulletType = EBulletType.DirectionBullet;
            UnitTransform.Pos = _attacker.UnitTransform.Pos;
            UnitTransform.Rot = Quaternion.AngleAxis(yAxisAngle, Vector3.up);
        }

        /// <summary>
        /// 子弹基础初始化
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="bulletData"></param>
        /// <param name="damageSourceType"></param>
        /// <param name="sourceAbilityId"></param>
        /// <param name="hitTargetDamageId"></param>
        /// <param name="hitNotTargetDamageId"></param>
        private void initBulletBase(Unit attacker,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int hitTargetDamageId,
            int hitNotTargetDamageId)
        {
            _vfxComp = addComponent<VFXComp>();
            _attacker = attacker;
            _bulletData = bulletData;
            _hitTargetDamageId = hitTargetDamageId;
            _hitNotTargetDamageId = hitNotTargetDamageId;
            _sourceAbilityId = sourceAbilityId;
            _hitCountdown = 0;
            
            DamageSourceType = damageSourceType;
            Uid = World.Current.GetUid();
            
            _attacker.RecycleCallBack += onAttackerRemove;
            
            Debug.Log($"[Bullet] Bullet Create Uid {Uid} sourceUnit {_attacker} ability {_attacker}");
            SetAttr(EAttrType.AttrUid,             Uid);
            SetAttr(EAttrType.AttrModelId,         3);
            SetAttr(EAttrType.AttrMoveSpeedPCTAdd, (int)(_bulletData.speed * 10000));
            SetAttr(EAttrType.SourceAbilityType,   (int)DamageSourceType);
            SetAttr(EAttrType.AttrSourceAbilityId, _sourceAbilityId);
            SetAttr(EAttrType.AttrSourceActorUid,  attacker.Uid);
            if (_bulletData.rotSpeed <= 0)
            {
                SetAttr(EAttrType.AttrRotSpeedPCTAdd, Int32.MaxValue);
            }
            else
            {
                SetAttr(EAttrType.AttrRotSpeedPCTAdd, (int)(_bulletData.rotSpeed * 10000));
            }
            
            _checkBox = new CheckBoxData()
            {
                ShapeType = ECheckBoxShapeType.Sphere,
                Radius = _bulletData.hitRadius,
            };
        }

        public void Ctor()
        {
            addLoadTask(UnityAdapter.Instance.CreateUnityObjectProxy(this));
            AddAbility(_bulletData.BulletAbility);
        }

        protected override void onBeforeFirstTick()
        {
            if (_bulletData.isUseVFXKeyModel)
            {
                _vfxComp.AddVFXByKey(_bulletData.flyVFXStr, new VFXSetting() { duration = -1 });
            }
            else
            {
                _vfxComp.AddVFXByResPath(_bulletData.flyVFXStr, new VFXSetting() { duration = -1 });
            }
            
            ExecuteAbility(_bulletData.id);
        }

        protected override void onTick(float dt)
        {
            if (_attacker == null)
            {
                return;
            }
            
            _duration += dt;

            if (_hitCountdown > 0)
            {
                _hitCountdown -= dt;
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
                    var angle = Vector3.SignedAngle(UnitTransform.Forward, dir, Vector3.up);
                    var rotSpeed = (GetAttr(EAttrType.AttrRotSpeedPCT) / 10000f) * dt;
                    float realRotAngle = rotSpeed <= 0 ? angle : Mathf.Min(rotSpeed, angle);

                    //获取移动方向
                    UnitTransform.Rot = Quaternion.AngleAxis(realRotAngle, Vector3.up) * UnitTransform.Rot;
                    //var moveDt = UnitTransform.Forward * ((GetAttr(EAttrType.AttrMoveSpeedPCT) / 10000f) * dt);
                    var moveDt = UnitTransform.Forward * (_bulletData.speed * dt);
                    UnitTransform.Pos += moveDt;

                    if (_target == null) return;

                    if (!_bulletData.ignoreAllNotTarget)
                    {
                        //判定命中
                        checkCollision();
                    }

                    var range = _bulletData.hitRadius + _target.UnitTransform.Radius;
                    Debug.Log($"Bullet : {Uid} Distance {Vector3.Distance(_targetPos, UnitTransform.Pos)}");
                    Vector3 targetPosXZ = new Vector3(_targetPos.x,      0, _targetPos.z);
                    Vector3 selfPosXZ = new Vector3(UnitTransform.Pos.x, 0, UnitTransform.Pos.z);
                    if (Vector3.Distance(targetPosXZ, selfPosXZ) < range)
                    {
                        //和目标的距离小于半径和
                        HitSystem.Instance.SingleHit(_attacker, _target, DamageSourceType, _bulletData.id, false,
                                                     _hitTargetDamageId);
                        World.Current.RemoveUnit(this);
                    }

                    break;
                case EBulletType.DirectionBullet:
                    UnitTransform.Pos += UnitTransform.Forward * (_bulletData.speed * dt);

                    if (!_bulletData.ignoreAllNotTarget)
                    {
                        checkCollision();
                    }

                    break;
            }

            var proxy = UnityAdapter.Instance.GetUnityObjectProxy(Uid);
            proxy?.SyncPosition(UnitTransform.Pos);
        }

        private void checkCollision()
        {
            if (CommonUtility.HitRayCast(_checkBox, UnitTransform.Pos, UnitTransform.Rot, ref _maxHitResult))
            {
                foreach (var uid in _maxHitResult)
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
                    HitSystem.Instance.SingleHit(_attacker, target, DamageSourceType, _bulletData.id, false,
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
            _attacker.RecycleCallBack -= onAttackerRemove;
            _attacker = null;
        }

        private void onTargetTick(Unit target, float dt)
        {
            _targetPos = target.UnitTransform.Pos;
        }

        private void onTargetRemove(Unit _)
        {
            _target.AfterTickCallBack -= onTargetTick;
            _target.RecycleCallBack -= onTargetRemove;
            _target = null;
        }

        public override void Recycle()
        {
            GPool<Bullet>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            if (_attacker != null)
            {
                _attacker.RecycleCallBack -= onAttackerRemove;
            }
            
            if (_target != null)
            {
                _target.AfterTickCallBack -= onTargetTick;
                _target.RecycleCallBack -= onTargetRemove;
            }

            _attacker = default;
            _vfxComp = default;
            _bulletData = default;
            _curHitNumber = default;
            _duration = default;
            _hitTargetDamageId = default;
            _hitNotTargetDamageId = default;
            _target = default;
            _targetPos = default;
            _bulletType = default;
            _hitCountdown = default;
            _maxHitResult.Clear();
            _sourceAbilityId = 0;
            DamageSourceType = 0;
            UnityAdapter.Instance.RemoveUnitObjectProxy(Uid);
        }
    }
}