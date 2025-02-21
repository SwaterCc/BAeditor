#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Core {
	[JsonUnitCompCtorParams(typeof(HateComp))]
	public class HateCompCtorParams : UnitCompCtorParams {
		/// <summary>
		/// 主动选择仇恨目标
		/// </summary>
		public bool ActiveTargetSelection;
		/// <summary>
		/// 主动仇恨半径
		/// </summary>
		public float ActiveHateRange;
		/// <summary>
		/// 追踪半径
		/// </summary>
		public float FollowHateTargetRange;
		/// <summary>
		/// 无仇恨目标时选择伤害自身的单位
		/// </summary>
		public bool BeHurtHateAttackerWhenNoHateTarget = true;
		/// <summary>
		/// 切换仇恨目标最低伤害
		/// </summary>
		public int SwitchHateDamageValue;
	}

	/// <summary>
	/// 仇恨组件，产生仇恨目标
	/// 主动仇恨，被动仇恨
	/// </summary>
	[JsonUnitComponent]
	public class HateComp : UnitComponent, IGPoolObject {
		/// <summary>
		/// 主动选择仇恨目标
		/// </summary>
		private bool _activeTargetSelection;
		/// <summary>
		/// 无仇恨目标时选择伤害自身的单位
		/// </summary>
		private bool _beHurtHateAttackerWhenNoHateTarget = true;
		/// <summary>
		/// 切换仇恨目标最低伤害
		/// </summary>
		private int _switchHateDamageValue;
		/// <summary>
		/// 主动仇恨半径
		/// </summary>
		private float _activeHateRange;
		/// <summary>
		/// 追踪半径
		/// </summary>
		private float _followHateTargetRange;
		/// <summary>
		/// 当前仇恨目标
		/// </summary>
		private Unit _hateTarget;
		/// <summary>
		/// 上次选择目标的时间
		/// </summary>
		private float _beforeSelectHateTime;
		private EventListener _behurtListener;

		/// <summary>
		/// 当前仇恨目标
		/// </summary>
		public int HateTargetUid => _hateTarget != null ? _hateTarget.Uid : 0;

		public override void Ctor(UnitCompCtorParams ctorParams) {
			var hateCtorParams = (HateCompCtorParams)ctorParams;
			_activeTargetSelection = hateCtorParams.ActiveTargetSelection;
			_beHurtHateAttackerWhenNoHateTarget = hateCtorParams.BeHurtHateAttackerWhenNoHateTarget;
			_switchHateDamageValue = hateCtorParams.SwitchHateDamageValue;
			_activeHateRange = hateCtorParams.ActiveHateRange;
			_followHateTargetRange = hateCtorParams.FollowHateTargetRange;
		}

		public override void Init() {
			if (_activeTargetSelection) {
				selectHateTarget();
				if (_hateTarget != null) {
					_beforeSelectHateTime = World.Current.RealWorldTimeSinceStart;
				}
			}

			_behurtListener = new EventListener(EEventType.OnBeHurt, beHurtHate);
			Unit.AddUnitEvtListener(_behurtListener);
		}

		protected override void onTick(float dt) {
			//主动选择仇恨
			if (_activeTargetSelection && _hateTarget == null) {
				if (World.Current.RealWorldTimeSinceStart - _beforeSelectHateTime < 1f) {
					//1秒检测间隔
					return;
				}

				selectHateTarget();
			}
		}

		public override void Recycle() {
			GPool<HateComp>.Pool.Recycle(this);
		}

		private void selectHateTarget() {
			throw new System.NotImplementedException();
		}

		private void beHurtHate(VariableBoard variableBoard) {
			if (_hateTarget == null && _beHurtHateAttackerWhenNoHateTarget) {
				var attacker = variableBoard.GetEvtField(BeHurtInfoKeys.AttackerUid);
				_hateTarget = World.Query.GetUnit(attacker);
				return;
			}

			var damageValue = variableBoard.GetEvtField(BeHurtInfoKeys.FinalDamageValue);
			if (_hateTarget != null && damageValue > _switchHateDamageValue) {
				var attacker = variableBoard.GetEvtField(BeHurtInfoKeys.AttackerUid);
				_hateTarget = World.Query.GetUnit(attacker);
			}
		}

		public void OnRecycle() {
			_hateTarget = null;
			_activeTargetSelection = false;
			_beHurtHateAttackerWhenNoHateTarget = true;
			_switchHateDamageValue = 0;
		}
	}
}