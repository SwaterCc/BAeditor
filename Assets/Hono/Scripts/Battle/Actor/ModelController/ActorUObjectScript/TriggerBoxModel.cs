#region

using MagicaCloth2;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public class TriggerBoxModel : ActorModel {
		[LabelText("拥有的Ability")] 
		public List<int> AbilityIds = new();
		[LabelText("默认为开启状态")] 
		public bool IsDefaultOpen;
		private readonly Dictionary<int, float> _stayDict = new(32);
		
		private void OnTriggerEnter(Collider other) {
			if (!SetupFinish) return;
			if (other.TryGetComponent<ActorModel>(out var actorModel)) {
				//_triggerBoxController.OnTriggerEnter(actorModel.ActorUid);
				_stayDict.TryAdd(actorModel.ActorUid, 0);
			}
		}

		private void OnTriggerExit(Collider other) {
			if (!SetupFinish) return;
			if (other.TryGetComponent<ActorModel>(out var actorModel)) {
				_stayDict.Remove(actorModel.ActorUid);
			}
		}
	}
}