using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools {
	public class ActorRefreshPoint : MonoBehaviour {
		public EActorType PointCreateType;
		public int ConfigId;
		
		[Button("创建Actor")]
		public void CreateActor() {
			var actor = ActorManager.Instance.CreateActor(PointCreateType, ConfigId);
			actor.Logic.SetAttr(ELogicAttr.AttrPosition, transform.position, false);
			actor.Logic.SetAttr(ELogicAttr.AttrRot, transform.rotation, false);
			ActorManager.Instance.AddActor(actor);
		}
	}
}