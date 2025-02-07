using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Performance Effect Templates 演出效果模板
    /// </summary>
    [Serializable]
    public class PETemplate
    {
        /// <summary>
        /// 模型路径
        /// </summary>
        public string model;

        /// <summary>
        /// 特效替换位
        /// </summary>
        public Dictionary<string, string> vfxs = new();

        /// <summary>
        /// 音频替换位
        /// </summary>
        public Dictionary<string, string> audios = new();

        /// <summary>
        /// 动画替换位
        /// </summary>
        public Dictionary<string, string> anims = new();
    }
}