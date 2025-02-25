using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Hono.Scripts.Battle.Core {
	public class VFXSystem : Singleton<VFXSystem>, IWorldSystemWhenTickCalled, IWorldSystemWhenExitCalled {
		private static readonly CommonUtility.IdGenerator IDGenerator = CommonUtility.GetIdGenerator();
		/// <summary>
		/// 世界特效
		/// </summary>
		private List<VFXInfo> _vfxInfos = new(500);

		public int AddVFXToWorld(string path, float lifeTime, Vector3 position, Vector3 eulerAngle, float scale = 1) {
			if (string.IsNullOrEmpty(path)) {
				return -1;
			}

			var worldRoot = World.Current.WorldRoot;

			VFXInfo vfxInfo = new() {
				Uid = IDGenerator.GenerateId(),
				OwnerUid = worldRoot.Uid,
				Path = path,
				Pos = position,
				Rot = Quaternion.Euler(eulerAngle),
				Scale = scale * Vector3.one,
				CreateTime = World.Current.RealWorldTimeSinceStart,
				LifeTime = lifeTime,
				IsPermanent = lifeTime < 0
			};
			_vfxInfos.Add(vfxInfo);

			if (UnityAdapter.Instance.TryGetUnityObjectProxy(worldRoot.Uid, out var proxy)) {
				proxy.AddVFXToWorld(vfxInfo);
			}

			return vfxInfo.Uid;
		}

		/// <summary>
		/// 添加特效到单位上
		/// </summary>
		/// <param name="vfxOwnerUid"></param>
		/// <param name="path"></param>
		/// <param name="lifeTime"></param>
		/// <param name="bindBond"></param>
		/// <param name="position"></param>
		/// <param name="eulerAngle"></param>
		/// <param name="scale"></param>
		/// <param name="notRotFollowParent"></param>
		public int AddVFXToUnit(int vfxOwnerUid,
			string path,
			float lifeTime,
			Vector3 position,
			Vector3 eulerAngle,
			string bindBond = "",
			float scale = 1,
			bool notRotFollowParent = false) {
			
			if (string.IsNullOrEmpty(path)) {
				return -1;
			}

			VFXInfo vfxInfo = new() {
				Uid = IDGenerator.GenerateId(),
				OwnerUid = vfxOwnerUid,
				Path = path,
				Pos = position,
				Rot = Quaternion.Euler(eulerAngle),
				Scale = scale * Vector3.one,
				CreateTime = World.Current.RealWorldTimeSinceStart,
				LifeTime = lifeTime,
				IsPermanent = lifeTime < 0
			};
			_vfxInfos.Add(vfxInfo);

			if (UnityAdapter.Instance.TryGetUnityObjectProxy(vfxOwnerUid, out var proxy)) {
				proxy.AddVFXToUnit(vfxInfo, bindBond, notRotFollowParent);
			}

			return vfxInfo.Uid;
		}

		public void RemoveVFXByUid(int uid) {
			for (int index = 0; index < _vfxInfos.Count; index++) {
				VFXInfo vfxInfo = _vfxInfos[index];
				if (vfxInfo.Uid == uid) {
					if (UnityAdapter.Instance.TryGetUnityObjectProxy(vfxInfo.OwnerUid, out var proxy)) {
						proxy.RemoveVFX(vfxInfo);
					}

					_vfxInfos.RemoveAtSwapBack(index);
					return;
				}
			}
		}
		
		public void OnWorldTick(float dt) {
			for (int i = 0; i < _vfxInfos.Count; i++) {
				var vfxInfo = _vfxInfos[i];
				if (!vfxInfo.IsPermanent) {
					if (World.Current.RealWorldTimeSinceStart - vfxInfo.CreateTime > vfxInfo.LifeTime) {
						if (UnityAdapter.Instance.TryGetUnityObjectProxy(vfxInfo.OwnerUid, out var proxy)) {
							proxy.RemoveVFX(vfxInfo);
						}

						_vfxInfos.RemoveAtSwapBack(i);
						i--;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void RemoveUnitAllVFX(Unit unit) {
			for (int index = 0; index < _vfxInfos.Count; index++) {
				VFXInfo vfxInfo = _vfxInfos[index];
				if (_vfxInfos[index].OwnerUid != unit.Uid) {
					continue;
				}

				if (UnityAdapter.Instance.TryGetUnityObjectProxy(vfxInfo.OwnerUid, out var proxy)) {
					proxy.RemoveVFX(vfxInfo);
				}

				_vfxInfos.RemoveAtSwapBack(index);
				index--;
			}
		}

		public void OnWorldExit() {
			_vfxInfos.Clear();
		}
	}
}