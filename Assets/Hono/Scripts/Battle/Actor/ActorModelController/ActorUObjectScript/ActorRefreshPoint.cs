using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class ActorRefreshPoint  : SceneActorModel {
		[ReadOnly]
		public EActorType CreateActorType;
		[ReadOnly]
		public int ConfigId;
		
		[InfoBox("目前只支持 Pawn Monster Building 三种类型")]
		
		[Button("设置运行时创建对象的信息")]
		public void SetActorInfo(EActorType actorType,int configId = 0) {
			CreateActorType = actorType;
			ConfigId = configId;
		}
		
		protected override void onInit() {
			ActorType = EActorType.ActorRefreshPoint;
		}
	}
}