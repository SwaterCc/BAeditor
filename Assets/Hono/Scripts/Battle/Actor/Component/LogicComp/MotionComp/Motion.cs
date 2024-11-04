#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class Motion
        {
            private MotionSetting _setting;
            public MotionSetting Setting => _setting;
            private int _moveTargetUid;
            private Vector3 _moveTargetPos;
            private bool _targetSurvive;
            private float _dt;
            private readonly ActorLogic _logic;
            private readonly MotionEventInfo _motionEventInfo;
            private Vector3 _moveOffset;
            private float _speed;
            private readonly Action<int> _moveCollisionCallBack;
            private const float ModelRadius = 1.2f;
            private List<int> _hitResults = new(32);

            private bool _isBegin;
            public bool IsBegin => _isBegin;

            private bool _isEnd;
            public bool IsEnd => _isEnd;

            public Motion(int uid, ActorLogic logic, Actor target, MotionSetting setting, Action<int> callBack = null)
            {
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

            public static implicit operator Vector3(Motion a)
            {
                return a._moveOffset;
            }

            public static Vector3 operator +(Motion a, Motion b)
            {
                return a._moveOffset + b._moveOffset;
            }

            public static Vector3 operator +(Vector3 a, Motion b)
            {
                return a + b._moveOffset;
            }

            public static Vector3 operator +(Motion a, Vector3 b)
            {
                return a._moveOffset + b;
            }

            #endregion

            private void checkCollisionAfterMove()
            {
                var checkBox = new CheckBoxData()
                {
                    ShapeType = ECheckBoxShapeType.Cube,
                    Height = 2,
                    Width = ModelRadius,
                    Length = 1,
                    Offset = new SVector3(0, 1, ModelRadius / 2f),
                };

                if (CommonUtility.HitRayCast(checkBox, _logic.Actor.Pos, _logic.Actor.Rot, ref _hitResults))
                {
                    foreach (var uid in _hitResults)
                    {
                        if (uid == _logic.Uid)
                        {
                            continue;
                        }

                        OnMoveCollision(uid);
                    }
                }
            }

            public void MotionBegin()
            {
                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnMotionBegin,
                    _motionEventInfo);
                _isBegin = true;
            }

            public void Moving(float dt)
            {
                if (_dt > _setting.Duration)
                {
                    _isEnd = true;
                }

                if (ActorManager.Instance.TryGetActor(_moveTargetUid, out Actor target))
                {
                    _moveTargetPos = target.Pos;
                }
                else
                {
                    if (Vector3.Distance(_logic.Actor.Pos, _moveTargetPos) < ModelRadius / 2f)
                    {
                        _isEnd = true;
                    }
                }

                checkCollisionAfterMove();

                if (_isEnd) return;

                switch (_setting.MoveType)
                {
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

            private void doLinerMotion(float dt)
            {
                //方向
                var dir = (_moveTargetPos - _logic.Actor.Pos).normalized;
                //dir.y = 0;
                if (_setting.IsReverse)
                {
                    dir *= -1;
                }

                _moveOffset = dir * (_speed * dt);
                _speed -= _setting.Acceleration * dt;
            }

            private void doAroundMotion(float dt) { }

            private void doCurve(float dt) { }

            public void OnMoveCollision(int colliderUid)
            {
                _moveCollisionCallBack?.Invoke(colliderUid);

                if (_setting.TriggerEventClose) return;

                if (!_setting.StopAfterCollision)
                {
                    if (colliderUid != _moveTargetUid)
                    {
                        return;
                    }
                }

                _motionEventInfo.MotionCollisionId = colliderUid;
                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnMoveCollision,
                    _motionEventInfo);

                _isEnd = true;
            }

            public void MoveEnd()
            {
                BattleEventManager.Instance.TriggerActorEvent(_logic.Uid, EBattleEventType.OnMotionEnd,
                    _motionEventInfo);
            }
        }
    }
}