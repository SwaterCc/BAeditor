#region

using System;
using System.Collections.Generic;
using XLua;

#endregion

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 命中数据
    /// </summary>
    public struct HitInfo
    {
        public Unit Attacker;
        public DamageTable.DamageRow DamageRow;
        public EDamageSourceType DamageSourceType;
        public int SourceAbilityId;
        public int HitCount;
    }

    [LuaCallCSharp]
    public struct DamageResult
    {
        /// <summary>
        ///     伤害最终值
        /// </summary>
        public int DamageValue;

        /// <summary>
        ///     是否暴击
        /// </summary>
        public bool IsCritical;
    }

    [LuaCallCSharp]
    public class DamagePipLine
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
        private SkillModifier _sourceSkillModifier;

        /// <summary>
        /// 攻击者记录
        /// </summary>
        private Unit _attacker;

        /// <summary>
        /// 本次伤害配置
        /// </summary>
        private DamageTable.DamageRow _damageRow;

        /// <summary>
        /// 伤害结果
        /// </summary>
        private DamageResult _damageResults;

        /// <summary>
        /// 初始化伤害相关数据
        /// </summary>
        /// <param name="hitInfo"></param>
        public void Init(HitInfo hitInfo)
        {
            _attacker = hitInfo.Attacker;
            _damageRow = hitInfo.DamageRow;
            DamageSourceType = hitInfo.DamageSourceType;
            SourceAbilityId = hitInfo.Attacker.Uid;
            if (DamageSourceType == EDamageSourceType.Skill)
            {
                _sourceSkillModifier = _attacker.GetComponent<CombatComp>().GetSkillModifier(SourceAbilityId);
            }

            HitUnitCount = hitInfo.HitCount;
            //收集伤害加值
            initDamageConfig();
        }

        private void initDamageConfig()
        {
            DamageType = (EDamageType)_damageRow.DamageType;
            FormulaName = _damageRow.FormulaName;
            DamageRatio = _damageRow.DamageRatio;

            foreach (var addiId in _damageRow.AdditiveId)
            {
                var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
                var funcInfo = GPool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
                funcInfo.ConditionIds = damageAddi.ConditionIds;
                funcInfo.ConditionParams = damageAddi.ConditionParams;
                funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
                AddiTypes.Add(funcInfo);
            }

            foreach (var multiId in _damageRow.MultiplyId)
            {
                var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
                var funcInfo = GPool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                MultiTypes.Add(funcInfo);
            }
        }

        /// <summary>
        /// 获取最终值，攻击者的属性会加上技能修改的额外属性
        /// </summary>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public int GetAttackerFinalAttr(EAttrType attrType)
        {
            if (_attacker == null)
            {
                return Int32.MinValue;
            }

            if (_sourceSkillModifier != null)
            {
                return _attacker.GetAttr(attrType) + _sourceSkillModifier.GetAttrModify(attrType);
            }

            return _attacker.GetAttr(attrType);
        }

        /// <summary>
        /// 获取技能修改的属性值
        /// </summary>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public int GetSkillModifierAttr(EAttrType attrType)
        {
            if (_sourceSkillModifier == null)
            {
                return Int32.MinValue;
            }

            return _sourceSkillModifier.GetAttrModify(attrType);
        }

        [LuaCallCSharp]
        public void SetDamageResult(DamageResult result)
        {
            _damageResults = result;
        }

        public DamageResult GetDamageResult()
        {
            return _damageResults;
        }

        public void Reset()
        {
            _attacker = null;
            DamageSourceType = EDamageSourceType.None;
            SourceAbilityId = 0;
            HitUnitCount = 0;
            FormulaName = null;
            DamageRatio = 0;
            DamageType = 0;
            foreach (var funcInfo in AddiTypes)
            {
                GPool<DamageFuncInfo>.Pool.Recycle(funcInfo);
            }

            AddiTypes.Clear();
            foreach (var funcInfo in MultiTypes)
            {
                GPool<DamageFuncInfo>.Pool.Recycle(funcInfo);
            }

            MultiTypes.Clear();
            _damageResults = default;
        }
    }
}