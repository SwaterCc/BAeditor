#region

using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core {
	public class Motion : IGPoolObject {
		/// <summary>
		/// 本次移动的Uid
		/// </summary>
		public int Uid { get; private set; }
		/// <summary>
		/// 移动设置
		/// </summary>
		public MotionSetting Setting { get; private set; }
		/// <summary>
		/// 当前帧速度
		/// </summary>
		public float Speed { get; protected set; }
		/// <summary>
		/// 最终计算的速度
		/// </summary>
		public Vector3 Velocity { get; protected set; }
		/// <summary>
		/// 位移开始的时间
		/// </summary>
		public float BeginTime { get; private set; }
		/// <summary>
		/// 位移是否结束
		/// </summary>
		public bool IsEnd { get; private set; }
		/// <summary>
		/// 是否锁定目标
		/// </summary>
		public bool LockTarget { get; private set; }
		/// <summary>
		/// motion绑定的对象
		/// </summary>
		private Unit _self;
		/// <summary>
		/// 锁定的目标
		/// </summary>
		private Unit _target;
		/// <summary>
		/// 指定的坐标
		/// </summary>
		private Vector3 _targetPosition;
		/// <summary>
		/// 上一次同步目标位置的时间
		/// </summary>
		private float _beforeSyncTargetPositionTime;
		/// <summary>
		/// 当前方向
		/// </summary>
		private Vector3 _direction;
		/// <summary>
		/// 位移类型
		/// </summary>
		private EMotionType _motionType;
		
		public void Init(int uid, Unit self, MotionSetting setting) {
			Uid = uid;
			Setting = setting;
			Velocity = Vector3.zero;
			BeginTime = World.Current.RealWorldTimeSinceStart;
			Speed = setting.speed;
			_self = self;
			_self.RecycleCallBack += onSelfRecycle;
		}

		public void DriectionMoiton(in Vector3 dir) {
			_direction = dir;
			_motionType = EMotionType.Liner;
		}
		
		public void LockTargetMotion(Unit target) {
			_target = target;
			_target.RecycleCallBack += onTargetRecycle;
			_motionType = EMotionType.MoveToTarget;
			_targetPosition = _target.UnitTransform.Pos;
			_beforeSyncTargetPositionTime = World.Current.RealWorldTimeSinceStart;
			_direction = CommonUtility.GetXZDirection(_self.UnitTransform.Pos, _targetPosition);
		}
		
		public void LockPositionMotion(Vector3 position) {
			_motionType = EMotionType.MoveToPosition;
			_direction = CommonUtility.GetXZDirection(_self.UnitTransform.Pos, position);
		}
		
		private void onSelfRecycle(Unit _) {
			_self = null;
			IsEnd = true;
		}
		
		private void onTargetRecycle(Unit _) {
			_target = null;
		}
		
		public void Tick() {
			Speed += Setting.acceleration * World.Current.OnceTickTime;
			if (Setting.maxSpeed > 0) {
				Speed = Mathf.Min(Speed, Setting.maxSpeed);
			}
			//计算最终速度
			CalcVelocity();
			//请求计算碰撞
			CheckCollision();
			if (Setting.duration > 0 && World.Current.RealWorldTimeSinceStart - BeginTime > Setting.duration) {
				IsEnd = true;
			}
		}

		protected void CalcVelocity() {
			switch (_motionType) {
				case EMotionType.MoveToPosition:
					_direction = CommonUtility.GetXZDirection(_self.UnitTransform.Pos, _targetPosition);
					break;
				case EMotionType.MoveToTarget:
					if (_target == null) {
						return;
					}
					else {
						//每隔0.2秒更新下目标坐标
						if (World.Current.RealWorldTimeSinceStart - _beforeSyncTargetPositionTime > 0.2f) {
							_beforeSyncTargetPositionTime = World.Current.RealWorldTimeSinceStart;
							_targetPosition = _target.UnitTransform.Pos;
						}
					}
					_direction = CommonUtility.GetXZDirection(_self.UnitTransform.Pos, _targetPosition);
					break;
			}
			Velocity = Speed * _direction;
		}
		
		private void CheckCollision() {
			//委托给逻辑四叉树碰撞系统
		}

		public void OnRecycle() {
			Uid = default;
			Setting = default;
			Speed = default;
			Velocity = default;
			BeginTime = default;
			IsEnd = default;
			LockTarget = default;

			_self = null;
			
		}
	}
}