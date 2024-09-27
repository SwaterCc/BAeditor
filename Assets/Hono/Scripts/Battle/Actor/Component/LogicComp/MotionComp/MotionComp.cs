using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class MotionComp : ALogicComponent {
			private CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
			private Dictionary<int, Motion> _motionDict = new();
			private List<int> _removeList = new();

			public Action<Motion> MotionAdd;
			public Action<Motion> MotionRemove;

			public MotionComp(ActorLogic logic) : base(logic) { }

			public override void Init() { }

			public bool HasMotion() {
				return _motionDict.Count > 0;
			}

			public int AddMotion(int moveTargetUid, MotionSetting motionSetting) {
				if (moveTargetUid <= 0) {
					Debug.LogError("找不到位移目标");
					return -1;
				}

				if (!ActorManager.Instance.TryGetActor(moveTargetUid, out var moveTarget)) {
					Debug.LogError("找不到位移目标");
					return -1;
				}

				var uid = _idGenerator.GenerateId();
				var motion = new Motion(ActorLogic, moveTarget, motionSetting);
				_motionDict.Add(uid, motion);
				MotionAdd?.Invoke(motion);
				return uid;
			}

			public void RemoveMotion(int uid) {
				if (_motionDict.TryGetValue(uid, out var motion)) {
					motion.MoveEnd();
					_removeList.Add(uid);
				}
			}

			/// <summary>
			/// 触发器直接调用
			/// </summary>
			public void OnCollision(int otherUid) {
				foreach (var motion in _motionDict) {
					motion.Value.OnMoveCollision(otherUid);
				}
			}

			protected override void onTick(float dt) {
				if (_motionDict.Count == 0) return;

				var curPos = Actor.GetAttr<Vector3>(ELogicAttr.AttrPosition);
				Vector3 finalOffset = Vector3.zero;
				foreach (var motionPair in _motionDict) {
					var motion = motionPair.Value;
					if (!motion.IsBegin) {
						motion.MotionBegin();
					}

					if (motion.IsBegin && !motion.IsEnd) {
						motion.Moving(dt);
						finalOffset += motion;
					}

					if (motion.IsEnd) {
						motion.MoveEnd();
						_removeList.Add(motionPair.Key);
					}
				}

				Actor.SetAttr<Vector3>(ELogicAttr.AttrPosition, curPos + finalOffset, false);

				foreach (var motionUid in _removeList) {
					var motion = _motionDict[motionUid];
					_motionDict.Remove(motionUid);
					MotionRemove?.Invoke(motion);
				}

				_removeList.Clear();
			}
		}
	}
}