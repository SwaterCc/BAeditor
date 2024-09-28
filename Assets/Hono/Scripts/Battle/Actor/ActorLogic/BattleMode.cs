using UnityEngine;

namespace Hono.Scripts.Battle {
	//当前游戏模式
	public class BattleMode : ActorLogic {
		private VFXWorldComp _vfxWorldComp;
		public BattleMode(Actor actor) : base(actor) { }
		protected override void setupAttrs() { }

		protected override void onInit() { }

		protected override void setupComponents() {
			_vfxWorldComp = new VFXWorldComp(this);
			addComponent(_vfxWorldComp);
		}

		public void AddWorldVFX(VFXObject obj) {
			_vfxWorldComp.AddVFXObjectToWorld(obj);
		}
	}
}