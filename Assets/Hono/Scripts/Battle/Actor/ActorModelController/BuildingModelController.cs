namespace Hono.Scripts.Battle
{
    public class BuildingModelController : ActorModelController , IPoolObject
    {
        private BuildingActorModel _buildingActorModel;
        private readonly CustomModelSetup _customModelSetup = new();
        private readonly AsyncLoadModelSetup _asyncLoadModelSetup = new();
        
        public void Init(Actor actor, BuildingActorModel buildingActorModel)
        {
            base.Init(actor);
            _buildingActorModel = buildingActorModel;
        }

        protected override ModelSetup getModelSetup()
        {
            if (_buildingActorModel == null)
            {
                return _asyncLoadModelSetup;
            }
            _customModelSetup.SetPath(_buildingActorModel.Path);
            _customModelSetup.SetModel(_buildingActorModel.gameObject);
                
            return _customModelSetup;
        }

        protected override void onEnterScene()
        {
            if (_buildingActorModel)
            {
                _buildingActorModel.ActorUid = Uid;
                _buildingActorModel.ActorType = EActorType.Building;
            }
            Actor.SetAttr(ELogicAttr.AttrPosition, Model.transform.position, false);
            Actor.SetAttr(ELogicAttr.AttrRot, Model.transform.rotation, false);
        }

        protected override void RecycleSelf()
        {
            AObjectPool<BuildingModelController>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
            _buildingActorModel = null;
        }
    }
}