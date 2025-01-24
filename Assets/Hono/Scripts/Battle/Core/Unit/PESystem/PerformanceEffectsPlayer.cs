using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 演出效果播放器
    /// 内置 模型对象
    /// 内置 模型对象挂点管理
    /// 内置 动画状态机
    /// 内置 特效管理器
    /// 内置 音效管理器
    /// </summary>
    public class PerformanceEffectsPlayer : MonoBehaviour
    {
        [Title("特效相关")] 
        public List<Transform> EffectPoints = new();
        public Transform HeadPoint;
        public Transform CenterPoint;

        private readonly Dictionary<int, GameObject> _vfxDict = new();
        private readonly Dictionary<string, Transform> _effectPoints = new(20);
        private VFXComp _vfxComp;
        public PerformanceEffectController ModelController { get; private set; }
        protected bool SetupFinish { get; private set; } = false;

        private void Awake()
        {
            foreach (var point in EffectPoints)
            {
                _effectPoints.Add(point.name, point);
            }
        }

        public void OnInit(PerformanceEffectController modelController)
        {
            ModelController = modelController;
            if (ModelController.Self.TryGetComponent(out _vfxComp))
            {
                _vfxComp.VFXAdd += OnAddVFXObject;
                _vfxComp.VFXRemove += OnRemoveVFXObject;
                foreach (var vfxObject in _vfxComp.VFXDict)
                {
                   
                }
            }

            onSetupFinish();
            SetupFinish = true;
        }

        protected virtual void onSetupFinish() { }

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
            UPool.Instance.Recycle(ModelController.PETemplate.model, gameObject);
        }

        private void OnAddVFXObject(VFXObject vfxObject)
        {
           
        }

        private void OnRemoveVFXObject(VFXObject vfxObject)
        {
            if (_vfxDict.Remove(vfxObject.Uid, out GameObject vfx))
            {
               // UPool.Instance.Recycle(vfxObject.Setting.VFXPath, vfx);
            }
        }
    }
}