using System;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class Motion {
			private MotionSetting _setting;
			public MotionSetting Setting => _setting;
			private int _moveTargetUid;
			private Vector3 _moveTargetPos;
			private bool _targetSurvive;
			private float _dt;
			private ActorLogic _logic;
			private MotionEventInfo _motionEventInfo;
			private Vector3 _moveOffset;
			private float _speed;
			private Action<int> _moveCollisionCallBack;
			
			private bool _isBegin;
			public bool IsBegin => _isBegin; 
			
			private bool _isEnd;
			public bool IsEnd => _isEnd;

			public Motion(int uid ,ActorLogic logic, Actor target, MotionSetting setting,Action<int> callBack = null) {
				_moveTargetUid = target.Uid;
				_setting = setting;
				_logic = logic;
				_moveOffset = Vector3.zero;
				_motionEventInfo = new MotionEventInfo();
				_motionEventInfo.MotionUid = uid;
				_speed = setting.Speed;
				_moveTargetPos = target.Pos;
				_moveCollisionCallBack = callBack;
			}

			#region 重载运算符
			public static implicit operator Vector3(Motion a) {
				return a._moveOffset;
			}
			
			public static Vector3 operator +(Motion a, Motion b) {
				return a._moveOffset + b._moveOffset;
			}
			
			public static Vector3 operator +(Vector3 a, Motion b) {
				return a + b._moveOffset;
			}
			
			public static Vector3 operator +(Motion a, Vector3 b) {
				return a._moveOffset + b;
			}
			#endregion
			
			public void MotionBegin() {
				BattleEventManager.Instance.TriggerEvent(_logic.Uid, EBattleEventType.OnMotionBegin, _motionEventInfo);
				_isBegin = true;
			}

			public void Moving(float dt) {
				if (_dt > _setting.Duration) {
					_isEnd = true;
				}

				if (ActorManager.Instance.TryGetActor(_moveTargetUid, out Actor target)) {
					_moveTargetPos = target.Pos;
				}
				else {
					if (Mathf.Abs(Vector3.Distance(_logic.Actor.Pos, _moveTargetPos)) < 0.25f) {
						_isEnd = true;
					}
				}

				switch (_setting.MoveType) {
					case EMotionType.Liner:
						doLinerMotion(dt);
						break;
					case EMotionType.Around:
						doAroundMotion(dt);
						break;
					case EMotionType.Curve:
						doCurve(dt);
						break;
				}
				
				_dt += dt;
			}

			private void doLinerMotion(float dt) {
				//方向
				var dir = (_moveTargetPos - _logic.Actor.Pos).normalized;
				
				if (_setting.IsReverse) {
					dir *= -1;
				}

				_moveOffset = dir * _speed * dt;
				_speed -= _setting.Acceleration * dt;
			}

			private void doAroundMotion(float dt) {
				
			}

			private void doCurve(float dt) {
				
			}
			
			public void OnMoveCollision(int colliderUid) {
				_moveCollisionCallBack?.Invoke(colliderUid);
				
				if(_setting.TriggerEventClose) return;

				_motionEventInfo.MotionCollisionId = colliderUid;
				BattleEventManager.Instance.TriggerEvent(_logic.Uid, EBattleEventType.OnMoveCollision, _motionEventInfo);
				
				if (!_setting.StopAfterCollision) {
					if (colliderUid != _moveTargetUid) {
						return;
					}
				}
				
				_isEnd = true;
			}

			public void MoveEnd() {
				BattleEventManager.Instance.TriggerEvent(_logic.Uid, EBattleEventType.OnMotionEnd, _motionEventInfo);
			}
		}
	}
}