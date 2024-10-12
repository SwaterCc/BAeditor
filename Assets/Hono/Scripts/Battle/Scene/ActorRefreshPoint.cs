using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Scene {
	public class ActorRefreshPoint : MonoBehaviour {
		public EActorType PointCreateType;
		public int ConfigId;
		
		[Button("创建Actor")]
		public void CreateActor() {
			/*var actor = ActorManager.Instance.CreateActor(PointCreateType, ConfigId);
			actor.Logic.SetAttr(ELogicAttr.AttrPosition, transform.position, false);
			actor.Logic.SetAttr(ELogicAttr.AttrRot, transform.rotation, false);
			ActorManager.Instance.AddActor(actor);*/
		}
	}
}