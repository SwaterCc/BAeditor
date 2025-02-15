using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 替换模板数据库
    /// </summary>
    public class ResReplTplDB : Singleton<ResReplTplDB>
    {
        private Dictionary<int, ResReplacementTemplate> _dataBases = new();
        
        /// <summary>
        /// 加载
        /// </summary>
        public async void Load()
        {
            
        }
        
        public string GetModelPath(int templateId)
        {
            if (templateId <= 0) return null;
            return !_dataBases.TryGetValue(templateId, out var template) ? null : template.model;
        }

        public string GetVFXPath(int templateId, string key)
        {
            if (templateId <= 0) return null;
            if (string.IsNullOrEmpty(key)) return null;
            if (!_dataBases.TryGetValue(templateId, out var template))
            {
                return null;
            }

            return !template.vfxReplacements.TryGetValue(key, out var path) ? null : path;
        }
        
        public string GetAudioPath(int templateId, string key)
        {
            if (templateId <= 0) return null;
            if (string.IsNullOrEmpty(key)) return null;
            if (!_dataBases.TryGetValue(templateId, out var template))
            {
                return null;
            }

            return !template.audioReplacements.TryGetValue(key, out var path) ? null : path;
        }
        
        public string GetAnimPath(int templateId, string key)
        {
            if (templateId <= 0) return null;
            if (string.IsNullOrEmpty(key)) return null;
            if (!_dataBases.TryGetValue(templateId, out var template))
            {
                return null;
            }

            return !template.animationReplacements.TryGetValue(key, out var path) ? null : path;
        }
    }
}