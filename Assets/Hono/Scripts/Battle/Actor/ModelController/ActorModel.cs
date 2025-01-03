#region

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace Hono.Scripts.Battle
{
    public class ActorModel : MonoBehaviour
    {
        [Title("基础信息")] 
        [InfoBox("如果是需要静态创建的Actor且需要指定Uid请给予初始值，该uid全局不变")]
        public int ActorUid;

        [InfoBox("如果是需要静态创建的Actor需要指定类型")] 
        public EActorType ActorType;
        
        [Title("特效相关")] 
        public List<Transform> EffectPoints = new();
        public Transform HeadPoint;
        public Transform CenterPoint;

        private readonly Dictionary<int, GameObject> _vfxDict = new();
        private readonly Dictionary<string, Transform> _effectPoints = new(20);
        private ActorLogic.VFXComp _vfxComp;
        protected ModelController ModelController { get; private set; }
        protected bool SetupFinish { get; private set; } = false;

        private void Awake()
        {
            foreach (var point in EffectPoints)
            {
                _effectPoints.Add(point.name, point);
            }
        }

        public void Setup(ModelController modelController)
        {
            ModelController = modelController;
            if (ModelController.Self.Logic.TryGetComponent(out _vfxComp))
            {
                _vfxComp.VFXAdd += OnAddVFXObject;
                _vfxComp.VFXRemove += OnRemoveVFXObject;
                foreach (var vfxObject in _vfxComp.VFXDict)
                {
                    loadVfx(vfxObject.Value).Forget();
                }
            }

            onSetupFinish();
            SetupFinish = true;
        }

        protected virtual void onSetupFinish() { }

#if UNITY_EDITOR
        [Button("激活")]
        public void ActorCreate(int configId)
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("仅Unity Playing可用");
                return;
            }

            ActorManager.Instance.CreateActor(ActorType, configId, this);
        }
#endif

        public void OnEnterScene() { }

        public void OnExitScene() { }

        public void OnTick(float dt)
        {
            foreach (KeyValuePair<int, VFXObject> obj in _vfxComp.VFXDict)
            {
                if (obj.Value.Setting.VFXBindType != EVFXType.FollowActor)
                {
                    continue;
                }

                if (!_vfxDict.TryGetValue(obj.Key, out GameObject vfx))
                {
                    continue;
                }

                if (vfx == null)
                {
                    continue;
                }

                vfx.transform.position = obj.Value.Pos;
            }
        }

        public void Recycle()
        {
            UObjectPool.Instance.Recycle(ModelController.ModelPath, gameObject);
        }

        private void OnAddVFXObject(VFXObject vfxObject)
        {
            loadVfx(vfxObject).Forget();
        }

        private void OnRemoveVFXObject(VFXObject vfxObject)
        {
            if (_vfxDict.Remove(vfxObject.Uid, out GameObject vfx))
            {
                UObjectPool.Instance.Recycle(vfxObject.Setting.VFXPath, vfx);
            }
        }

        private async UniTask loadVfx(VFXObject vfxObject)
        {
            if (!UObjectPool.Instance.TryGet(vfxObject.Setting.VFXPath, out GameObject vfx))
            {
                try
                {
                    vfx = await Addressables.LoadAssetAsync<GameObject>(vfxObject.Setting.VFXPath)
                        .ToUniTask(cancellationToken: ModelController.MainCancelToken.Token);

                    Transform parent = null;
                    if (vfxObject.Setting.VFXBindType == EVFXType.BindActorBone)
                    {
                        if (!_effectPoints.TryGetValue(vfxObject.Setting.BoneName, out parent))
                        {
                            parent = transform;
                        }
                    }

                    vfx = Instantiate(vfx, parent);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return;
                }
            }

            vfx.transform.localPosition = vfxObject.Pos;
            vfx.transform.localRotation = vfxObject.Rot;
            vfx.transform.localScale = vfxObject.Scale;
            _vfxDict.Add(vfxObject.Uid, vfx);
        }
    }
}