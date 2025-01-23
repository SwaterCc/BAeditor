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
        [Serializable]
        public struct KeyPathPair
        {
            public string key;
            public string path;
        }
        
        /// <summary>
        /// 模型路径
        /// </summary>
        public string model;
        
        /// <summary>
        /// 特效替换位
        /// </summary>
        public List<KeyPathPair> vfxs = new();
        
        /// <summary>
        /// 音频替换位
        /// </summary>
        public List<KeyPathPair> audios = new();
        
        /// <summary>
        /// 动画替换位
        /// </summary>
        public List<KeyPathPair> anims = new();
    }
}