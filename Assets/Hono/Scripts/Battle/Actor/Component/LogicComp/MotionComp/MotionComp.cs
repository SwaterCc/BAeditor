using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class MotionComp : ALogicComponent
        {
            private CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
            private Dictionary<int, Motion> _motionDict = new();
            private List<int> _removeList = new();
            
            
            public Action<Motion> MotionAdd;
            public Action<Motion> MotionRemove;

            public bool DisableMoveInput { get; private set; }
            public bool ForceFaceMoveTarget { get; private set; }
            public MotionComp(ActorLogic logic) : base(logic) { }

            public override void Init() { }

            public bool HasMotion()
            {
                return _motionDict.Count > 0;
            }

            public int AddMotion(int moveTargetUid, MotionSetting motionSetting, Action<int> moveCallBack = null)
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
                var motion = new Motion(uid, ActorLogic, moveTarget, motionSetting, moveCallBack);
                _motionDict.Add(uid, motion);
                MotionAdd?.Invoke(motion);
                return uid;
            }

            public void RemoveMotion(int uid)
            {
                if (_motionDict.TryGetValue(uid, out var motion))
                {
                    motion.MoveEnd();
                    _removeList.Add(uid);
                }
            }

            /// <summary>
            /// 触发器直接调用
            /// </summary>
            public void OnCollision(int otherUid)
            {
                return;
            }

            protected override void onTick(float dt)
            {
                DisableMoveInput = false;
                ForceFaceMoveTarget = false;
                if (_motionDict.Count == 0) return;

                var curPos = Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
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
                        motion.MoveEnd();
                        _removeList.Add(motionPair.Key);
                    }

                    DisableMoveInput = motion.Setting.DisableMoveInput;
                    if (ForceFaceMoveTarget)
                    {
                        Debug.LogWarning("存在复数个强制面向目标的Motion");
                    }
                    ForceFaceMoveTarget = motion.Setting.MovingFaceToTarget;
                }

                Actor.SetAttr<Vector3>(ELogicAttr.AttrPosition, curPos + finalOffset, false);

				if (ForceFaceMoveTarget) {
					Actor.SetAttr<Quaternion>(ELogicAttr.AttrRot, Quaternion.FromToRotation(Vector3.forward, finalOffset.normalized), false);
				}

                foreach (var motionUid in _removeList)
                {
                    var motion = _motionDict[motionUid];
                    _motionDict.Remove(motionUid);
                    MotionRemove?.Invoke(motion);
                }

                _removeList.Clear();
            }
        }
    }
}