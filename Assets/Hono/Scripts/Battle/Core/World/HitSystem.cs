using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.ObjectPool;
using UnityEngine;

namespace Hono.Scripts.Battle.Core {
	public class HitSystem : Singleton<HitSystem> {
		/// <summary>
		/// 命中的目标
		/// </summary>
		private List<int> _hitTargets = new(1000);

		/// <summary>
		/// 单体打击点，直接Hit目标
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="damageSourceType"></param>
		/// <param name="sourceAbilityId"></param>
		/// <param name="disableEventTrigger"></param>
		/// <param name="damageId"></param>
		public void SingleHit(Unit attacker,
			Unit target,
			EDamageSourceType damageSourceType,
			int sourceAbilityId,
			bool disableEventTrigger = false,
			int damageId = 0) {
			return;
			if (attacker == null) {
				return;
			}

			if (target == null) {
				return;
			}

			//Debug.Log($"[HitSystem] SingleHit attacker:{attacker} target:{target} damageSource:{damageSourceType} abilityId:{sourceAbilityId} damageId:{damageId}");

			if (!disableEventTrigger) {
				var board = GPool<VariableBoard>.Pool.Rent();
				board.SetEvtField(HitInfoKey.AttackerUid,      attacker.Uid);
				board.SetEvtField(HitInfoKey.DamageSourceType, damageSourceType);
				board.SetEvtField(HitInfoKey.SourceAbilityId,  sourceAbilityId);
				board.SetEvtField(HitInfoKey.DamageConfigId,   damageId);
				board.SetEvtField(HitInfoKey.HitBoxHitCount,   1);
				var gList = GPool<GList<int>>.Pool.Rent();
				gList.Add(target.Uid);
				board.SetEvtField(HitInfoKey.HitTargetUid, gList);
				EventManager.Instance.FireWorldEvent(EEventType.OnHit, board);
				GPool<VariableBoard>.Pool.Recycle(board);
			}

			var damageResult = GPool<VariableBoard>.Pool.Rent();

			if (damageId == 0) {
				return;
			}

			if (!ConfigDataBase.Table<DamageTable>().TryGetRow(damageId, out var damageRow)) {
				return;
			}

			if (target.TryGetComponent(out HpComp hpComp)) {
				var hitInfo = new HitInfo() {
					Attacker = attacker,
					DamageRow = damageRow,
					DamageSourceType = damageSourceType,
					HitCount = 1,
					SourceAbilityId = sourceAbilityId
				};
				hpComp.MakeDamage(hitInfo);
			}

			if (target.TryGetComponent(out ElementComp combatComp)) {
				combatComp.CumulativeElementValue(attacker, damageRow);
			}

			GPool<VariableBoard>.Pool.Recycle(damageResult);
		}

		/// <summary>
		/// 对目标单体造成固定伤害
		/// </summary>
		/// <param name="target"></param>
		/// <param name="damageValue"></param>
		/// <param name="damageType"></param>
		/// <param name="disableEventTrigger"></param>
		public void SingleHitFixDamage(
			Unit target,
			int damageValue,
			EDamageType damageType,
			bool disableEventTrigger = true) {
			if (target == null) {
				return;
			}

			if (!disableEventTrigger) {
				var board = GPool<VariableBoard>.Pool.Rent();

				EventManager.Instance.FireWorldEvent(EEventType.OnHit, board);
				GPool<VariableBoard>.Pool.Recycle(board);
			}

			var damageResult = GPool<VariableBoard>.Pool.Rent();

			if (target.TryGetComponent(out HpComp hpComp)) {
				hpComp.MakeFixDamageValue(damageValue, damageType);
			}

			GPool<VariableBoard>.Pool.Recycle(damageResult);
		}

