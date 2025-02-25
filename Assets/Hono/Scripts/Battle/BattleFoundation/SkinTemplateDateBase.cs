using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 替换模板数据库
    /// </summary>
    public class SkinTemplateDateBase : BattleFoundation<SkinTemplateDateBase>
    {
        private Dictionary<string, SkinTemplate> _dataBases = new();

        /// <summary>
        /// 加载
        /// </summary>
        public override async UniTask AsyncLoad()
        {
            var assets = await Addressables.LoadAssetsAsync<SkinTemplate>("aSkinTpl").ToUniTask();
            foreach (var asset in assets)
            {
                _dataBases.TryAdd(asset.name, asset);
            }
        }

        public string GetModelPath(string templateId)
        {
            if (string.IsNullOrEmpty(templateId)) return null;
            return !_dataBases.TryGetValue(templateId, out var template) ? null : template.model;
        }

        public string GetVFXPath(string templateId, string key)
        {
	        if (string.IsNullOrEmpty(templateId)) return null;
            if (string.IsNullOrEmpty(key)) return null;
            if (!_dataBases.TryGetValue(templateId, out var template))
            {
                return null;
            }

            return !template.vfxReplacements.TryGetValue(key, out var path) ? null : path;
        }

        public string GetAudioPath(string templateId, string key)
        {
	        if (string.IsNullOrEmpty(templateId)) return null;
            if (string.IsNullOrEmpty(key)) return null;
            if (!_dataBases.TryGetValue(templateId, out var template))
            {
                return null;
            }

            return !template.audioReplacements.TryGetValue(key, out var path) ? null : path;
        }

        public string GetAnimPath(string templateId, string key)
        {
	        if (string.IsNullOrEmpty(templateId)) return null;
            if (string.IsNullOrEmpty(key)) return null;
            if (!_dataBases.TryGetValue(templateId, out var template))
            {
                return null;
            }

            return !template.animationReplacements.TryGetValue(key, out var path) ? null : path;
        }
    }
}