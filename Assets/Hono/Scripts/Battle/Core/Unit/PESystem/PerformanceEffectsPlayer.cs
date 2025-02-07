using System;
using System.Collections.Generic;
using System.Threading;
using Hono.Scripts.Battle.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 演出效果播放器
    /// 内置 模型对象
    /// 内置 模型对象挂点管理
    /// 内置 动画播放器
    /// 内置 特效管理器
    /// 内置 音效管理器
    /// </summary>
    public partial class PerformanceEffectsPlayer : MonoBehaviour
    {
        public enum EPeType
        {
            VFX,
            Anim,
            Audio
        }
        
        public GameObject model;
        
        public PEModelHandler ModelHandler { get; private set; }

        /// <summary>
        /// 基础模板
        /// </summary>
        private PETemplate _baseTemplate;

        /// <summary>
        /// 复写模板
        /// </summary>
        private PETemplate _overrideTemplate;
        private ActorModelController _modelController;
        /// <summary>
        /// 特效播放器
        /// </summary>
        private VFXPlayer _vfxPlayer;

        public async void LoadPE(ActorModelController modelController)
        {
            _modelController = modelController;
            
            //加载模型
            model = await UPool.Instance.Get(GetPEModelPath(), modelController.MainCancelToken);
            if (model != null)
            {
                ModelHandler = model.GetComponent<PEModelHandler>();
                _vfxPlayer ??= new VFXPlayer(this);
                _vfxPlayer.BindVFXComp();
            }
        }

        public void SetModelActive(bool active)
        {
            model?.SetActive(active);
        }
        
        public void SetBasePETemplate(PETemplate tpl)
        {
            _baseTemplate = tpl;
        }

        public void SetOverridePETemplate(PETemplate tpl)
        {
            _overrideTemplate = tpl;
        }

        public string GetPEModelPath()
        {
            return _overrideTemplate == null ? _baseTemplate.model : _overrideTemplate.model;
        }

        public string GetTplPath(string key, EPeType type)
        {
            Dictionary<string, string> dict = null;
            switch (type)
            {
                case EPeType.VFX:
                    dict = _overrideTemplate == null ? _baseTemplate.vfxs : _overrideTemplate.vfxs;
                    break;
                case EPeType.Anim:
                    dict = _overrideTemplate == null ? _baseTemplate.anims : _overrideTemplate.anims;
                    break;
                case EPeType.Audio:
                    dict = _overrideTemplate == null ? _baseTemplate.audios : _overrideTemplate.audios;
                    break;
            }

            if (dict == null)
                return null;

            if (dict.TryGetValue(key, out var path))
            {
                return path;
            }

            switch (type)
            {
                case EPeType.VFX:
                    dict = _baseTemplate.vfxs;
                    break;
                case EPeType.Anim:
                    dict = _baseTemplate.anims;
                    break;
                case EPeType.Audio:
                    dict = _baseTemplate.audios;
                    break;
            }

            if (dict.TryGetValue(key, out path))
            {
                return path;
            }


            return null;
        }

        public void OnTick(float dt) { }

        public void Clear()
        {
          
        }
    }

    public partial class PerformanceEffectsPlayer
    {
        public class VFXPlayer
        {
            public PerformanceEffectsPlayer PEPlayer { get; }

            private VFXComp _vfxComp;
            
            public VFXPlayer(PerformanceEffectsPlayer pePlayer)
            {
                PEPlayer = pePlayer;
            }

            public void BindVFXComp()
            {
                PEPlayer._modelController.Self.TryGetComponent(out _vfxComp);
                _vfxComp.VFXAdd += onVFXAdd;
                _vfxComp.VFXRemove += onVFXRemove;
                foreach (var VARIABLE in _vfxComp.VFXDict)
                {
                    
                }
            }

            private void onVFXAdd(VFXInfo vfxInfo)
            {
                
            }

            private void onVFXRemove(VFXInfo vfxInfo)
            {
                
            }
            
            public void Tick(float dt)
            {
            
            }
        }
    } 
}