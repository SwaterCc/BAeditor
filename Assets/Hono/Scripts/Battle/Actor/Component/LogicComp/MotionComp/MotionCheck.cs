using Hono.Scripts.Battle.Tools;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle {
	public class MotionCheck : MonoBehaviour {

		public ActorModel model;

		private ActorLogic.MotionComp _motionComp;

		private bool _openCheck;

		public void Start() {
			if (!ActorManager.Instance.TryGetActor(model.ActorUid, out var actor)) {
				return;
			}

			if (!actor.Logic.TryGetComponent(out _motionComp)) {
				_openCheck = false;
				
			}
			else {
				_motionComp.MotionAdd += onMotionAdd;
				_motionComp.MotionRemove += onMotionRemove;
				_openCheck = _motionComp.HasMotion();
			}
		}

		private void onMotionAdd(ActorLogic.Motion motion) {
			_openCheck = _motionComp.HasMotion();
		}

		private void onMotionRemove(ActorLogic.Motion motion) {
			_openCheck = _motionComp.HasMotion();
		}
		
		public void OnTriggerEnter(Collider other) {
			if(!_openCheck) return;
			
			if (!other.gameObject.TryGetComponent<ActorModel>(out var otherHandle)) {
				return;
			}
			if (!ActorManager.Instance.TryGetActor(model.ActorUid, out var actor)) {
				return;
			}

			if (actor.Logic.TryGetComponent<ActorLogic.MotionComp>(out var comp)) {
				comp.OnCollision(otherHandle.ActorUid);
			}
		}
	}
}