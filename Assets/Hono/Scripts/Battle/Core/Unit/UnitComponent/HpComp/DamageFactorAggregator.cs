namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 伤害收集器
    /// </summary>
    public class DamageFactorAggregator
    {
        private DamageStatIntegrator _statIntegrator;


        /// <summary>
        /// 收集增伤条件
        /// </summary>
        private void initDamageConfig()
        {
            _statIntegrator.DamageType = (EDamageType)_damageRow.DamageType;
            _statIntegrator.ElementType = (EDamageElementType)_damageRow.ElementType;
            _statIntegrator.FormulaName = _damageRow.FormulaName;
            _statIntegrator.ImpactValue = _damageRow.ImpactValue;
            _statIntegrator.DamageRatio = _damageRow.DamageRatio;

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
    }
}