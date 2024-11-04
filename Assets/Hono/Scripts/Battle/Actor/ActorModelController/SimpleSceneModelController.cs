namespace Hono.Scripts.Battle
{
    public class SimpleSceneModelController : ActorModelController
    {
        private readonly ModelSetup _modelSetup;

        public SimpleSceneModelController(Actor actor, SceneActorModel sceneActorModel) : base(actor)
        {
            _modelSetup = new SceneModelSetup(sceneActorModel);
        }

        protected override ModelSetup getModelSetup()
        {
            return _modelSetup;
        }
    }
}