using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 伤害流程
    /// </summary>
    public class DamageManager : Singleton<DamageManager>
    {
        /// <summary>
        /// 造成伤害
        /// </summary>
        /// <param name="attackProxy">攻击者代理</param>
        /// <param name="hurtTarget">受击者的属性</param>
        /// <param name="damageSetting"></param>
        public void MakeDamage(UnitProxy attackProxy, Unit hurtTarget, DamageSetting damageSetting)
        {
            if (attackProxy == null || hurtTarget == null || damageSetting == null)
            {
                return;
            }
        }


        /*
        /// <summary>
        /// 初始化伤害信息
        /// </summary>
        private void initDamageInfo()
        {
            _damageInfo.SourceAbilityType = (EAbilityType)_binder.GetAttr(EAttrType.SourceAbilityType);
            _damageInfo.SourceAbilityConfigId = _binder.GetAttr(EAttrType.AttrSourceAbilityConfigId);
        }

        /// <summary>
        /// 收集增伤条件
        /// </summary>
        private void initDamageConfig()
        {
            _damageConfig.DamageType = (EDamageType)_damageRow.DamageType;
            _damageConfig.ElementType = (EDamageElementType)_damageRow.ElementType;
            _damageConfig.FormulaName = _damageRow.FormulaName;
            _damageConfig.ImpactValue = _damageRow.ImpactValue;
            _damageConfig.DamageRatio = _damageRow.DamageRatio;

            foreach (var addiId in _damageRow.AdditiveId)
            {
                var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
                var funcInfo = GPool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
                funcInfo.ConditionIds = damageAddi.ConditionIds;
                funcInfo.ConditionParams = damageAddi.ConditionParams;
                funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
                _damageConfig.AddiTypes.Add(funcInfo);
                _recyclePools.Add(funcInfo);
            }

            foreach (var multiId in _damageRow.MultiplyId)
            {
                var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
                var funcInfo = GPool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                _damageConfig.MultiTypes.Add(funcInfo);
                _recyclePools.Add(funcInfo);
            }
        }


        /// <summary>
        /// 生成HitDamageInfo
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private HitDamageInfo makeHitInfo(DamageResults results)
        {
            var hitInfo = new HitDamageInfo();
            hitInfo.SourceActorId = _attacker.Uid;
            hitInfo.SourceAbilityId = _damageInfo.SourceAbilityConfigId;
            hitInfo.DamageConfigId = _damageConfigId;
            hitInfo.HitBoxHitCount = _damageInfo.HitCount;
            hitInfo.HitTargetUid = _target.Uid;
            hitInfo.FinalDamageValue = results.DamageValue;
            hitInfo.IsCritical = results.IsCritical;
            hitInfo.IsImmunity = _target.GetAttr(EAttrType.AttrInvincible) > 0;
            hitInfo.IsKillTarget = results.DamageValue > _target.GetAttr(EAttrType.AttrHp);
            return hitInfo;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _damageConfigId = 0;
            _attacker = null;
            _target = null;
            _damageRow = null;
            _hasError = false;

            _damageInfo.Clear();
            _damageConfig.Clear();
            foreach (var info in _recyclePools)
            {
                GPool<DamageFuncInfo>.Pool.Recycle(info);
            }

            _recyclePools.Clear();
        }*/
    }
}