using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class HitBoxLogic : ActorLogic {
		private HitBoxData _hitBoxConfig;
		private float _duration;
		private float _effectDuration;
		private float _intervalDuration;
		private bool _isInEffect;
		private int _count;
		private Dictionary<BeHurtComp, int> _hitCountDict = new();

		public HitBoxLogic(int configId) : base(configId) { }

		protected override void onInit() {
			
			_duration = 0;
			_intervalDuration = _hitBoxConfig.Interval;
			_count = 0;
			_isInEffect = false;
		}

		protected override void registerChildComp() { }


		private void hit() {
			List<int> targets = null;
			switch (_hitBoxConfig.HitType) {
				case EHitType.Aoe:
					targets = CommonUtility.CheckAABB(_hitBoxConfig.AoeData, Vector3.one);
					break;
				case EHitType.LockTarget:
					break;
			}

			if (targets == null) return;

			var damageInfo = new DamageInfo();
			var source = this.GetSimpleAttr<int>(EAttributeType.Source).GetValue();
			var attacker = ActorManager.Instance.GetActor(source).ActorLogic;
			foreach (var target in targets) {
				var targetLogic = ActorManager.Instance.GetActor(target).ActorLogic;
				var behitComp = targetLogic.GetComponent<BeHurtComp>();
				if (_hitCountDict.TryGetValue(behitComp,out var count)) {
					if (count >= _hitBoxConfig.ValidCount) {
						continue;
					}
				}
				else {
					_hitCountDict.Add(behitComp, 1);
				}
				var res = DamageLuaInterface.GetDamageResults(attacker, targetLogic, damageInfo);
				behitComp.OnBeHurt(res);
			}
		}

		protected override void onTick(float dt) {
			//有效时间
			if (_duration >= _hitBoxConfig.DelayTime && _duration <= _hitBoxConfig.DelayTime + _hitBoxConfig.Duration) {
				if (_intervalDuration >= _hitBoxConfig.Interval) {
					_intervalDuration = 0;
					_effectDuration = 0;
					++_count;
					_isInEffect = true;
				}

				if (_isInEffect) {
					if (_effectDuration <= _hitBoxConfig.EffectTime && _count <= _hitBoxConfig.ValidCount) {
						_effectDuration += dt;
						hit();
					}
					else {
						_isInEffect = false;
					}
				}
				
				if (!_isInEffect) {
					_intervalDuration += dt;
				}
			}
			
			_duration += dt;

			if (_duration >= _hitBoxConfig.Duration + _hitBoxConfig.DelayTime) { }
		}
	}
}