using Hono.Scripts.Battle.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.DebugTools {
	public class UnitDebugInspector : MonoBehaviour {
		private Unit _unit;
		private BuffComp _buffComp;
		private HpComp _hpComp;
		private CombatComp _combatComp;

		[ReadOnly]
		public float currentHp;

		[ReadOnly]
		public int attrValue;
		public EAttrType listenerAttrType;

		[ReadOnly]
		public List<int> tags = new();
		
		[LabelText("buff列表更新间隔")]
		public float buffsUpdateInterval = 1;
		[ReadOnly]
		[ShowInInspector]
		public List<Buff> Buffs = new();
		private float _beforeBuffsUpdateTime;

		[LabelText("Energy列表更新间隔")]
		public float energysUpdateInterval = 1;
		[ShowInInspector]
		public Dictionary<int, int> EnergyList = new();
		private float _beforeEnergysUpdateTime;

		private void Start() {
			if (TryGetComponent<UOProxyPhysicsHandler>(out var handler)) {
				_unit = handler.Unit;
			}
		}

		[Button("绑定Unit")]
		public void BindUnit(int unitUId) {
			_unit = World.Query.GetUnit(unitUId);
			if (_unit == null) {
				Debug.LogError($"找不到Unit{unitUId}");
				return;
			}
			_unit.Tags.GetSnapshot(ref tags);

			if (!_unit.TryGetComponent(out _buffComp)) {
				Debug.Log($"Unit{unitUId}身上没有BuffComp");
				return;
			}

			if (!_unit.TryGetComponent(out _hpComp)) {
				Debug.Log($"Unit{unitUId}身上没有HpComp");
				return;
			}
			
			if (!_unit.TryGetComponent(out _combatComp)) {
				Debug.Log($"Unit{unitUId}身上没有CombatComp");
				return;
			}
		}

		public void Update() {
			if (_unit == null)
				return;
			
			if (listenerAttrType != 0) {
				attrValue = _unit.GetAttr(listenerAttrType);
				_unit.Tags.GetSnapshot(ref tags);
			}

			if (_hpComp != null) {
				updateHp();
			}

			if (_buffComp != null) {
				updateBuffList();
			}

			if (_combatComp != null) {
				updateCombatEnergy();
			}
		}

		private void updateHp() {
			currentHp = _hpComp.CurrentHp;
		}

		private void updateCombatEnergy() {
			if (energysUpdateInterval > 0) {
				if (Time.realtimeSinceStartup - _beforeEnergysUpdateTime > energysUpdateInterval) {
					_combatComp.GetEnergyList(ref EnergyList);
				}
			}
			else {
				_combatComp.GetEnergyList(ref EnergyList);
			}
		}
		
		private void updateBuffList() {
			if (buffsUpdateInterval > 0) {
				if (Time.realtimeSinceStartup - _beforeBuffsUpdateTime > buffsUpdateInterval) {
					_buffComp.GetBuffList(ref Buffs);
				}
			}
			else {
				_buffComp.GetBuffList(ref Buffs);
			}
		}
	}
}