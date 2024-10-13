namespace Hono.Scripts.Battle
{
    public class SimplePreLoadModelController : ActorModelController
    {
        private readonly ModelSetup _modelSetup;

        public SimplePreLoadModelController(Actor actor, EPreLoadGameObjectType objectType) : base(actor)
        {
            _modelSetup = new PreLoadModelSetup(objectType);
        }
        
        protected override ModelSetup getModelSetup()
        {
            return _modelSetup;
        }
    }
}