		/// <summary>
		/// 对范围敌人Hit
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="aoeCenterPos"></param>
		/// <param name="yAngle"></param>
		/// <param name="damageSourceType"></param>
		/// <param name="sourceAbilityId"></param>
		/// <param name="aoeSetting"></param>
		/// <param name="disableEventTrigger"></param>
		/// <param name="damageId"></param>
		public void AreaHit(Unit attacker,
			Vector3 aoeCenterPos,
			float yAngle,
			EDamageSourceType damageSourceType,
			int sourceAbilityId,
			RangeFilterSetting aoeSetting,
			bool disableEventTrigger = false,
			int damageId = 0) {
			if (attacker == null) {
				return;
			}
		
			World.Query.SearchUnits(attacker, aoeCenterPos, yAngle, aoeSetting, ref _hitTargets);

			if (_hitTargets.Count == 0) {
				return;
			}
			
#if UNITY_EDITOR
			foreach (var hitUid in _hitTargets) {
				//Debug.Log($"[HitSystem] SingleHit attacker:{attacker} target:{hitUid} damageSource:{damageSourceType} abilityId:{sourceAbilityId} damageId:{damageId}");
			}
#endif
			
			if (!disableEventTrigger) {
				var board = GPool<VariableBoard>.Pool.Rent();
				board.SetEvtField(HitInfoKey.AttackerUid,      attacker.Uid);
				board.SetEvtField(HitInfoKey.DamageSourceType, damageSourceType);
				board.SetEvtField(HitInfoKey.SourceAbilityId,  sourceAbilityId);
				board.SetEvtField(HitInfoKey.DamageConfigId,   damageId);
				board.SetEvtField(HitInfoKey.HitBoxHitCount,   1);
				var gList = GPool<GList<int>>.Pool.Rent();
				gList.AddRange(_hitTargets);
				board.SetEvtField(HitInfoKey.HitTargetUid, gList);
				EventManager.Instance.FireWorldEvent(EEventType.OnHit, board);
				GPool<VariableBoard>.Pool.Recycle(board);
			}

			var damageResult = GPool<VariableBoard>.Pool.Rent();

			if (damageId == 0) {
				return;
			}

			if (!ConfigDataBase.Table<DamageTable>().TryGetRow(damageId, out var damageRow)) {
				return;
			}

			foreach (var targetUid in _hitTargets) {
				var target = World.Query.GetUnit(targetUid);
				if (target.TryGetComponent(out HpComp hpComp)) {
					var hitInfo = new HitInfo() {
						Attacker = attacker,
						DamageRow = damageRow,
						DamageSourceType = damageSourceType,
						HitCount = _hitTargets.Count,
						SourceAbilityId = sourceAbilityId
					};
					hpComp.MakeDamage(hitInfo);
				}

				if (target.TryGetComponent(out ElementComp combatComp)) {
					combatComp.CumulativeElementValue(attacker, damageRow);
				}
			}

			_hitTargets.Clear();
			GPool<VariableBoard>.Pool.Recycle(damageResult);
		}


		/// <summary>
		/// 对范围敌人Hit产生固定伤害
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="aoeCenterPos"></param>
		/// <param name="yAngle"></param>
		/// <param name="damageValue"></param>
		/// <param name="damageType"></param>
		/// <param name="aoeSetting"></param>
		/// <param name="disableEventTrigger"></param>
		public void AreaHitMakeFixDamage(Unit attacker,
			Vector3 aoeCenterPos,
			float yAngle,
			int damageValue,
			EDamageType damageType,
			RangeFilterSetting aoeSetting,
			bool disableEventTrigger = true) {
			if (attacker == null) {
				return;
			}

			World.Query.SearchUnits(attacker, aoeCenterPos, yAngle, aoeSetting, ref _hitTargets);

			if (_hitTargets.Count == 0) {
				return;
			}

			if (!disableEventTrigger) {
				var board = GPool<VariableBoard>.Pool.Rent();
				EventManager.Instance.FireWorldEvent(EEventType.OnHit, board);
				GPool<VariableBoard>.Pool.Recycle(board);
			}

			var damageResult = GPool<VariableBoard>.Pool.Rent();


			foreach (var targetUid in _hitTargets) {
				var target = World.Query.GetUnit(targetUid);
				if (target.TryGetComponent(out HpComp hpComp)) {
					hpComp.MakeFixDamageValue(damageValue, damageType);
				}
			}

			_hitTargets.Clear();
			GPool<VariableBoard>.Pool.Recycle(damageResult);
		}
	}
}