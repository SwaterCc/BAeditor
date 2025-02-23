using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class UnityObjectProxy {
		private struct VFXGameObject {
			public VFXInfo VFXInfo;
			public bool NotFollowParentRot;
			public GameObject GameObject;
		}

		/// <summary>
		/// 绑定的Unit
		/// </summary>
		public Unit Unit { get; private set; }

		/// <summary>
		/// 代理类型
		/// </summary>
		private EUOProxyType _proxyType;

		/// <summary>
		/// 场景对象的代理
		/// </summary>
		private GameObject _proxy;

		/// <summary>
		/// actorModel路径
		/// </summary>
		private string _actorModelPath;

		/// <summary>
		/// 模型,及挂点收集
		/// </summary>
		private ActorModelHandler _actorModelHandler;

		/// <summary>
		/// 物理组件对象
		/// </summary>
		private UOProxyPhysicsHandler _physicsHandler;

		/// <summary>
		/// 特效缓存
		/// </summary>
		private readonly Dictionary<int, VFXGameObject> _vfxCache = new(20);

		/// <summary>
		/// model表数据
		/// </summary>
		private ModelTableRow _modelRow;

		/// <summary>
		/// 绑定Unit
		/// </summary>
		/// <param name="unit"></param>
		public void BindUnit(Unit unit) {
			Unit = unit;
			int modelId = Unit.GetAttr(EAttrType.AttrModelId);
			if (!ConfigDataBase.Table<ModelTable>().TryGetRow(modelId, out _modelRow)) {
				throw new Exception("找不到Model配置");
			}
		}

		/// <summary>
		/// 加载资源代理
		/// </summary>
		public async UniTask LoadProxy() {
			_proxyType = Enum.Parse<EUOProxyType>(_modelRow.UOProxyType, true);
			string path = BattleSetting.GetUOProxyPath(_proxyType);
			_proxy = await UGameObjectPool.Instance.Get(path, Unit.MainCancelToken);
			if (_proxy == null) {
				throw new Exception($"Load UnityObjectProxy :{path} failed!");
			}

			//初始化
			var unitName = string.IsNullOrEmpty(Unit.DynamicName) ? "Unit" : Unit.DynamicName;
			_proxy.name = $"{unitName}:{Unit.Uid}";
			_proxy.transform.position = Unit.UnitTransform.Pos;
			_proxy.transform.rotation = Unit.UnitTransform.Rot;
			_proxy.transform.localScale = _modelRow.ModelScale == 0 ? Vector3.one : _modelRow.ModelScale * Vector3.one;

			//初始化层级
			_proxy.layer = BattleSetting.GetLayerMask(_modelRow.ProxyLayer.ToString());

			if (_proxy.TryGetComponent(out _physicsHandler)) {
				_physicsHandler.Unit = Unit;
				_physicsHandler.Set(_modelRow.P1, _modelRow.P2, _modelRow.P3);
				if (_modelRow.ColliderCenter.Count == 3) {
					_physicsHandler.SetCenter(new Vector3(_modelRow.ColliderCenter[0], _modelRow.ColliderCenter[1],
						_modelRow.ColliderCenter[2]));
				}
			}

			await loadActorModel();
		}

		/// <summary>
		/// 回收
		/// </summary>
		public void OnRecycle() {
			BattleManager.Instance.OnLateUpdate -= LateUpdate;
			foreach (var vfxGameObject in _vfxCache.Values) {
				UGameObjectPool.Instance.Recycle(vfxGameObject.VFXInfo.Path, vfxGameObject.GameObject);
			}

			_vfxCache.Clear();

			if (!string.IsNullOrEmpty(_actorModelPath) && _actorModelHandler) {
				UGameObjectPool.Instance.Recycle(_actorModelPath, _actorModelHandler.gameObject);
			}

			_actorModelPath = null;
			_actorModelHandler = null;

			if (_proxy) {
				UGameObjectPool.Instance.Recycle(BattleSetting.GetUOProxyPath(_proxyType), _proxy);
			}

			Unit = null;
			_proxy = null;
			_physicsHandler = null;
			_proxyType = 0;
		}

		private async UniTask loadActorModel() {
			var baseId = Unit.GetAttr(EAttrType.AttrResReplTplBaseId);
			var overrideId = Unit.GetAttr(EAttrType.AttrResReplTplBaseId);

			if (baseId + overrideId == 0) {
				return;
			}

			var baseModelPath = ResReplTplDateBase.Instance.GetModelPath(baseId);
			var overrideModelPath = ResReplTplDateBase.Instance.GetModelPath(overrideId);

			GameObject model = null;

			_actorModelPath = string.IsNullOrEmpty(overrideModelPath) ? baseModelPath : overrideModelPath;

			if (!string.IsNullOrEmpty(_actorModelPath)) {
				model = await UGameObjectPool.Instance.Get(_actorModelPath, _proxy.transform, Vector3.zero,
					Unit.MainCancelToken);
			}
			else {
				Debug.LogError($"Unit:{Unit} 加载ActorModel失败 ");
			}

			if (model == null || !model.TryGetComponent(out _actorModelHandler)) {
				Debug.LogError($"Unit:{Unit} 加载ActorModelHandel 失败 ");
			}
		}

		public void SyncTransform() {
			/*if (_physicsHandler != null) {
				//使用物理组件前进，
				_physicsHandler.Move(Unit.UnitTransform);
			}
			else {
				
			}*/
            _proxy.transform.localPosition = Unit.UnitTransform.Pos;
			_proxy.transform.localRotation = Unit.UnitTransform.Rot;
		}

		public async void AddVFXToWorld(VFXInfo vfxInfo) {
			if (string.IsNullOrEmpty(vfxInfo.Path)) {
				return;
			}
			
			var vfx = await UGameObjectPool.Instance.Get(vfxInfo.Path, null, vfxInfo.Pos, vfxInfo.Rot,
				Unit.MainCancelToken);

			if (vfx == null)
				return;
			
			_vfxCache.Add(vfxInfo.Uid, new VFXGameObject() { VFXInfo = vfxInfo, GameObject = vfx });
		}

		public async void AddVFXToUnit(VFXInfo vfxInfo, string bindBond, bool notfollowParentRot) {
			if (string.IsNullOrEmpty(vfxInfo.Path)) {
				return;
			}

			Transform parent = _proxy.transform;
			if (!string.IsNullOrEmpty(bindBond)) {
				if (_actorModelHandler == null || !_actorModelHandler.TryGetPoint(bindBond, out parent)) {
					Debug.Log($"{vfxInfo.Path} 找不到挂点！");
					parent = _proxy.transform;
				}
			}

			var vfx = await UGameObjectPool.Instance.Get(vfxInfo.Path, parent, vfxInfo.Pos, vfxInfo.Rot,
				Unit.MainCancelToken);

			if (vfx == null) return;

			if (notfollowParentRot) {
				vfx.transform.rotation = vfxInfo.Rot;
			}

			if (_vfxCache.Count == 0) {
				BattleManager.Instance.OnLateUpdate += LateUpdate;
			}

			_vfxCache.Add(vfxInfo.Uid, new VFXGameObject() { VFXInfo = vfxInfo, GameObject = vfx });
		}

		public void RemoveVFX(in VFXInfo vfxInfo) {
			if (_vfxCache.Remove(vfxInfo.Uid, out VFXGameObject vfxGO)) {
				UGameObjectPool.Instance.Recycle(vfxGO.VFXInfo.Path, vfxGO.GameObject);
			}

			if (_vfxCache.Count == 0) {
				BattleManager.Instance.OnLateUpdate -= LateUpdate;
			}
		}

		private void LateUpdate() {
			//保持特效不转
			foreach (var vfxGameObject in _vfxCache.Values) {
				if (vfxGameObject.NotFollowParentRot) {
					vfxGameObject.GameObject.transform.rotation = vfxGameObject.VFXInfo.Rot;
				}
			}
		}

		public void PlayAnimClip(string clipName) {
			_actorModelHandler.PlayAnim(clipName);
		}
	}
}