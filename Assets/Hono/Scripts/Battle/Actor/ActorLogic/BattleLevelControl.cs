using UnityEngine;

namespace Hono.Scripts.Battle {
	//当前游戏模式
	public class BattleLevelControl : ActorLogic {
		private VFXWorldComp _vfxWorldComp;
		
		public bool AutoUltimateSkill;
		
		public EBattleModeType BattleModeType;
		
		public BattleLevelControl(Actor actor) : base(actor) { }
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