namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class Buff {
			public int SourceId => _sourceId;

			public int ConfigId => _data.id;
			private int _sourceId;
			private ActorLogic _logic;
			private BuffData _data;
			private int _abilityUid;

			private int _layer;
			public int LayerCount => _layer;

			public Buff(ActorLogic logic, int sourceId, BuffData data) {
				_logic = logic;
				_sourceId = sourceId;
				_data = data;
				_layer = data.InitLayer;
			}

			public void OnAdd() {
				_abilityUid = _logic._abilityController.AwardAbility(_data.id, true);
			}

			public void AddLayer(int layerCount) {
				_layer += layerCount;
				_logic._abilityController.ExecutingAbilityForce(ConfigId);
			}

			public void OnRemove() {
				_logic._abilityController.RemoveAbility(_abilityUid);
			}
		}
	}
}