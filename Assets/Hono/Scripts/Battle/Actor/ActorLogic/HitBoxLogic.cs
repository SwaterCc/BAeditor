using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle {
	/// <summary>
	/// 打击点目前是每隔一段时间对打击目标结算一次伤害
	/// </summary>
	public class HitBoxLogic : ActorLogic {
		private HitBoxData _hitBoxData;
		private float _intervalDuration;
		private int _curCount;

		private int _sourceActorId;
		private int _sourceAbilityConfigId;
		private int _targetUid;
		private EAbilityType _sourceAbilityType;
		private Action<Actor, DamageInfo> _hitProcess;
		private FilterSetting _filterSetting;

		private readonly Dictionary<BeHurtComp, int> _hitCountDict = new();

		public HitBoxLogic(Actor actor, ActorLogicTable.ActorLogicRow logicData) : base(actor, logicData) {
			_filterSetting = new FilterSetting();
		}

		protected override void setupAttrs() { }

		protected override void onInit() {
			_sourceActorId = GetAttr<int>(ELogicAttr.AttrSourceActorUid);
			_sourceAbilityConfigId = GetAttr<int>(ELogicAttr.AttrSourceAbilityConfigId);
			_sourceAbilityType = AssetManager.Instance.GetData<AbilityData>(_sourceAbilityConfigId).Type;
			_hitBoxData = (HitBoxData)(_variables.Get("hitBoxData"));

			var attacker = ActorManager.Instance.GetActor(_sourceActorId);
			_targetUid = (int)(_variables.Get("targetUid"));
			var target = ActorManager.Instance.GetActor(_targetUid);
			var pos = target.Logic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
			SetAttr(ELogicAttr.AttrPosition, pos, false);
			SetAttr(ELogicAttr.AttrFaction, attacker.Logic.GetAttr<int>(ELogicAttr.AttrFaction), false);

			_intervalDuration = _hitBoxData.Interval;
			_curCount = 0;

			switch (_hitBoxData.HitType) {
				case EHitType.Aoe:
					_hitProcess = aoeHit;
					break;
				case EHitType.Single:
					_hitProcess = singleHit;
					break;
			}

			if (_sourceAbilityType == EAbilityType.Skill && _hitBoxData.HitType == EHitType.Aoe) {
				if (_hitBoxData.UseCustomFilter) {
					_filterSetting = _hitBoxData.FilterSetting;
				}
				else {
					//来源于技能的打击点要继承技能的基础筛选条件
					_filterSetting.OpenBoxCheck = true;
					_filterSetting.BoxData = _hitBoxData.AoeData;
					var skillData = AssetManager.Instance.GetData<SkillData>(_sourceAbilityConfigId);
					var range = new FilterRange() { RangeType = EFilterRangeType.Faction, };

					switch (skillData.SkillTargetType) {
						case ESkillTargetType.Enemy:
							range.Value = (int)EFactionType.Enemy;
							break;
						case ESkillTargetType.Friendly:
							range.Value = (int)EFactionType.Friendly;
							break;
					}

					_filterSetting.Ranges.Add(range);
				}
			}
		}

		protected override void registerChildComponents() { }

		private void hitCounter(BeHurtComp beHurtComp) {
			if (_hitCountDict.TryGetValue(beHurtComp, out var count)) {
				if (count < _hitBoxData.ValidCount) {
					++count;
				}
			}
			else {
				_hitCountDict.Add(beHurtComp, 1);
			}
		}

		private HitInfo makeHitInfo(DamageResults damageResults) {
			var hitInfo = new HitInfo();
			hitInfo.SourceActorId = _sourceActorId;
			hitInfo.SourceAbilityConfigId = _sourceAbilityConfigId;
			hitInfo.SourceAbilityType = (int)_sourceAbilityType;
			hitInfo.SourceAbilityUId = (int)Actor.Variables.Get("abilityUid");
			hitInfo.ParseDamageResult(damageResults);
			hitInfo.DamageConfigId = _hitBoxData.DamageConfigId;
			hitInfo.SourceAbilityType = (int)_sourceAbilityType;
			return hitInfo;
		}

		private DamageConfig makeDamageConfig() {
			var damageConfig = new DamageConfig();
			var damage = ConfigManager.Table<DamageTable>().Get(_hitBoxData.DamageConfigId);
			damageConfig.DamageType = (EDamageType)damage.DamageType;
			damageConfig.ElementType = (EDamageElementType)damage.ElementType;
			damageConfig.FormulaName = damage.FormulaName;
			damageConfig.ImpactValue = damage.ImpactValue;
			Debug.Log($"damageConfig.ImpactValue {damageConfig.ImpactValue}");
			foreach (var addiId in damage.AdditiveId) {
				var damageAddi = ConfigManager.Table<DamageAdditiveTable>().Get(addiId);
				var funcInfo = new DamageFuncInfo();
				funcInfo.ValueFuncName = damageAddi.ApplyFuncName;
				funcInfo.ConditionIds = damageAddi.ConditionIds;
				funcInfo.ConditionParams = damageAddi.ConditionParams;
				funcInfo.ValueParams.AddRange(damageAddi.DamageValue);
				damageConfig.AddiTypes.Add(funcInfo);
			}

			foreach (var multiId in damage.MultiplyId) {
				var damageMultiply = ConfigManager.Table<DamageMultiplyTable>().Get(multiId);
				var funcInfo = new DamageFuncInfo();
				funcInfo.ValueFuncName = damageMultiply.ApplyFuncName;
				funcInfo.ConditionIds = damageMultiply.ConditionIds;
				funcInfo.ConditionParams = damageMultiply.ConditionParams;
				funcInfo.ValueParams.AddRange(damageMultiply.DamageValue);
				damageConfig.MultiTypes.Add(funcInfo);
			}

			return damageConfig;
		}

		private void singleHit(Actor attacker, DamageInfo damageInfo) {
			var target = ActorManager.Instance.GetActor(_targetUid);
			if (target == null) return;
			if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp)) return;
			hitCounter(beHurtComp);
			var damageItem = makeDamageConfig();
			var res = LuaInterface.GetDamageResults(attacker, target, damageInfo, damageItem);
			var hitInfo = makeHitInfo(res);
			hitInfo.HitTargetUid = _targetUid;
			BattleEventManager.Instance.TriggerEvent(EBattleEventType.OnHit, hitInfo);
			beHurtComp.OnBeHurt(hitInfo);
		}

		private void aoeHit(Actor attacker, DamageInfo damageInfo) {
			//aoe会根据目标坐标二次筛选
			var targetIds = ActorManager.Instance.UseFilter(Actor, _filterSetting);
			damageInfo.HitCount = targetIds.Count;
			foreach (var targetUid in targetIds) {
				var target = ActorManager.Instance.GetActor(targetUid);
				if (target == null) return;
				if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp)) return;
				hitCounter(beHurtComp);
				var damageItem = makeDamageConfig();
				var res = LuaInterface.GetDamageResults(attacker, target, damageInfo, damageItem);
				var hitInfo = makeHitInfo(res);
				hitInfo.HitTargetUid = targetUid;

				if (beHurtComp.Actor.GetAttr<int>(ELogicAttr.AttrInvincible) != 0) {
					Debug.Log($"actor uid: {beHurtComp.Actor.Uid}  是无敌的");
					hitInfo.IsImmunity = true;
				}

				if (beHurtComp.Actor.GetAttr<int>(ELogicAttr.AttrHp) <= 0) {
					hitInfo.IsKillTarget = true;
				}

				BattleEventManager.Instance.TriggerEvent(EBattleEventType.OnHit, hitInfo);
				beHurtComp.OnBeHurt(hitInfo);
			}
		}

		private void onHit() {
			var attacker = ActorManager.Instance.GetActor(_sourceActorId);

			var damageInfo = new DamageInfo();
			damageInfo.SourceActorId = _sourceActorId;
			damageInfo.SourceAbilityConfigId = _sourceAbilityConfigId;
			damageInfo.SourceAbilityType = _sourceAbilityType;
			var tags = (List<int>)_variables.Get("abilityTags");
			if (tags != null) {
				damageInfo.Tags.AddRange(tags);
			}
			else {
				Debug.Log("tag == null");
			}

			var abilityData = AssetManager.Instance.GetData<AbilityData>(_sourceAbilityConfigId);
			switch (abilityData.Type) {
				case EAbilityType.Skill:
					var skillData = AssetManager.Instance.GetData<SkillData>(_sourceAbilityConfigId);
					damageInfo.BaseDamagePer = skillData.SkillDamageBasePer;
					break;
				case EAbilityType.Buff:
					var buffData = AssetManager.Instance.GetData<BuffData>(_sourceAbilityConfigId);
					damageInfo.BaseDamagePer = buffData.BuffDamageBasePer;
					break;
			}

			for (int i = 0; i < _hitBoxData.OnceHitDamageCount; i++) {
				_hitProcess.Invoke(attacker, damageInfo);
			}
		}

		protected override void onTick(float dt) {
			var interval = _curCount == 0 ? _hitBoxData.FirstInterval : _hitBoxData.Interval;

			if (_intervalDuration > interval) {
				++_curCount;
				_intervalDuration = 0;
				onHit();
			}

			_intervalDuration += dt;

			if (_curCount >= _hitBoxData.MaxCount) {
				ActorManager.Instance.RemoveActor(this.Uid);
			}
		}

		protected override void onDestroy() { }
	}
}