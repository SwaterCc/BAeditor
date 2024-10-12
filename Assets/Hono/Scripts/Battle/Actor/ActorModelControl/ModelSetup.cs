namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public abstract class ModelSetup
        {
            protected readonly ActorModelController _modelController;
        
            protected ModelSetup(ActorModelController modelController)
            {
                _modelController = modelController;
            }

            public abstract void SetupModel();
        }
    }
}