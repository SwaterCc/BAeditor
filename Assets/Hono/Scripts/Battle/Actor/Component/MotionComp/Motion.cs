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
        /// <summary>
        /// 移动数据，目前其实是直接对坐标进行修正，后续改为速度向量的修正，单位移动最后依靠最终速度来决定移动
        /// </summary>
        public class Motion : IAPoolObject
        {
            /// <summary>
            /// 位移组件
            /// </summary>
            public MotionComp Comp { get; private set; }

            /// <summary>
            /// Logic
            /// </summary>
            public ActorLogic Logic { get; private set; }

            /// <summary>
            /// 唯一Id
            /// </summary>
            public int Uid { get; private set; }

            /// <summary>
            /// 移动设置
            /// </summary>
            public MotionSetting Setting { get; private set; }

            /// <summary>
            /// 移动目标的UID
            /// </summary>
            private int _moveTargetUid;

            /// <summary>
            /// 目的地坐标
            /// </summary>
            private Vector3 _moveTargetPos;

            /// <summary>
            /// 目标是否存活
            /// </summary>
            private bool _targetSurvive;

            //private readonly MotionEventInfo _motionEventInfo = new();
            private Vector3 _moveOffset;
            private float _speed;
            private const float ModelRadius = 1.2f;
            private List<int> _hitResults = new(32);

            /// <summary>
            /// 移动持续时长
            /// </summary>
            private float _motionDuration;

            /// <summary>
            /// 位移开始
            /// </summary>
            public bool IsBegin { get; private set; }

            /// <summary>
            /// 位移结束
            /// </summary>
            public bool IsEnd { get; private set; }

            /// <summary>
            /// 位移碰撞回调
            /// </summary>
            private Action<int> _moveCollisionCallBack;

            public void OnRent(in int uid, in ActorLogic logic, in Actor target, in MotionSetting setting,
                Action<int> callBack = null)
            {
                _moveTargetUid = target.Uid;
                Setting = setting;
                Logic = logic;
                _moveOffset = Vector3.zero;

                //_motionEventInfo.MotionUid = uid;
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
                    Offset = new Vector3(0, 1, ModelRadius / 2f),
                };

                if (CommonUtility.HitRayCast(checkBox, Logic.Self.Pos, Logic.Self.Rot, ref _hitResults))
                {
                    foreach (var uid in _hitResults)
                    {
                        if (uid == Logic.Uid)
                        {
                            continue;
                        }

                        OnMoveCollision(uid);
                    }
                }
            }

            public void MotionBegin()
            {
                
                IsBegin = true;
            }

            public void Moving(float dt)
            {
                if (_motionDuration > Setting.Duration)
                {
                    IsEnd = true;
                }

                if (ActorManager.Instance.TryGetActor(_moveTargetUid, out Actor target))
                {
                    _moveTargetPos = target.Pos;
                }
                else
                {
                    if (Vector3.Distance(Logic.Self.Pos, _moveTargetPos) < ModelRadius / 2f)
                    {
                        IsEnd = true;
                    }
                }

                checkCollisionAfterMove();

                if (IsEnd) return;

                switch (Setting.MoveType)
                {
                    case EMotionType.Liner:
                        doLinerMotion(dt);
                        break;
                    case EMotionType.Around:
                        doAroundMotion(dt);
                        break;
                    case EMotionType.Parabola:
                        doCurve(dt);
                        break;
                }

                _motionDuration += dt;
            }

            private void doLinerMotion(float dt)
            {
                //方向
                var dir = (_moveTargetPos - Logic.Self.Pos).normalized;
                //dir.y = 0;
                if (Setting.IsReverse)
                {
                    dir *= -1;
                }

                _moveOffset = dir * (_speed * dt);
                _speed -= Setting.Acceleration * dt;
            }

            private void doAroundMotion(float dt) { }

            private void doCurve(float dt) { }

            public void OnMoveCollision(int colliderUid)
            {
                _moveCollisionCallBack?.Invoke(colliderUid);

                if (Setting.TriggerEventClose) return;

                if (!Setting.StopAfterCollision)
                {
                    if (colliderUid != _moveTargetUid)
                    {
                        return;
                    }
                }

               

                IsEnd = true;
            }

            public void MoveEnd()
            {
              
            }

            public void OnRecycle()
            {
                _moveTargetUid = 0;
                Setting = null;
                Logic = null;
                _moveOffset = Vector3.zero;

              
                _speed = 0;
                _moveTargetPos = default;
                _moveCollisionCallBack = null;
            }
        }
    }
}