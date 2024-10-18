using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle {
	public class VFXModelHandler : MonoBehaviour {
		public ActorModel model;
		private ActorLogic.VFXComp _vfxComp;
		public List<Transform> EffectPointList = new();
		public Transform HeadPoint;
		private readonly Dictionary<int, GameObject> _vfxShowDict = new();
		private readonly Dictionary<string, Transform> _effectPoints = new();
		private bool _hasError;

		public void Awake() {
			if (!TryGetComponent(out model)) {
				model = gameObject.AddComponent<ActorModel>();
			}
		}

		public void Start() {
			Actor actor;
			if (model.ActorType != EActorType.BattleLevelController) {
				actor = ActorManager.Instance.GetActor(model.ActorUid);
			}
			else {
				actor = BattleManager.BattleController.Actor;
			}
			
			if (actor == null || !actor.Logic.TryGetComponent(out _vfxComp)) {
				_hasError = true;
				Debug.LogError($"uid {model.ActorUid} Get VFXComp failed!");
			}

			if (!_hasError) {
				foreach (var point in EffectPointList) {
					_effectPoints.Add(point.name, point);
				}

				List<UniTask> tasks = new();
				foreach (var obj in _vfxComp.VFXObjects) {
					tasks.Add(loadGameObject(obj.Value));
				}

				_vfxComp.VFXAdd += OnAddVFXObject;
				_vfxComp.VFXRemove += OnRemoveVFXObject;

				UniTask.WhenAll(tasks);
			}
		}

		private async UniTask loadGameObject(VFXObject vfxObject) {
			try {
				var obj = await Addressables.LoadAssetAsync<GameObject>(vfxObject.Setting.VFXPath);
				if (vfxObject.Setting.VFXBindType == EVFXType.BindActorBone) {
					if (_effectPoints.TryGetValue(vfxObject.Setting.BoneName, out var parent)) {
						obj = Instantiate(obj, parent);
					}
					else {
						obj = Instantiate(obj, transform);
					}

					obj.transform.localPosition = vfxObject.Pos;
					obj.transform.localRotation = vfxObject.Rot;
				}
				else {
					obj = Instantiate(obj, vfxObject.Pos, vfxObject.Rot);
				}

				obj.transform.localScale = vfxObject.Scale;

				_vfxShowDict.Add(vfxObject.Uid, obj);
			}
			catch (KeyNotFoundException e) {
				Debug.LogError(e);
			}
			catch (Exception e) {
				Debug.LogError(e);
			}
		}

		public void Update() {
			if (_hasError) return;
			foreach (var obj in _vfxComp.VFXObjects) {
				if (obj.Value.Setting.VFXBindType == EVFXType.FollowActor) {
					if (_vfxShowDict.TryGetValue(obj.Key, out var vfx)) {
						vfx.transform.position = obj.Value.Pos;
					}
				}
			}
		}

		private void OnAddVFXObject(VFXObject vfxObject) {
			loadGameObject(vfxObject).Forget();
		}

		private void OnRemoveVFXObject(VFXObject vfxObject) {
			if (_vfxShowDict.Remove(vfxObject.Uid, out var obj)) {
				Destroy(obj);
			}
		}
	}
}