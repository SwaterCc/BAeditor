using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 资源替换模板
    /// </summary>
    [CreateAssetMenu(menuName = "战斗相关/ResReplacementTemplate")]
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
}