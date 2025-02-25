using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 资源替换模板
    /// </summary>
    [CreateAssetMenu(menuName = "战斗相关/ResReplacementTemplate")]
    public class SkinTemplate : SerializedScriptableObject
    {
        /// <summary>
        /// 模型路径
        /// </summary>
        [PrefabPath]
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