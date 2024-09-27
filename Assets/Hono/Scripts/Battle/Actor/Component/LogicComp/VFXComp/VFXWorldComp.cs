using System;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class VFXWorldComp : VFXComp {
			public VFXWorldComp(ActorLogic logic) : base(logic) { }
			
			public new Type GetType() {
				return typeof(VFXComp);
			}
			
			public void AddVFXObjectToWorld(VFXObject obj) {
				AddVFXToList(obj);
			}
		}
	}
}