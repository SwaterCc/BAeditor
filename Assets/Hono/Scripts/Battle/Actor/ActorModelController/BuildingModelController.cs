namespace Hono.Scripts.Battle {
	public class BuildingModelController : ActorModelController {
		private readonly BuildingActorModel _buildingActorModel;

		public BuildingModelController(Actor actor, BuildingActorModel buildingActorModel) : base(actor) {
			_buildingActorModel = buildingActorModel;
		}
		protected override ModelSetup getModelSetup() {
			if (_buildingActorModel != null) {
				Model = _buildingActorModel.gameObject;
				IsModelLoadFinish = true;
				onModelLoadFinish();
				return null;
			}
			
			return new AsyncLoadModelSetup();
		}

		public override void OnEnterScene() {
			if (_buildingActorModel) {
				_buildingActorModel.ActorUid = Uid;
				_buildingActorModel.ActorType = EActorType.Building;
			}
			else {
				base.OnEnterScene();
			}
		}
	}
}