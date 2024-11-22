#region

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public enum EVFXType {
		InWorld,
		FollowActor,
		BindActorBone,
	}

	[Serializable]
	public class VFXSetting {
		[FilePath(Extensions = "prefab")] public string VFXPath;

		public Vector3 Offset;
		public Vector3 Rot;
		public float Scale = 1;

		public EVFXType VFXBindType;

		[ShowIf("VFXBindType", EVFXType.BindActorBone)] [ValueDropdown("GetBindBoneNames")]
		public string BoneName = "";

		[LabelText("特效持续时长 -1为永久")] public float Duration;


		private static IEnumerable<string> GetBindBoneNames() {
			List<string> BindList = new() { "effect_point" };
			return BindList;
		}
	}
}