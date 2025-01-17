#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class MotionComp : AComponent
        {
            /// <summary>
            /// MotionId生成器，每个组件独立
            /// </summary>
            private readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

            /// <summary>
            /// Motion字典
            /// </summary>
            private readonly Dictionary<int, Motion> _motionDict = new(10);

            /// <summary>
            /// 删除队列
            /// </summary>
            private readonly List<int> _removeList = new(5);

            /// <summary>
            /// 静止移动输入
            /// </summary>
            public bool DisableMoveInput { get; private set; }

            /// <summary>
            /// 强制面向移动方向
            /// </summary>
            public bool ForceFaceMoveTarget { get; private set; }

            public MotionComp(ActorLogic logic) : base(logic) { }

            public override void onInit() { }

            public int AddMotion(int moveTargetUid, in MotionSetting motionSetting,
                in Action<int> moveCallBack = null)
            {
                if (moveTargetUid <= 0)
                {
                    Debug.LogError("找不到位移目标");
                    return -1;
                }

                if (!ActorManager.Instance.TryGetActor(moveTargetUid, out var moveTarget))
                {
                    Debug.LogError("找不到位移目标");
                    return -1;
                }

                var uid = _idGenerator.GenerateId();
                var motion = APool<Motion>.Pool.Rent();
                motion.OnRent(uid, ActorLogic, moveTarget, motionSetting, moveCallBack);
                _motionDict.Add(uid, motion);
                return uid;
            }

            public void RemoveMotion(int uid)
            {
                if (_motionDict.TryGetValue(uid, out var motion))
                {
                    motion.MoveEnd();
                    APool<Motion>.Pool.Recycle(motion);
                    _removeList.Add(uid);
                }
            }

            protected override void onTick(float dt)
            {
                DisableMoveInput = false;
                ForceFaceMoveTarget = false;
                if (_motionDict.Count == 0) return;

                var curPos = Self.Pos;
                Vector3 finalOffset = Vector3.zero;
                foreach (var motionPair in _motionDict)
                {
                    var motion = motionPair.Value;
                    if (!motion.IsBegin)
                    {
                        motion.MotionBegin();
                    }

                    if (motion.IsBegin && !motion.IsEnd)
                    {
                        motion.Moving(dt);
                        finalOffset += motion;
                    }

                    if (motion.IsEnd)
                    {
                        RemoveMotion(motion.Uid);
                    }

                    DisableMoveInput = motion.Setting.DisableMoveInput;
                    if (ForceFaceMoveTarget)
                    {
                        Debug.LogWarning("存在复数个强制面向目标的Motion");
                    }

                    ForceFaceMoveTarget = motion.Setting.MovingFaceToTarget;
                }

                Self.Pos = curPos + finalOffset;

                if (ForceFaceMoveTarget)
                {
                    Self.Rot = Quaternion.FromToRotation(Vector3.forward, finalOffset.normalized);
                }

                foreach (var motionUid in _removeList)
                {
                    _motionDict.Remove(motionUid);
                }

                _removeList.Clear();
            }

            public override void Clear()
            {
                foreach (var pMotion in _motionDict)
                {
                    APool<Motion>.Pool.Recycle(pMotion.Value);
                }

                _motionDict.Clear();
                _removeList.Clear();
                DisableMoveInput = false;
                ForceFaceMoveTarget = false;
            }
        }
    }
}