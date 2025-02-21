using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using System.Linq;
using UnityEngine;


namespace Hono.Scripts.Battle.Core {
	//锁目标 直线
	//方向  直线
	public class Bullet : Unit, IGPoolObject {
		private enum EBulletType {
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
		private List<int> _maxHitResult = new(50);
		/// <summary>
		/// 碰撞cd
		/// </summary>
		private Dictionary<int, float> _hitCDList = new(10);

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
			int hitNotTargetDamageId = 0) {
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
			int hitNotTargetDamageId) {
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
			int hitNotTargetDamageId) {
			_attacker = attacker;
			_bulletData = bulletData;
			_hitTargetDamageId = hitTargetDamageId;
			_hitNotTargetDamageId = hitNotTargetDamageId;
			_sourceAbilityId = sourceAbilityId;

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
			if (_bulletData.rotSpeed <= 0) {
				SetAttr(EAttrType.AttrRotSpeedPCTAdd, Int32.MaxValue);
			}
			else {
				SetAttr(EAttrType.AttrRotSpeedPCTAdd, (int)(_bulletData.rotSpeed * 10000));
			}

			_checkBox = new CheckBoxData() { ShapeType = ECheckBoxShapeType.Sphere, Radius = _bulletData.hitRadius, };
		}

		public void Ctor() {
			addLoadTask(UnityAdapter.Instance.CreateUnityObjectProxy(this));
			AddAbility(_bulletData.BulletAbility);
		}

		protected override void onBeforeFirstTick() {
			string path = null;
			if (_bulletData.isUseVFXKeyModel) {
				path = _attacker.GetVFXPathByKey(_bulletData.flyVFXStr);
			}
			else {
				path = _bulletData.flyVFXStr;
			}

			VFXManager.Instance.AddVFXToUnit(Uid, path, -1, Vector3.zero, Vector3.zero);

			ExecuteAbility(_bulletData.id);
		}

		protected override void onTick(float dt) {
			if (_attacker == null) {
				return;
			}

			_duration += dt;

			move(dt);

			if (_duration > _bulletData.lifeTime) {
				dead();
			}
		}

		private void move(float dt) {
			switch (_bulletType) {
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

					if (!_bulletData.ignoreAllNotTarget) {
						//判定命中
						checkCollision();
					}

					var range = _bulletData.hitRadius + _target.UnitTransform.Radius;
					Vector3 targetPosXZ = new(_targetPos.x, 0, _targetPos.z);
					Vector3 selfPosXZ = new(UnitTransform.Pos.x, 0, UnitTransform.Pos.z);
					if (Vector3.Distance(targetPosXZ, selfPosXZ) < range) {
						//和目标的距离小于半径和
						HitSystem.Instance.SingleHit(_attacker, _target, DamageSourceType, _bulletData.id, false,
							_hitTargetDamageId);
						dead();
					}

					break;
				case EBulletType.DirectionBullet:
					UnitTransform.Pos += UnitTransform.Forward * (_bulletData.speed * dt);

					if (!_bulletData.ignoreAllNotTarget) {
						checkCollision();
					}

					break;
			}
		}

		private void checkCollision() {
			if (CommonUtility.HitRayCast(_checkBox, UnitTransform.Pos, UnitTransform.Rot, ref _maxHitResult)) {
				foreach (var uid in _maxHitResult) {
					if (uid == Uid || uid == _attacker.Uid) {
						continue;
					}

					if (!World.Query.ConditionFilter(_attacker, uid, _bulletData.notTargetHitCondition)) {
						return;
					}

					if (_bulletData.minHitInterval > 0 && _hitCDList.TryGetValue(uid, out float hitTime) && Time.realtimeSinceStartup - hitTime < _bulletData.minHitInterval) {
						return;
					}

					var target = World.Query.GetUnit(uid);
					HitSystem.Instance.SingleHit(_attacker, target, DamageSourceType, _bulletData.id, false,
						_hitNotTargetDamageId);

					if (_bulletData.minHitInterval > 0) {
						_hitCDList[uid] = Time.realtimeSinceStartup;
					}

					string path = null;
					if (_bulletData.isUseVFXKeyModel) {
						path = _attacker.GetVFXPathByKey(_bulletData.hitVFXStr);
					}
					else {
						path = _bulletData.hitVFXStr;
					}

					VFXManager.Instance.AddVFXToWorld(path, 1, UnitTransform.Pos, Vector3.zero);

					++_curHitNumber;
					if (_curHitNumber >= _bulletData.maxHitCount) {
						dead();
					}
				}
			}
		}

		private void onAttackerRemove(Unit _) {
			_attacker.RecycleCallBack -= onAttackerRemove;
			_attacker = null;
		}

		private void onTargetTick(Unit target, float dt) {
			_targetPos = target.UnitTransform.Pos;
		}

		private void onTargetRemove(Unit _) {
			_target.AfterTickCallBack -= onTargetTick;
			_target.RecycleCallBack -= onTargetRemove;
			_target = null;
		}

		private void dead() {
			string path = null;
			if (_bulletData.isUseVFXKeyModel) {
				path = _attacker.GetVFXPathByKey(_bulletData.deadVFXStr);
			}
			else {
				path = _bulletData.deadVFXStr;
			}

			VFXManager.Instance.AddVFXToWorld(path, 1, UnitTransform.Pos, Vector3.zero);


			World.Current.RemoveUnit(this);
		}

		public override void Recycle() {
			GPool<Bullet>.Pool.Recycle(this);
		}

		public void OnRecycle() {
			if (_attacker != null) {
				_attacker.RecycleCallBack -= onAttackerRemove;
			}

			if (_target != null) {
				_target.AfterTickCallBack -= onTargetTick;
				_target.RecycleCallBack -= onTargetRemove;
			}

			_attacker = default;
			_bulletData = default;
			_curHitNumber = default;
			_duration = default;
			_hitTargetDamageId = default;
			_hitNotTargetDamageId = default;
			_target = default;
			_targetPos = default;
			_bulletType = default;
			_hitCDList.Clear();
			_maxHitResult.Clear();
			_sourceAbilityId = 0;
			DamageSourceType = 0;
			UnityAdapter.Instance.RemoveUnitObjectProxy(Uid);
		}
	}
}