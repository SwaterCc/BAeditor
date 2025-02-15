using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class UnityObjectProxy
    {
        private struct VFXGameObject
        {
            public VFXInfo VFXInfo;
            public GameObject GameObject;
        }

        /// <summary>
        /// 绑定的Unit
        /// </summary>
        private Unit _unit;

        /// <summary>
        /// 代理类型
        /// </summary>
        private EUnityObjectProxyType _proxyType;

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
        /// 绑定Unit
        /// </summary>
        /// <param name="unit"></param>
        public void BindUnit(Unit unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// 加载资源代理
        /// </summary>
        public async UniTask LoadProxy(EUnityObjectProxyType proxyType)
        {
            _proxyType = proxyType;
            string path = getProxyPath();
            _proxy = await UGameObjectPool.Instance.Get(path, _unit.MainCancelToken);
            if (_proxy == null)
            {
                throw new Exception($"Load UnityObjectProxy :{path} failed!");
            }

            //初始化位置
            _proxy.transform.localPosition = _unit.UnitTransform.Pos;
            _proxy.transform.localRotation = _unit.UnitTransform.Rot;

            if (!_proxy.TryGetComponent(out _physicsHandler))
            {
                Debug.Log("找不到物理组件");
            }

            await loadActorModel();
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void OnRecycle()
        {
            foreach (var vfxGameObject in _vfxCache.Values)
            {
                UGameObjectPool.Instance.Recycle(vfxGameObject.VFXInfo.Path, vfxGameObject.GameObject);
            }

            _vfxCache.Clear();

            if (!string.IsNullOrEmpty(_actorModelPath) && _actorModelHandler)
            {
                UGameObjectPool.Instance.Recycle(_actorModelPath, _actorModelHandler.gameObject);
            }

            _actorModelPath = null;
            _actorModelHandler = null;

            if (_proxy)
            {
                UGameObjectPool.Instance.Recycle(getProxyPath(), _proxy);
            }

            _unit = null;
            _proxy = null;
            _physicsHandler = null;
            _proxyType = 0;
        }

        private string getProxyPath()
        {
            string path = "";
            switch (_proxyType)
            {
                case EUnityObjectProxyType.OnlyTransform:
                    break;
                case EUnityObjectProxyType.Collider:
                    break;
                case EUnityObjectProxyType.Actor:
                    break;
            }

            return path;
        }

        private async UniTask loadActorModel()
        {
            var baseId = _unit.GetAttr(EAttrType.AttrResReplTplBaseId);
            var overrideId = _unit.GetAttr(EAttrType.AttrResReplTplBaseId);

            if (baseId + overrideId == 0)
            {
                return;
            }

            var baseModelPath = ResReplTplDB.Instance.GetModelPath(baseId);
            var overrideModelPath = ResReplTplDB.Instance.GetModelPath(overrideId);

            GameObject model = null;

            _actorModelPath = string.IsNullOrEmpty(overrideModelPath) ? baseModelPath : overrideModelPath;

            if (!string.IsNullOrEmpty(_actorModelPath))
            {
                model = await UGameObjectPool.Instance.Get(_actorModelPath, _unit.MainCancelToken);
            }
            else
            {
                Debug.LogError($"Unit:{_unit} 加载ActorModel失败 ");
            }

            if (model == null || !model.TryGetComponent(out _actorModelHandler))
            {
                Debug.LogError($"Unit:{_unit} 加载ActorModelHandel 失败 ");
            }
        }

        public Vector3 SyncVelocity(in Vector3 velocity, in float dt)
        {
            if (_physicsHandler != null)
            {
                _physicsHandler.Move(velocity);
            }
            else
            {
                _proxy.transform.localPosition = velocity * dt;
            }

            return _proxy.transform.localPosition;
        }

        public void SyncRot(in Quaternion rot)
        {
            _proxy.transform.localRotation = rot;
        }

        public async void AddVFX(VFXInfo vfxInfo)
        {
            if (!string.IsNullOrEmpty(vfxInfo.Path))
            {
                return;
            }

            Transform parent = null;
            if (!vfxInfo.Setting.isWorldVFX)
            {
                if (!_actorModelHandler.TryGetPoint(vfxInfo.Setting.boneName, out parent))
                {
                    Debug.LogError("找不到挂点！");
                    parent = _proxy.transform;
                }
            }

            var vfx = await UGameObjectPool.Instance.Get(vfxInfo.Path, parent, vfxInfo.Pos, vfxInfo.Rot,
                                                         _unit.MainCancelToken);

            if (vfx == null) return;

            if (vfxInfo.Setting.notFollowParentRot)
            {
                vfx.transform.rotation = vfxInfo.Rot;
            }

            _vfxCache.Add(vfxInfo.Uid, new VFXGameObject() { VFXInfo = vfxInfo, GameObject = vfx });
        }

        public void RemoveVFX(in VFXInfo vfxInfo)
        {
            if (_vfxCache.Remove(vfxInfo.Uid, out VFXGameObject vfxGO))
            {
                UGameObjectPool.Instance.Recycle(vfxGO.VFXInfo.Path, vfxGO.GameObject);
            }
        }

        private void LateUpdate()
        {
            //保持特效不转
            foreach (var vfxGameObject in _vfxCache.Values)
            {
                if (vfxGameObject.VFXInfo.Setting.notFollowParentRot)
                {
                    vfxGameObject.GameObject.transform.rotation = vfxGameObject.VFXInfo.Rot;
                }
            }
        }

        public void PlayAnimClip(string clipName)
        {
            _actorModelHandler.PlayAnim(clipName);
        }
    }
}