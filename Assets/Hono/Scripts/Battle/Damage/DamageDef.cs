using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [ShowOdinSerializedPropertiesInInspector]
    [CreateAssetMenu(menuName = "战斗编辑器/DamageConfig")]
    public class DamageItem : SerializedScriptableObject, IAllowedIndexing
    {
        public int Id;

        /// <summary>
        /// 加值类型
        /// </summary>
        [LabelText("加值参数：{{X, X, X}，{X, X}}")] [OdinSerialize]
        public List<DamageFuncInfo> AddiTypes = new List<DamageFuncInfo>();

        /// <summary>
        /// 乘值类型
        /// </summary>
        [LabelText("乘值参数：{{X, X, X}，{X, X}}")] [OdinSerialize]
        public List<DamageFuncInfo> MultiTypes = new List<DamageFuncInfo>();

        /// <summary>
        /// 大公式Name
        /// </summary>
        public string FormulaName;

        /// <summary>
        /// 冲击值
        /// </summary>
        public int ImpactValue;

        /// <summary>
        /// 元素类型
        /// </summary>
        public EDamageElementType ElementType;

        /// <summary>
        /// 伤害类型
        /// </summary>
        public EDamageType DamageType;

        public int ID => Id;
    }
}