namespace Hono.Scripts.Battle {
	public class TriggerBoxLogic : ActorLogic, IAPoolObject {
		protected override void onInit() {
			if (Self.ModelController.Model.TryGetComponent<TriggerBoxModel>(out var triggerBoxModel)) {
				foreach (var abilityId in triggerBoxModel.AbilityIds) {
					var ability = Self.Abilities.AwardAbility(abilityId);
					ability.Execute();
				}
			}
		}

		public override void RecycleLogicObject() {
			AObjectPool<TriggerBoxLogic>.Pool.Recycle(this);
		}
	}
}