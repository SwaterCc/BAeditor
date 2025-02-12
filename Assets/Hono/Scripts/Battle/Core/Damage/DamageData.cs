#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.Core
{
    public class DamageStatIntegrator
    {
        /// <summary>
        /// 来源ability类型
        /// </summary>
        public EDamageSourceType DamageSourceType { get; private set; }

        /// <summary>
        /// 来源Id,如果是buff就是buffId，如果是技能就是技能Id，子弹则是子弹Id
        /// </summary>
        public int SourceAbilityId { get; private set; }

        /// <summary>
        /// 一共命中了几个目标
        /// </summary>
        public int HitUnitCount { get; private set; }

        /// <summary>
        /// 收集所有的伤害加值类型
        /// </summary>
        public readonly List<DamageFuncInfo> AddiTypes = new(10);

        /// <summary>
        /// 收集所有的伤害乘值类型
        /// </summary>
        public readonly List<DamageFuncInfo> MultiTypes = new(10);

        /// <summary>
        /// 大公式Name
        /// </summary>
        public string FormulaName { get; private set; }

        /// <summary>
        /// 最终伤害倍率
        /// </summary>
        public int DamageRatio { get; private set; }

        /// <summary>
        /// 伤害类型
        /// </summary>
        public EDamageType DamageType { get; private set; }
        
        /// <summary>
        /// 来源技能属性修改器
        /// </summary>
        public SkillModifier SkillModifier { get; private set; }

        /// <summary>
        /// 攻击者记录
        /// </summary>
        private Unit _attacker;
        
        public void Init(Unit attacker,Unit target,)
        {
            
        }
        
        public int GetAttackFinalAttr(EAttrType attrType)
        {
            if (_attacker != null)
            {
                return _attacker.GetAttr(attrType) ;
            }

            return _attrSnapshot.GetValueOrDefault(attrType, 0);
        }
        
        private void initDamageConfig()
        {
            DamageType = (EDamageType)_damageRow.DamageType;
            ElementType = (EDamageElementType)_damageRow.ElementType;
            FormulaName = _damageRow.FormulaName;
            ImpactValue = _damageRow.ImpactValue;
            DamageRatio = _damageRow.DamageRatio;

            foreach (var addiId in _damageRow.AdditiveId)
            {
                var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
                var funcInfo = GPool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
                funcInfo.ConditionIds = damageAddi.ConditionIds;
                funcInfo.ConditionParams = damageAddi.ConditionParams;
                funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
                _statIntegrator.AddiTypes.Add(funcInfo);
            }

            foreach (var multiId in _damageRow.MultiplyId)
            {
                var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
                var funcInfo = GPool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                _statIntegrator.MultiTypes.Add(funcInfo);
            }
        }

       

        public void Reset()
        {
            _attacker = null;
            DamageSourceType = EDamageSourceType.None;
            SourceAbilityId = 0;
            HitUnitCount = 0;
            AddiTypes.Clear();
            MultiTypes.Clear();
            FormulaName = null;
            DamageRatio = 0;
            DamageType = 0;
        }
    }
}