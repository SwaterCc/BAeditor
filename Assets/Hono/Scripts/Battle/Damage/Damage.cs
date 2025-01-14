using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class Damage
    {
        private int _damageConfigId;
        private readonly DamageInfo _damageInfo;
        private readonly DamageConfig _damageConfig;
        private DamageTable.DamageRow _damageRow;
        private readonly List<DamageFuncInfo> _recyclePools;
        private readonly Actor _binder;
        private Actor _attacker;
        private Actor _target;
        private bool _hasError;

        public Damage(Actor bindActor)
        {
            _binder = bindActor;
            _damageInfo = new DamageInfo();
            _damageConfig = new DamageConfig();
            _recyclePools = new List<DamageFuncInfo>(30);
        }

        /// <summary>
        /// 初始化打击点
        /// </summary>
        public void Init(Actor attacker, Actor target, int damageConfigId)
        {
            _attacker = attacker;
            _target = target;

            if (_attacker == null || _target == null)
            {
                _hasError = true;
            }

            _damageConfigId = damageConfigId;
            _hasError = ConfigManager.Table<DamageTable>().TryGet(_damageConfigId, out _damageRow);

            if (_hasError)
                return;

            initDamageInfo();
            initDamageConfig();
        }

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
                var funcInfo = APool<DamageFuncInfo>.Pool.Rent();
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
                var funcInfo = APool<DamageFuncInfo>.Pool.Rent();
                funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
                funcInfo.ConditionIds = damageMultiply.ConditionIds;
                funcInfo.ConditionParams = damageMultiply.ConditionParams;
                funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
                _damageConfig.MultiTypes.Add(funcInfo);
                _recyclePools.Add(funcInfo);
            }
        }

        /// <summary>
        /// 造成伤害(单线程)
        /// </summary>
        /// <param name="hitActorNumber">命中Actor的数量</param>
        /// <param name="hitCount">第几次命中</param>
        /// <param name="criticalFlag">必定暴击</param>
        /// <returns>返回伤害数据</returns>
        public HitDamageInfo MakeDamage(int hitActorNumber, int hitCount, bool criticalFlag)
        {
            if (_hasError) return default;

            _damageInfo.HitCount = hitActorNumber;
            _damageInfo.HitNumberCount = hitCount;
            _damageInfo.IsCriticalOnce = criticalFlag;
            var results = LuaInterface.GetDamageResults(_attacker, _target, _damageInfo, _damageConfig);
            //BattlePanel.ShowDamage(_target.Pos, results);
            return makeHitInfo(results);
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
                APool<DamageFuncInfo>.Pool.Recycle(info);
            }

            _recyclePools.Clear();
        }
    }
}