#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle
{
    public class DamageConfig : ICPoolObject
    {
        /// <summary>
        ///     加值类型
        /// </summary>
        public List<DamageFuncInfo> AddiTypes = new(10);

        /// <summary>
        ///     乘值类型
        /// </summary>
        public List<DamageFuncInfo> MultiTypes = new(10);

        /// <summary>
        ///     大公式Name
        /// </summary>
        public string FormulaName;

        /// <summary>
        ///     冲击值
        /// </summary>
        public int ImpactValue;

        /// <summary>
        ///     傷害倍率
        /// </summary>
        public int DamageRatio;

        /// <summary>
        ///     元素类型
        /// </summary>
        public EDamageElementType ElementType;

        /// <summary>
        ///     伤害类型
        /// </summary>
        public EDamageType DamageType;

        public void Clear()
        {
            AddiTypes.Clear();
            MultiTypes.Clear();
            FormulaName = null;
            ImpactValue = 0;
            DamageRatio = 0;
            ElementType = 0;
            DamageType = 0;
        }
    }
}