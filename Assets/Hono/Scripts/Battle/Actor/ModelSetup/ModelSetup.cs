namespace Hono.Scripts.Battle
{
    public partial class ActorModelController
    {
        public abstract class ModelSetup
        {
            public abstract void SetupModel(ActorModelController modelController);
        }
    }
}