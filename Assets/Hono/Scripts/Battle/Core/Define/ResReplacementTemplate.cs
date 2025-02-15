using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle.Core
{
    public enum EPeType
    {
        VFX,
        Anim,
        Audio
    }

    /// <summary>
    /// 资源替换模板
    /// </summary>
    public class ResReplacementTemplate : SerializedScriptableObject
    {
        /// <summary>
        /// 模型路径
        /// </summary>
        public string model;

        /// <summary>
        /// 特效替换位
        /// </summary>
        public Dictionary<string, string> vfxReplacements = new();

        /// <summary>
        /// 音频替换位
        /// </summary>
        public Dictionary<string, string> audioReplacements = new();

        /// <summary>
        /// 动画替换位
        /// </summary>
        public Dictionary<string, string> animationReplacements = new();
    }

    public static class PETemplateEx
    {
        /// <summary>
        /// 获取模板中key值对应的资源路径
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTplPath(string key, EPeType type)
        {
            /*Dictionary<string, string> dict = null;
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
*/

            return null;
        }
    }
}