using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Hono.Scripts.Battle.Core {
	public class MotionSystem : Singleton<MotionSystem> {
		private IdAllocator _idAllocator = new();
		private Dictionary<int, Motion> _motionDict = new();
		private List<Motion> _removeList = new();

		/// <summary>
		/// 跟随位移
		/// </summary>
		public int AddFollowTargetMotion(Unit self, Unit targetUnitOfMoveTo, in MotionSetting motionSetting) {
			if (targetUnitOfMoveTo == null) {
				Debug.LogError("找不到位移目标");
				return -1;
			}

			var motion = GPool<Motion>.Pool.Rent();
			motion.Init(_idAllocator.Allocate(), self, motionSetting);
			motion.LockTargetMotion(targetUnitOfMoveTo);
			_motionDict.Add(motion.Uid, motion);
			return motion.Uid;
		}

		/// <summary>
		/// 直线位移
		/// </summary>
		public Motion AddLinerMotion(Unit self, Vector3 dir, MotionSetting motionSetting) {
			var motion = GPool<Motion>.Pool.Rent();
			motion.Init(_idAllocator.Allocate(), self, motionSetting);
			motion.DriectionMoiton(dir);
			_motionDict.Add(motion.Uid, motion);
			return motion;
		}
		
		/// <summary>
		/// 直线位移
		/// </summary>
		public Motion AddLockPositionMotion(Unit self, Vector3 pos, MotionSetting motionSetting) {
			var motion = GPool<Motion>.Pool.Rent();
			motion.Init(_idAllocator.Allocate(), self, motionSetting);
			motion.LockPositionMotion(pos);
			_motionDict.Add(motion.Uid, motion);
			return motion;
		}

		public void RemoveMotion(int uid) {
			if (_motionDict.TryGetValue(uid, out var motion)) {
				_removeList.Add(motion);
			}
		}

		public void Tick(float dt) {
			foreach (var motion in _motionDict.Values) {
				if (motion.IsEnd) {
					_removeList.Add(motion);
					continue;
				}
				motion.Tick();
			}

			foreach (var motion in _removeList) {
				_motionDict.Remove(motion.Uid);
				GPool<Motion>.Pool.Recycle(motion);
			}
			_removeList.Clear();
		}
	}
}