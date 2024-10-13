namespace Hono.Scripts.Battle
{
    public class NormalModelController : ActorModelController
    {
        private readonly ModelSetup _modelSetup; 
            
        public NormalModelController(Actor actor, SceneActorModel model) : base(actor)
        {
            if (model == null)
            {
                _modelSetup = new AsyncLoadModelSetup();
            }
            else
            {
                _modelSetup = new SceneModelSetup(model);
            }
        }
        
        protected override ModelSetup getModelSetup()
        {
            return _modelSetup;
        }
    }
}