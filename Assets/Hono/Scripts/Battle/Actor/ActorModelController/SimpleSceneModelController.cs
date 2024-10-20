namespace Hono.Scripts.Battle
{
    public class SimpleSceneModelController : ActorModelController , IPoolObject
    {
        private readonly SceneModelSetup _modelSetup = new();
        private SceneActorModel _sceneActorModel;

        public void Init(Actor actor, SceneActorModel sceneActorModel)
        {
            base.Init(actor);
            _modelSetup.Init(sceneActorModel);
            _sceneActorModel = sceneActorModel;
        }

        protected override ModelSetup getModelSetup()
        {
            return _modelSetup;
        }

        protected override void onModelLoadComplete()
        {
            Actor.SetAttr(ELogicAttr.AttrPosition, Model.transform.position, false);
            Actor.SetAttr(ELogicAttr.AttrRot, Model.transform.rotation, false);
            if (_sceneActorModel != null)
            {
                _sceneActorModel.OnModelSetupFinish(Actor);
            }
        }

        protected override void RecycleSelf()
        {
            AObjectPool<SimpleSceneModelController>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
        }
    }
}