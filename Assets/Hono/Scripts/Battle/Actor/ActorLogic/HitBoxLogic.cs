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

		public HitBoxLogic(Actor actor) : base(actor) {
			_filterSetting = new FilterSetting();
		}

		protected override void setupAttrs()
		{
			SetAttr(ELogicAttr.AttrUnselectable, 1, false);
		}

		protected override void onInit() {
			_sourceActorId = GetAttr<int>(ELogicAttr.AttrSourceActorUid);
			_sourceAbilityConfigId = GetAttr<int>(ELogicAttr.AttrSourceAbilityConfigId);
			_sourceAbilityType = AssetManager.Instance.GetData<AbilityData>(_sourceAbilityConfigId).Type;
			_hitBoxData = (HitBoxData)(Variables.Get("hitBoxData"));

			var attacker = ActorManager.Instance.GetActor(_sourceActorId);
			_targetUid = (int)(Variables.Get("targetUid"));
			var target = ActorManager.Instance.GetActor(_targetUid);
			var pos = target.GetAttr<Vector3>(ELogicAttr.AttrPosition);
			SetAttr(ELogicAttr.AttrPosition, pos, false);
			SetAttr(ELogicAttr.AttrRot, attacker.GetAttr<Quaternion>(ELogicAttr.AttrRot), false);
			
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

			_filterSetting = _hitBoxData.FilterSetting;
		}
		
		protected override void setupComponents() { }

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

		private HitInfo makeHitInfo() {
			var hitInfo = new HitInfo();
			hitInfo.SourceActorId = _sourceActorId;
			hitInfo.SourceAbilityConfigId = _sourceAbilityConfigId;
			hitInfo.SourceAbilityType = (int)_sourceAbilityType;
			hitInfo.SourceAbilityUId = _sourceAbilityConfigId;
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
			damageConfig.DamageRatio = damage.DamageRatio;
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
			var hitInfo = makeHitInfo();
			BattleEventManager.Instance.TriggerActorEvent(_sourceActorId, EBattleEventType.OnHit, hitInfo);
			
			var hitDamageInfo = new HitDamageInfo(hitInfo);
			hitDamageInfo.ParseDamageResult(res);
			hitDamageInfo.HitTargetUid = _targetUid;
			hitInfo.HitBoxHitCount = 1;
			hitDamageInfo.IsKillTarget = (target.GetAttr<int>(ELogicAttr.AttrHp) - res.DamageValue) <= 0;
			BattleEventManager.Instance.TriggerActorEvent(_sourceActorId, EBattleEventType.OnHitDamage, hitDamageInfo);
			
			beHurtComp.OnBeHurt(hitDamageInfo);
		}

		private void aoeHit(Actor attacker, DamageInfo damageInfo) {
			//aoe会根据目标坐标二次筛选
			var targetIds = ActorManager.Instance.UseFilter(Actor, _filterSetting);
			var finalTargets = new List<BeHurtComp>();
			
			foreach (var targetUid in targetIds) {
				var target = ActorManager.Instance.GetActor(targetUid);
				if (target == null)
					continue;
				if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp))
					continue;
				finalTargets.Add(beHurtComp);
			}
			
			damageInfo.HitCount = finalTargets.Count;
			var hitInfo = makeHitInfo();
			hitInfo.HitBoxHitCount = finalTargets.Count;
			BattleEventManager.Instance.TriggerActorEvent(_sourceActorId, EBattleEventType.OnHit, hitInfo);
			
			foreach (var beHurtComp in finalTargets) {
				var damageItem = makeDamageConfig();
				var res = LuaInterface.GetDamageResults(attacker, beHurtComp.Actor, damageInfo, damageItem);
				
				var hitDamageInfo = new HitDamageInfo(hitInfo);
				hitDamageInfo.ParseDamageResult(res);
				hitDamageInfo.HitTargetUid = beHurtComp.Actor.Uid;
				
				if (beHurtComp.Actor.GetAttr<int>(ELogicAttr.AttrInvincible) != 0) {
					Debug.Log($"actor uid: {beHurtComp.Actor.Uid}  是无敌的");
					hitDamageInfo.IsImmunity = true;
				}
				hitDamageInfo.IsKillTarget = (beHurtComp.Actor.GetAttr<int>(ELogicAttr.AttrHp) - res.DamageValue) <= 0;
				Debug.Log($"[OnHit] hitBoxUid {Uid}: AttackUid{attacker.Uid} -----> targetUid {beHurtComp.Actor.Uid}");
				BattleEventManager.Instance.TriggerActorEvent(_sourceActorId, EBattleEventType.OnHitDamage, hitDamageInfo);
				beHurtComp.OnBeHurt(hitDamageInfo);
			}
		}

		private void onHit() {
			var attacker = ActorManager.Instance.GetActor(_sourceActorId);

			var damageInfo = new DamageInfo();
			damageInfo.SourceAbilityConfigId = _sourceAbilityConfigId;
			damageInfo.SourceAbilityType = _sourceAbilityType;
			var tags = (List<int>)Variables.Get("abilityTags");
			if (tags != null) {
				damageInfo.Tags.AddRange(tags);
			}
			else {
				Debug.Log("tag == null");
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